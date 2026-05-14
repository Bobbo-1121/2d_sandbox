using Godot;
using System;

public class Chunk
{
    public struct Tile
    {
        private bool empty = false;
        public string Id;
        public float HitPoints;
        public Tile(string id)
        {
            Id = id;
        }
        public Tile()
        {
            this = Empty;
        }
        public static Tile Empty = new()
        {
            empty = true
        };
    }
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