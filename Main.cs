using Godot;
using System;

public partial class Main : Node2D
{
    private int counter = 0;
    private ChunkManager chunkManager = new();
    private Vector2 playerPos = Vector2.Zero;
    public override void _Ready()
    {
        chunkManager.SetLoadingDistance(6);
        chunkManager.SetPlayerPosition(Vector2.Zero);
        base._Ready();
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        counter++;
        if (counter % 10 == 0)
        {
            if (Input.IsPhysicalKeyPressed(Key.D))
            {
                playerPos.X += 10;
            }
            if (Input.IsPhysicalKeyPressed(Key.A))
            {
                playerPos.X += 10;
            }
            chunkManager.SetPlayerPosition(playerPos);
        }
    }
}
