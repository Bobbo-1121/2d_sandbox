using Godot;
using System;

public partial class Main : Node2D
{
    public override void _Ready()
    {
        base._Ready();
        TileLoader.Result loaded = TileLoader.LoadImages("res://data/tiles", "res://images/tiles");
        loaded.Merged.SavePng("res://merged2.png");
    }
}
