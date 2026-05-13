using Godot;
using System;

public partial class Main : Node2D
{
    public override void _Ready()
    {
        base._Ready();
        string[] verified = TileLoader.GetVerifiedTiles("res://data/tiles", "res://images/tiles");
        Debug.Info(verified);
    }
}
