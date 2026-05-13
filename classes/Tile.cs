using Godot;
using System;

public class Tile
{
    private static Tile[] tileArray;
    private static TileSet mainTileSet;
    public static TileSet GetTileSet()
    {
        return mainTileSet;
    }
    public static void CreateTile(TileSetAtlasSource source, Vector2I position)
    {
        //inner
        source.CreateTile(position);                                                                  //top left
        source.CreateTile(position + new Vector2I(1, 0));                                             //top right
        source.CreateTile(position + new Vector2I(0, 1));                                             //bottom left
        source.CreateTile(position + new Vector2I(1, 1));                                             //bottom right
        //fill
        source.CreateTile(position + new Vector2I(2, 0));                                             //top left
        source.CreateTile(position + new Vector2I(3, 0));                                             //top right
        source.CreateTile(position + new Vector2I(2, 1));                                             //bottom left
        source.CreateTile(position + new Vector2I(3, 1));                                             //bottom right
        //left
        source.CreateTile(position + new Vector2I(0, 2));                                             //top
        source.GetTileData(position + new Vector2I(0, 2), 0).TextureOrigin = new Vector2I(2, 0);
        source.CreateTile(position + new Vector2I(0, 3));                                             //bottom
        source.GetTileData(position + new Vector2I(0, 3), 0).TextureOrigin = new Vector2I(2, 0);
        //right
        source.CreateTile(position + new Vector2I(2, 2));                                             //top
        source.GetTileData(position + new Vector2I(2, 2), 0).TextureOrigin = new Vector2I(-2, 0);
        source.CreateTile(position + new Vector2I(2, 3));                                             //bottom
        source.GetTileData(position + new Vector2I(2, 3), 0).TextureOrigin = new Vector2I(-2, 0);
        //top
        source.CreateTile(position + new Vector2I(0, 4));                                             //left
        source.GetTileData(position + new Vector2I(0, 4), 0).TextureOrigin = new Vector2I(0, 2);
        source.CreateTile(position + new Vector2I(1, 4));                                             //right
        source.GetTileData(position + new Vector2I(1, 4), 0).TextureOrigin = new Vector2I(0, 2);
        //bottom
        source.CreateTile(position + new Vector2I(2, 4));                                             //left
        source.GetTileData(position + new Vector2I(2, 4), 0).TextureOrigin = new Vector2I(0, -2);
        source.CreateTile(position + new Vector2I(3, 4));                                             //right
        source.GetTileData(position + new Vector2I(3, 4), 0).TextureOrigin = new Vector2I(0, -2);
        //corners
        source.CreateTile(position + new Vector2I(0, 6));                                             //top left
        source.GetTileData(position + new Vector2I(0, 6), 0).TextureOrigin = new Vector2I(2, 2);
        source.CreateTile(position + new Vector2I(2, 6));                                             //top right
        source.GetTileData(position + new Vector2I(2, 6), 0).TextureOrigin = new Vector2I(-2, 2);
        source.CreateTile(position + new Vector2I(0, 8));                                             //bottom left
        source.GetTileData(position + new Vector2I(0, 8), 0).TextureOrigin = new Vector2I(2, -2);
        source.CreateTile(position + new Vector2I(2, 8));                                             //bottom right
        source.GetTileData(position + new Vector2I(2, 8), 0).TextureOrigin = new Vector2I(-2, -2);
    }
    public static void Init(string[] tileIds, int[] variantCounts, Image merged)
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
            for(int j = 0; j < variantCounts[i]; j++)
            {
                cursorPosition.X += 4;
                if (cursorPosition.X + 4 > source.GetAtlasGridSize().X)
                {
                    cursorPosition.X = 0;
                    cursorPosition.Y += 10;
                }
                CreateTile(source, cursorPosition);
            }
        }
    }
    public string Id;
    public string Name;
    public int VariantCount;
    public Vector2I TileSetPosition;
}
