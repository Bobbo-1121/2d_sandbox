using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class ChunkManager
{
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
    public void Update()
    {
        int newLoaded = 0;
        int newUnloaded = 0;
        List<Vector2> loadedPositions = [];
        List<Chunk> newChunks = [.. chunks];
        int maxDistance = 0;
        Vector2I firstChunkPos = Vector2I.Zero;
        if (chunks.Count() > 0)
        {
            firstChunkPos = chunks.ElementAt(0).Position;
        }
        foreach (Chunk chunk in chunks){
            Vector2I position = chunk.Position;
            int distance = (int)Math.Max((position + new Vector2(0.5f, 0.5f) - playerPosition / 16.0f).X, (position + new Vector2(0.5f, 0.5f) - playerPosition / 16.0f).Y);
            maxDistance = maxDistance < distance ? distance : maxDistance;
            if (distance > loadingDistance)
            {
                newChunks.Remove(chunk);
                newUnloaded++;
                continue;
            }
            loadedPositions.Add(position);
        }
        for(int i = -loadingDistance; i < loadingDistance; i++)
        {
            for(int j = -loadingDistance; j < loadingDistance; j++)
            {
                if (!loadedPositions.Contains(new Vector2I(i, j)))
                {
                    newChunks.Add(new Chunk(new Vector2I(i, j)));
                    newLoaded++;
                }
            }
        }
        chunks = newChunks;
        Debug.Info($"Max chunk distance: {maxDistance}\nFirst chunk at: {firstChunkPos}");
        if(newLoaded > 0 && newUnloaded > 0)
        {
            Debug.Info($"Loaded {newLoaded} new chunks, unloaded {newUnloaded} chunks");
        }
        else if (newLoaded > 0)
        {
            Debug.Info($"Loaded {newLoaded} new chunks");
        }
        else if (newUnloaded > 0)
        {
            Debug.Info($"Unloaded {newUnloaded} chunks");
        }
    }
}