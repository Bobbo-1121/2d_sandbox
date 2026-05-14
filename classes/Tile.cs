using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Tile
{
    private static Dictionary<string, Tile> tileDictionary = [];
    private static int tileCount = 0;
    public static Tile GetTileById(string id)
    {
        bool found = tileDictionary.TryGetValue(id, out Tile tile);
        return found ? tile : null;
    }
    public static string[] GetTileIds()
    {
        string[] ids = new string[tileCount];
        for(int i = 0; i < tileCount; i++)
        {
            ids[i] = tileDictionary.ElementAt(i).Value.Id;
        }
        return ids;
    }
    private static TileSet mainTileSet;
    public static TileSet dataTileSet;
    public static TileSet GetMainTileSet()
    {
        return mainTileSet;
    }
    public static void CreateTile(TileSetAtlasSource source, Vector2I position, int zIndex = 0)
    {
        //inner
        source.CreateTile(position);
        source.GetTileData(position, 0).ZIndex = zIndex;
        source.CreateTile(position + new Vector2I(1, 0));
        source.GetTileData(position + new Vector2I(1, 0) , 0).ZIndex = zIndex;
        source.CreateTile(position + new Vector2I(0, 1));
        source.GetTileData(position + new Vector2I(0, 1) , 0).ZIndex = zIndex;
        source.CreateTile(position + new Vector2I(1, 1));
        source.GetTileData(position + new Vector2I(1, 1) , 0).ZIndex = zIndex;
        //fill
        source.CreateTile(position + new Vector2I(2, 0));
        source.GetTileData(position + new Vector2I(2, 0) , 0).ZIndex = zIndex;
        source.CreateTile(position + new Vector2I(3, 0));
        source.GetTileData(position + new Vector2I(3, 0) , 0).ZIndex = zIndex;
        source.CreateTile(position + new Vector2I(2, 1));
        source.GetTileData(position + new Vector2I(2, 1) , 0).ZIndex = zIndex;
        source.CreateTile(position + new Vector2I(3, 1));
        source.GetTileData(position + new Vector2I(3, 1) , 0).ZIndex = zIndex;
        //left
        source.CreateTile(position + new Vector2I(0, 2), new Vector2I(2, 1));
        source.GetTileData(position + new Vector2I(0, 2), 0).TextureOrigin = new Vector2I(2, 0);
        source.GetTileData(position + new Vector2I(0, 2) , 0).ZIndex = zIndex;
        source.CreateTile(position + new Vector2I(0, 3), new Vector2I(2, 1));
        source.GetTileData(position + new Vector2I(0, 3), 0).TextureOrigin = new Vector2I(2, 0);
        source.GetTileData(position + new Vector2I(0, 3) , 0).ZIndex = zIndex;
        //right
        source.CreateTile(position + new Vector2I(2, 2), new Vector2I(2, 1));
        source.GetTileData(position + new Vector2I(2, 2), 0).TextureOrigin = new Vector2I(-2, 0);
        source.GetTileData(position + new Vector2I(2, 2) , 0).ZIndex = zIndex;
        source.CreateTile(position + new Vector2I(2, 3), new Vector2I(2, 1));
        source.GetTileData(position + new Vector2I(2, 3), 0).TextureOrigin = new Vector2I(-2, 0);
        source.GetTileData(position + new Vector2I(2, 2) , 0).ZIndex = zIndex;
        //top
        source.CreateTile(position + new Vector2I(0, 4), new Vector2I(1, 2));
        source.GetTileData(position + new Vector2I(0, 4), 0).TextureOrigin = new Vector2I(0, 2);
        source.GetTileData(position + new Vector2I(0, 4) , 0).ZIndex = zIndex;
        source.CreateTile(position + new Vector2I(1, 4), new Vector2I(1, 2));
        source.GetTileData(position + new Vector2I(1, 4), 0).TextureOrigin = new Vector2I(0, 2);
        source.GetTileData(position + new Vector2I(1, 4) , 0).ZIndex = zIndex;
        //bottom
        source.CreateTile(position + new Vector2I(2, 4), new Vector2I(1, 2));
        source.GetTileData(position + new Vector2I(2, 4), 0).TextureOrigin = new Vector2I(0, -2);
        source.GetTileData(position + new Vector2I(2, 4) , 0).ZIndex = zIndex;
        source.CreateTile(position + new Vector2I(3, 4), new Vector2I(1, 2));
        source.GetTileData(position + new Vector2I(3, 4), 0).TextureOrigin = new Vector2I(0, -2);
        source.GetTileData(position + new Vector2I(3, 4) , 0).ZIndex = zIndex;
        //corners
        source.CreateTile(position + new Vector2I(0, 6), new Vector2I(2, 2));
        source.GetTileData(position + new Vector2I(0, 6), 0).TextureOrigin = new Vector2I(2, 2);
        source.GetTileData(position + new Vector2I(0, 6) , 0).ZIndex = zIndex;
        source.CreateTile(position + new Vector2I(2, 6), new Vector2I(2, 2));
        source.GetTileData(position + new Vector2I(2, 6), 0).TextureOrigin = new Vector2I(-2, 2);
        source.GetTileData(position + new Vector2I(2, 6) , 0).ZIndex = zIndex;
        source.CreateTile(position + new Vector2I(0, 8), new Vector2I(2, 2));
        source.GetTileData(position + new Vector2I(0, 8), 0).TextureOrigin = new Vector2I(2, -2);
        source.GetTileData(position + new Vector2I(0, 8) , 0).ZIndex = zIndex;
        source.CreateTile(position + new Vector2I(2, 8), new Vector2I(2, 2));
        source.GetTileData(position + new Vector2I(2, 8), 0).TextureOrigin = new Vector2I(-2, -2);
        source.GetTileData(position + new Vector2I(2, 8) , 0).ZIndex = zIndex;
    }
    public static void Init(string[] tileIds, int[] variantCountById, Image merged, string dataDirectoryPath)
    {
        mainTileSet = new()
        {
            TileSize = new Vector2I(4, 4)
        };
        TileSetAtlasSource source = new()
        {
            Texture = ImageTexture.CreateFromImage(merged),
            TextureRegionSize = new Vector2I(4, 4)
        };
        Vector2I cursorPosition = Vector2I.Zero;
        for(int i = 0; i < tileIds.Length; i++)
        {
            Vector2I[] variantPositions = new Vector2I[variantCountById[i]];
            Godot.Collections.Dictionary<string, Variant> tileData = TileLoader.LoadData(dataDirectoryPath, tileIds[i]);
            JsonReader.NullInt zIndexNullable = JsonReader.ReadInt(tileData, "z-index");
            int zIndex = zIndexNullable != null ? zIndexNullable.value : 0;
            for(int j = 0; j < variantCountById[i]; j++)
            {
                if (cursorPosition.X + 4 > source.GetAtlasGridSize().X)
                {
                    cursorPosition.X = 0;
                    cursorPosition.Y += 10;
                }
                
                CreateTile(source, cursorPosition, zIndex);
                variantPositions[j] = cursorPosition;
                cursorPosition.X += 4;
            }
            new Tile(tileIds[i])
            {
                Name = JsonReader.ReadString(tileData, "name"),
                VariantCount = variantCountById[i],
                TileSetPositions = variantPositions
            };
        }
        mainTileSet.AddSource(source, 0);
        ResourceSaver.Save(mainTileSet, "res://main_tile_set.tres");
        dataTileSet = GD.Load<TileSet>("res://resources/data_tile_set.tres");
    }
    public Tile(string id)
    {
        this.Id = id;
        tileDictionary.Add(Id, this);
        tileCount++;
    }
    public string Id;
    public string Name;
    public int VariantCount;
    public Vector2I[] TileSetPositions;
}
