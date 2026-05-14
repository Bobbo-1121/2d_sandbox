using Godot;
using System;

public partial class AssetLoader : Node
{
    [Export]
    public string TileDataDirectoryPath = "res://data/tiles";
    [Export]
    public string TileImageDirectoryPath = "res://images/tiles";
    private void LoadTiles()
    {
        TileLoader.Result result = TileLoader.LoadImages(TileDataDirectoryPath, TileImageDirectoryPath);
        Tile.Init(result.Ids, result.VariantCountById, result.Merged, TileDataDirectoryPath);
    }
    public override void _Ready()
    {
        base._Ready();
        LoadTiles();
        Debug.Info(Tile.GetTileIds());
    }
}
