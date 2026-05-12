using Godot;
using System;

public partial class Main : Node2D
{
    [Export]
    Image[] Images;
    [Export]
    Sprite2D OutputSprite;
    public override void _Ready()
    {
        base._Ready();
        Image[] Formatted = new Image[Images.Length];
        for(int i = 0; i < Images.Length; i++)
        {
            Formatted[i] = TileRes.FormatTileImage(Images[i]);
        }
        Image merged = TileRes.MergeTileImages(Formatted);
        OutputSprite.Texture = ImageTexture.CreateFromImage(merged);
    }
}
