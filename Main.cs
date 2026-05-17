using Godot;
using Godot.Bridge;
using System;

public partial class Main : Node2D
{
    private Vector2 playerPosition = Vector2.Zero;
    private Vector2 lastChunkUpdate = Vector2.Zero;
    ChunkManager chunkManager = new();
    public override void _Ready()
    {
        base._Ready();
        // TileLoader.Result loaded = TileLoader.LoadImages("res://data/tiles", "res://images/tiles");
        // loaded.Merged.SavePng("res://merged2.png");
        Chunk.TileMapParent = this;
        chunkManager.SetLoadingDistance(1);
        chunkManager.SetPlayerPosition(Vector2.Zero);
        chunkManager.Draw();
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (Input.IsPhysicalKeyPressed(Key.D))
        {
            playerPosition += new Vector2((float)delta * 20, 0);
        }
        if (Input.IsPhysicalKeyPressed(Key.A))
        {
            playerPosition += new Vector2((float)delta * -20, 0);
        }
        if (Input.IsPhysicalKeyPressed(Key.W))
        {
            playerPosition += new Vector2(0, (float)delta * -20);
        }
        if (Input.IsPhysicalKeyPressed(Key.S))
        {
            playerPosition += new Vector2(0, (float)delta * 20);
        }
        if (playerPosition.DistanceTo(lastChunkUpdate) > 8)
        {
            chunkManager.SetPlayerPosition(playerPosition);
            chunkManager.Draw();
            lastChunkUpdate = playerPosition;
        }
    }
}
