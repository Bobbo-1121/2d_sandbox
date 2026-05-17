using Godot;
using System;

public struct TileCell
{
    public Tile MainTile;
    public Tile BackgroundTile;
    public TileCell(Tile main, Tile background)
    {
        MainTile = main;
        BackgroundTile = background;
    }
    public TileCell(string mainId, string backgroundId)
    {
        MainTile = new Tile(mainId);
        BackgroundTile = new Tile(backgroundId);
    }
    public TileCell()
    {
        MainTile = new Tile();
        BackgroundTile = new Tile();
    }
    public bool MainEmpty()
    {
        return MainTile.Id == null;
    }
    public bool BackgroundEmpty()
    {
        return BackgroundTile.Id == null;
    }
}