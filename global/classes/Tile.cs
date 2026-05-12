using Godot;
using System;
using System.Numerics;

public class Tile
{
    public struct Source
    {
        public Vector2I FillTopLeft;
        public Vector2I FillTopRight;
        public Vector2I FillBottomLeft;
        public Vector2I FillBottomRight;
        public Vector2I SideTopLeft;
        public Vector2I SideTopRight;
        public Vector2I SideBottomLeft;
        public Vector2I SideBottomRight;
        public Vector2I SideLeftTop;
        public Vector2I SideLeftBottom;
        public Vector2I SideRightTop;
        public Vector2I SideRightBottom;
        public Vector2I CornerTopLeft;
        public Vector2I CornerTopRight;
        public Vector2I CornerBottomLeft;
        public Vector2I CornerBottomRight;
        public Vector2I InnerTopLeft;
        public Vector2I InnerTopRight;
        public Vector2I InnerBottomLeft;
        public Vector2I InnerBottomRight;
    }
    private static Tile[] tileArray;
    private static TileSet tileSet;
    public static TileSet GetTileSet()
    {
        return tileSet;
    }
    public static void Init(string[] tileIds, int[] variantCounts, Image merged)
    {
        tileSet = new()
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
                Source tileSource = new()
                {
                    //HERE     <--     <--     <--     <--     <--     <--     <--     <--     <--     <--     <--     <--     HERE
                    FillTopLeft = new(cursorPosition.X, cursorPosition.Y),
                    FillTopRight = new(cursorPosition.X, cursorPosition.Y),
                    FillBottomLeft = new(cursorPosition.X, cursorPosition.Y),
                    FillBottomRight = new(cursorPosition.X, cursorPosition.Y),
                    SideTopLeft = new(cursorPosition.X, cursorPosition.Y),
                    SideTopRight = new(cursorPosition.X, cursorPosition.Y), 
                    SideBottomLeft = new(cursorPosition.X, cursorPosition.Y),
                    SideBottomRight = new(cursorPosition.X, cursorPosition.Y),
                    SideLeftTop = new(cursorPosition.X, cursorPosition.Y),
                    SideLeftBottom = new(cursorPosition.X, cursorPosition.Y),
                    SideRightTop = new(cursorPosition.X, cursorPosition.Y),
                    SideRightBottom = new(cursorPosition.X, cursorPosition.Y),
                    CornerTopLeft = new(cursorPosition.X, cursorPosition.Y),
                    CornerTopRight = new(cursorPosition.X, cursorPosition.Y),
                    CornerBottomLeft = new(cursorPosition.X, cursorPosition.Y),
                    CornerBottomRight = new(cursorPosition.X, cursorPosition.Y),
                    InnerTopLeft = new(cursorPosition.X, cursorPosition.Y),
                    InnerTopRight = new(cursorPosition.X, cursorPosition.Y),
                    InnerBottomLeft = new(cursorPosition.X, cursorPosition.Y),
                    InnerBottomRight = new(cursorPosition.X, cursorPosition.Y)
                };
                
                cursorPosition.X += 4;
                if (cursorPosition.X + 4 > source.GetAtlasGridSize().X)
                {
                    cursorPosition.X = 0;
                    cursorPosition.Y += 10;
                }
            }
        }
    }

    public string Id;
    public string Name;
    public int variantCount;
}
