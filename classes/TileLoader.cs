using Godot;
using System;
using System.Collections.Generic;

public class TileLoader
{
    public class LoadResult
    {
        public Image Merged;
        public int[] VariantCounts;
    }
    public static string[] GetVerifiedTiles(string dataDirectoryPath, string imageDirectoryPath, List<string> occupiedIds = null)
    {
        List<string> verifiedTiles = [];
        List<string> failed = [];
        List<string> failureReasons = [];
        if (occupiedIds == null)
        {
            occupiedIds = [];
        }
        DirAccess dataDirectory = DirAccess.Open(dataDirectoryPath);
        DirAccess imageDirectory = DirAccess.Open(imageDirectoryPath);
        if (dataDirectory == null || imageDirectory == null)
        {
            return null;
        }
        string[] tileDataFiles = dataDirectory.GetFiles();
        for(int i = 0; i < tileDataFiles.Length; i++)
        {
            Godot.Collections.Dictionary<string, Variant> json = JsonReader.StringToDictionary(JsonReader.LoadJsonString(dataDirectoryPath + "/" + tileDataFiles[i]));
            if (json == null)
            {
                failed.Add(tileDataFiles[i].Substr(0, tileDataFiles[i].Length - 5));
                failureReasons.Add("Failed to parse JSON");
                continue;
            }
            string id = JsonReader.ReadString(json, "id");
            if (id == null)
            {
                failed.Add(tileDataFiles[i]);
                failureReasons.Add("Id not found in JSON");
                continue;
            }
            if (occupiedIds.Contains(id))
            {
                failed.Add(id);
                failureReasons.Add("Id already occupied");
                continue;
            }
            if (!imageDirectory.FileExists(id + ".png"))
            {
                failed.Add(id);
                failureReasons.Add($"Image '{id}.png' file not found");
                continue;
            }
            Image image = GD.Load<Image>(imageDirectoryPath + "/" + id + ".png");
            if (image.GetWidth() % 16 != 0 || image.GetHeight() != 24)
            {
                failed.Add(id);
                failureReasons.Add("Image has incorrect size (should be 16n * 24)");
                continue;
            }
            occupiedIds.Add(id);
            verifiedTiles.Add(id);
        }
        string[] failedArray = [..failed];
        string[] failureReasonsArray = [..failureReasons];
        string failedPrint = "";
        for(int i = 0; i < failedArray.Length; i++)
        {
            failedPrint += $"[{failedArray[i]}] - {failureReasonsArray[i]}\n";
        }
        if (failedPrint != "")
        {
            Debug.Err("Failed to load " + failedArray.Length + " tiles:\n" + failedPrint);
        }
        return [..verifiedTiles];
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
    public static Image MergeImages(Image[] images)
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
    // public static LoadResult Load(string dataDirectoryPath, string imageDirectoryPath, List<string> occupiedIds = null)
    // {
    //     if (occupiedIds == null)
    //     {
    //         occupiedIds = [];
    //     }
    // }
}