using Godot;
using System;
using System.Runtime.CompilerServices;

public class Chunk
{
    public static int VariantRandomizer(Vector2I position)
    {
        double nonFract = Math.Sin(position.X * 12.9898 + position.Y * 78.233) * 43758.5453;
        return (int)((nonFract - Math.Floor(nonFract)) * 1000000);
    }
    public static Node TileMapParent;
    public readonly Vector2I Position;
    public TileCell[,] Cells;
    public TileMapLayer MainTileMap;
    public TileMapLayer BackgroundTileMap;
    public Chunk[] AdjacentChunks = new Chunk[4]; //up, right, down, left
    public Chunk(Vector2I position)
    {
        Position = position;
        Cells = new TileCell[16, 16];
        MainTileMap = new(){
            Position = position * 128,
            TileSet = TileRes.GetMainTileSet()
        };
        BackgroundTileMap = new(){
            Position = position * 128,
            TileSet = TileRes.GetMainTileSet()
        };
        if (TileMapParent != null)
        {
            TileMapParent.AddChild(MainTileMap);
            TileMapParent.AddChild(BackgroundTileMap);
        }
        else
        {
            Debug.Err("Chunk TileMapLayer parent is not set, chunks cannot be rendered");
        }
    }
    ~Chunk()
    {
        if (GodotObject.IsInstanceValid(MainTileMap))
        {
            MainTileMap.QueueFree();
        }
        if (GodotObject.IsInstanceValid(BackgroundTileMap))
        {
            BackgroundTileMap.QueueFree();
        }
    }
    public void Delete()
    {
        MainTileMap.QueueFree();
        BackgroundTileMap.QueueFree();
        Cells = null;
    }
    private TileCell GetRelativeCell(Vector2I position)
    {
        Chunk targeted = this;
        while(position.X < 0)
        {
            if (targeted == null) return new TileCell();
            targeted = targeted.AdjacentChunks[3];
            position.X += 16;
        }
        while(position.X > 15)
        {
            if (targeted == null) return new TileCell();
            targeted = targeted.AdjacentChunks[1];
            position.X -= 16;
        }
        while(position.Y < 0)
        {
            if (targeted == null) return new TileCell();
            targeted = targeted.AdjacentChunks[0];
            position.Y += 16;
        }
        while(position.Y > 15)
        {
            if (targeted == null) return new TileCell();
            targeted = targeted.AdjacentChunks[2];
            position.Y -= 16;
        }
        if (targeted != null)
        {
            if(targeted.Cells != null)
            {
                return targeted.Cells[position.X, position.Y];
            }
        }
        return new TileCell();
    }
    public void Draw(Vector2I tile)
    {
        int variant = VariantRandomizer(tile + Position * 16);
        string mainTileId = Cells[tile.X, tile.Y].MainTile.Id;
        if (mainTileId != null)
        {
            Vector2I[] mainSubTileCoords = Cells[tile.X, tile.Y].MainTile.GetSubTileForms(
                GetRelativeCell(tile + new Vector2I(-1, -1)).MainTile.Id == mainTileId,
                GetRelativeCell(tile + new Vector2I(0, -1)).MainTile.Id == mainTileId,
                GetRelativeCell(tile + new Vector2I(1, -1)).MainTile.Id == mainTileId,
                GetRelativeCell(tile + new Vector2I(1, 0)).MainTile.Id == mainTileId,
                GetRelativeCell(tile + new Vector2I(1, 1)).MainTile.Id == mainTileId,
                GetRelativeCell(tile + new Vector2I(0, 1)).MainTile.Id == mainTileId,
                GetRelativeCell(tile + new Vector2I(-1, 1)).MainTile.Id == mainTileId,
                GetRelativeCell(tile + new Vector2I(-1, 0)).MainTile.Id == mainTileId,
                variant
            );
            MainTileMap.SetCell(tile * 2, 0, mainSubTileCoords[0]);
            MainTileMap.SetCell(tile * 2 + new Vector2I(1, 0), 0, mainSubTileCoords[1]);
            MainTileMap.SetCell(tile * 2 + new Vector2I(0, 1), 0, mainSubTileCoords[2]);
            MainTileMap.SetCell(tile * 2 + new Vector2I(1, 1), 0, mainSubTileCoords[3]);
        }
        else
        {
            MainTileMap.EraseCell(tile * 2);
            MainTileMap.EraseCell(tile * 2);
            MainTileMap.EraseCell(tile * 2);
            MainTileMap.EraseCell(tile * 2);
        }
        string backgroundTileId = Cells[tile.X, tile.Y].BackgroundTile.Id;
        if (backgroundTileId != null)
        {
            Vector2I[] backgroundSubTileCoords = Cells[tile.X, tile.Y].BackgroundTile.GetSubTileForms(
                GetRelativeCell(tile + new Vector2I(-1, -1)).BackgroundTile.Id == backgroundTileId,
                GetRelativeCell(tile + new Vector2I(0, -1)).BackgroundTile.Id == backgroundTileId,
                GetRelativeCell(tile + new Vector2I(1, -1)).BackgroundTile.Id == backgroundTileId,
                GetRelativeCell(tile + new Vector2I(1, 0)).BackgroundTile.Id == backgroundTileId,
                GetRelativeCell(tile + new Vector2I(1, 1)).BackgroundTile.Id == backgroundTileId,
                GetRelativeCell(tile + new Vector2I(0, 1)).BackgroundTile.Id == backgroundTileId,
                GetRelativeCell(tile + new Vector2I(-1, 1)).BackgroundTile.Id == backgroundTileId,
                GetRelativeCell(tile + new Vector2I(-1, 0)).BackgroundTile.Id == backgroundTileId,
                variant + 333
            );
            BackgroundTileMap.SetCell(tile * 2, 0, backgroundSubTileCoords[0]);
            BackgroundTileMap.SetCell(tile * 2 + new Vector2I(1, 0), 0, backgroundSubTileCoords[1]);
            BackgroundTileMap.SetCell(tile * 2 + new Vector2I(0, 1), 0, backgroundSubTileCoords[2]);
            BackgroundTileMap.SetCell(tile * 2 + new Vector2I(1, 1), 0, backgroundSubTileCoords[3]);
        }
        else
        {
            BackgroundTileMap.EraseCell(tile * 2);
            BackgroundTileMap.EraseCell(tile * 2);
            BackgroundTileMap.EraseCell(tile * 2);
            BackgroundTileMap.EraseCell(tile * 2);
        }
    }
    public void Draw()
    {
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                Draw(new Vector2I(i, j));
            }
        }
    }
    public static Chunk MakeTestChunk(Vector2I position, string id = "dirt")
    {
        Chunk chunk = new(position);
        for(int i = 0; i < 16; i++)
        {
            for(int j = 0; j < 16; j++)
            {
                chunk.Cells[i, j] = new TileCell(id, null);
            }
        }
        return chunk;
    }
}