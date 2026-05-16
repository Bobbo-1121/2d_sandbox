using Godot;
using System;

public class Chunk
{
    public readonly Vector2I Position;
    public Tile[,] MainTiles;
    public Tile[,] BackgroundTiles;
    public Chunk(Vector2I position)
    {
        Position = position;
        MainTiles = new Tile[16, 16];
        BackgroundTiles = new Tile[16, 16];
    }
}