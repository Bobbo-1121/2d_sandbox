using Godot;
using System;

public class ChunkTileMap
{
    public TileMapLayer MainTileMap;
    public readonly Vector2I Position;
    public ChunkTileMap(Vector2I position)
    {
        Position = position;
        MainTileMap = new()
        {
            Position = position * 16,
            TileSet = TileRes.GetMainTileSet()
        };
    }
    public void Update(Vector2I tilePosition, Tile tile, Tile above, Tile below, Tile left, Tile right)
    {
        
    }
}