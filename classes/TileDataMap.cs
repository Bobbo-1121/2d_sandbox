using Godot;
using System;

public partial class TileDataMap : Node2D
{
    public class Chunk
    {
        private TileMapLayer tileMap;
        public Chunk(TileDataMap parent, TileSet tileSet)
        {
            tileMap = new();
            parent.AddChild(tileMap);
            tileMap.TileSet = tileSet;
            
        }
    }
    [Export]
    public TileSet TileSet;
    [Export]
    public int ChunkSize;
    private Chunk[] loadedChunks;

}
