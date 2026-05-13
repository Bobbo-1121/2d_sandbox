using Godot;
using System;
using System.Collections.Generic;

//    0. test if all the previous shi works (verification and id occupation and such (completely untested hopefully works :3))
//    0.5. fix verification buges
//    1. get verified tiles (method done)
//    2. format tiles (method done)
//    3. merge tiles (method done)
//--> 4. create tileset
//    5. make some sorta static dictionary to store tiles by id
//    6. create tiles and assign them tileset positions

public partial class TileRes : Node
{
    private static List<string> occupiedIds = [];
    public static void AddOccupiedId(string id)
    {
        if (!occupiedIds.Contains(id))
        {
            occupiedIds.Add(id);
        }
    }
    public static bool IsIdOccupied(string id)
    {
        return occupiedIds.Contains(id);
    }
    [Export]
    public string _TileDataDirectory = "res://data/tiles";
    [Export]
    public string _TileImagesDirectory = "res://images/tiles";
    private static string tileDataDirectory;
    private static string tileImagesDirectory;
    public static bool VerifyJsonStringValue(Godot.Collections.Dictionary<string, Variant> json, string key)
    {
        if (json.ContainsKey(key))
        {
            return json[key].VariantType == Variant.Type.String;
        }
        else
        {
            return false;
        }
    }
    public static bool VerifyJsonIntValue(Godot.Collections.Dictionary<string, Variant> json, string key)
    {
        if (json.ContainsKey(key))
        {
            return json[key].VariantType == Variant.Type.Int;
        }
        else
        {
            return false;
        }
    }
    public static string VerifyData(string path)
    {
        if (FileAccess.FileExists(path))
        {
            FileAccess dataFile = FileAccess.Open(path, FileAccess.ModeFlags.Read);
            string jsonString = dataFile.GetAsText();
            Json json = new();
            Error error = json.Parse(jsonString);
            if (error != Error.Ok)
            {
                return "Error while parsing tile data file at '" + path + "'!";
            }
            Godot.Collections.Dictionary<string, Variant> parsed = json.Data.AsGodotDictionary<string, Variant>();
            if (VerifyJsonStringValue(parsed, "id"))
            {
                if (IsIdOccupied(parsed["id"].AsString()))
                {
                    return "Duplicate tile data id '" + parsed["id"].AsString() + "' from json at '" + path + "'!";
                }
                else
                {
                    AddOccupiedId(parsed["id"].AsString());
                }
            }
            else
            {
                return "Tile data JSON at '" + path + "' doesn't contain id or id is not a string value!";
            }
            if (!VerifyJsonStringValue(parsed, "name"))
            {
                return "Tile data JSON at '" + path + "' doesn't contain name or name is not a string value!";
            }
            return null;
        }
        return "Tile data file at '" + path + "' doesn't exist!";
    }
    public static string VerifyImage(string path)
    {
        if (FileAccess.FileExists(path))
        {
            Image image = GD.Load<Image>(path);
            if (image.GetWidth() % 16 == 0 && image.GetHeight() == 24)
            {
                return null;
            }
            else
            {
                return "Tile image '" + path + "' has an incorrect size!";
            }
        }
        else
        {
            return "Tile image file at '" + path + "' doesn't exist!";
        }
    }
    public string[] GetVerifiedTiles(string dataDirectoryPath, string imageDirectoryPath)
    {
        DirAccess dataDirectory = DirAccess.Open(dataDirectoryPath);
        string[] data = dataDirectory.GetFiles();
        for(int i = 0; i < data.Length; i++)
        {
            if (data[i].Substr(data[i].Length - 4, 4) == "json")
            {
                data[i] = data[i].Substr(0, data[i].Length - 5);
            }
            else
            {
                Debug.Warn("Incorrect tile data file format: " + data[i]);
            }
        }
        List<string> verifiedDataList = [];
        for(int i = 0; i < data.Length; i++)
        {
            string error = VerifyData(dataDirectoryPath + "/" + data[i] + ".json");
            if (error != null)
            {
                Debug.Warn("Tile data verification: " + error);
            }
            else
            {
                verifiedDataList.Add(data[i]);
                Debug.Info("Added " + data[i] + " to verified data");
            }
        }
        string[] verifiedData = [.. verifiedDataList];
        List<string> verifiedTileList = [];
        for(int i = 0; i < verifiedData.Length; i++)
        {
            string error = VerifyImage(imageDirectoryPath + "/" + verifiedData[i] + ".png");
            if (error != null)
            {
                Debug.Warn("Tile texture image verification: " + error);
            }
            else
            {
                verifiedTileList.Add(verifiedData[i]);
                Debug.Info("Added " + verifiedData[i] + " to verified images");
            }
        }
        return [..verifiedTileList];
    }
    public static Image FormatTileImage(Image input)
    {
        if (input.GetWidth() % 16 != 0 || input.GetHeight() != 24)
        {
            return null;
        }
        int variants = input.GetWidth() / 16;
        Image result = Image.CreateEmpty(variants * 16, 40, false, Image.Format.Rgba8);
        for(int i = 0; i < variants; i++)
        {
            result.BlitRect(input, new Rect2I(i * 16, 16, 8, 8), new Vector2I(i * 16, 0));
            result.BlitRect(input, new Rect2I(4 + i * 16, 4, 8, 8), new Vector2I(8 + i * 16, 0));
            result.BlitRect(input, new Rect2I(i * 16, 4, 16, 8), new Vector2I(i * 16, 8));
            result.BlitRect(input, new Rect2I(4 + i * 16, 0, 8, 8), new Vector2I(i * 16, 16));
            result.BlitRect(input, new Rect2I(4 + i * 16, 8, 8, 8), new Vector2I(8 + i * 16, 16));
            result.BlitRect(input, new Rect2I(i * 16, 0, 16, 16), new Vector2I(i * 16, 24));
        }
        return result;
    }
    public static int MinMergedImageSize(int totalVariantCount)
    {
        for(int i = 4; i < 15; i++)
        {
            int size = (int)Math.Pow(2, i);
            int occupiedRows = (int)Math.Ceiling(totalVariantCount / (size / 16.0));
            if (occupiedRows * 40 <= size)
            {
                return size;
            }
        }
        Debug.Err("Tile image variant count is too high! How is this even possible bro???");
        return -1;
    }
    public static Image MergeTileImages(Image[] images)
    {
        int totalVariantCount = 0;
        for(int i = 0; i < images.Length; i++)
        {
            int variantCount = images[i].GetWidth() / 16;
            totalVariantCount += variantCount;
        }
        int mergedSize = MinMergedImageSize(totalVariantCount);
        Image merged = Image.CreateEmpty(mergedSize, mergedSize, false, Image.Format.Rgba8);
        Vector2I cursorPosition = Vector2I.Zero;
        for(int i = 0; i < images.Length; i++)
        {
            int variantCount = images[i].GetWidth() / 16;
            for(int j = 0; j < variantCount; j++)
            {
                merged.BlitRect(images[i], new Rect2I(j * 16, 0, 16, 40), new Vector2I(cursorPosition.X, cursorPosition.Y));
                cursorPosition.X += 16;
                if (cursorPosition.X + 16 > mergedSize)
                {
                    cursorPosition.Y += 40;
                    cursorPosition.X = 0;
                }
            }
        }
        return merged;
    }
    public override void _Ready()
    {
        base._Ready();
        // tileDataDirectory = _TileDataDirectory;
        // tileImagesDirectory = _TileImagesDirectory;
        // string[] verifiedTiles = GetVerifiedTiles(tileDataDirectory, tileImagesDirectory);
        // Image[] images = new Image[verifiedTiles.Length];
        // for(int i = 0; i < verifiedTiles.Length; i++)
        // {
        //     images[i] = FormatTileImage(GD.Load<Image>(tileImagesDirectory + "/" + verifiedTiles[i] + ".png"));
        // }
        // Image merged = MergeTileImages(images);
        // merged.SavePng("res://merged.png");
        // Debug.Info(verifiedTiles);
    }
}
