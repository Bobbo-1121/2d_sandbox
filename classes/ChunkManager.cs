using Godot;
using System;
using System.Collections.Generic;

public class ChunkManager
{
    public readonly Node2D TileMapContainer;
    public ChunkManager(Node2D container)
    {
        TileMapContainer = container;
    }
    private Vector2 playerPosition;
    public void SetPlayerPosition(Vector2 position)
    {
        bool update = playerPosition != position;
        playerPosition = position;
        Update();
    }
    private int loadingDistance;
    public void SetLoadingDistance(int distance)
    {
        bool update = loadingDistance != distance;
        loadingDistance = distance;
        Update();
    }
    private List<Chunk> chunks = [];
    private List<ChunkTileMap> tileMaps = [];
    private void AddChunk()
    {
        
    }
    public List<Chunk> Chunks()
    {
        return chunks;
    }
    public void Update()
    {
        Debug.Info($"Currently loaded: {chunks.Count}");
        int newLoaded = 0;
        int newUnloaded = 0;
        List<Chunk> newChunks = [.. chunks];
        List<Vector2I> shouldBeLoaded = [];
        for (int i = -loadingDistance; i < loadingDistance; i++)
        {
            for(int j = -loadingDistance; j < loadingDistance; j++)
            {
                if (Math.Sqrt(i * i + j * j) < loadingDistance)
                {
                    shouldBeLoaded.Add(new Vector2I(i, j) + (Vector2I)(playerPosition / 16.0f));
                }
            }
        }
        foreach (Chunk chunk in chunks)
        {
            bool shouldStayLoaded = false;
            foreach (Vector2I position in shouldBeLoaded)
            {
                if (chunk.Position == position)
                {
                    shouldStayLoaded = true;
                    break;
                }
            }
            if (!shouldStayLoaded)
            {
                newChunks.Remove(chunk);
                newUnloaded++;
            }
        }
        for (int i = -loadingDistance; i < loadingDistance; i++)
        {
            for(int j = -loadingDistance; j < loadingDistance; j++)
            {
                if (Math.Sqrt(i * i + j * j) < loadingDistance)
                {
                    bool alreadyLoaded = false;
                    foreach (Chunk chunk in chunks)
                    {
                        if (chunk.Position == new Vector2I(i, j) + (Vector2I)(playerPosition / 16.0f))
                        {
                            alreadyLoaded = true;
                            break;
                        }
                    }
                    if (!alreadyLoaded)
                    {
                        newChunks.Add(new Chunk(new Vector2I(i, j) + (Vector2I)(playerPosition / 16.0f)));
                        newLoaded++;
                    }
                }
            }
        }
        chunks = newChunks;
        Debug.Info($"Loaded: {newLoaded}, unloaded: {newUnloaded} ()");
    }
    public void Draw()
    {
        
    }
}