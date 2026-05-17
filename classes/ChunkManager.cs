using Godot;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;

public class ChunkManager
{
    private Vector2 playerPosition;
    public void SetPlayerPosition(Vector2 position)
    {
        bool update = playerPosition != position;
        playerPosition = position - new Vector2I(8, 8);
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
    public Chunk ChunkAt(Vector2I position)
    {
        foreach (Chunk chunk in chunks)
        {
            if (chunk.Position == position)
            {
                return chunk;
            }
        }
        return null;
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
        List<Chunk> justAdded = [];
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
                chunk.Delete();
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
                        Chunk chunk = Chunk.MakeTestChunk(new Vector2I(i, j) + (Vector2I)(playerPosition / 16.0f));
                        newChunks.Add(chunk);
                        justAdded.Add(chunk);
                        newLoaded++;
                    }
                }
            }
        }
        chunks = newChunks;
        foreach (Chunk chunk in justAdded)
        {
            chunk.AdjacentChunks[0] = ChunkAt(chunk.Position + Vector2I.Up);
            chunk.AdjacentChunks[1] = ChunkAt(chunk.Position + Vector2I.Right);
            chunk.AdjacentChunks[2] = ChunkAt(chunk.Position + Vector2I.Down);
            chunk.AdjacentChunks[3] = ChunkAt(chunk.Position + Vector2I.Left);
        }
        Debug.Info($"Loaded: {newLoaded}, unloaded: {newUnloaded} ()");
    }
    public void Draw()
    {
        foreach (Chunk chunk in chunks)
        {
            chunk.Draw();
        }
    }
}