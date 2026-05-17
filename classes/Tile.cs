using Godot;
using System;
using System.Net;
public struct Tile
{
    public string Id;
    public float HitPoints;
    public readonly TileRes Res; 
    public Tile(string id)
    {
        Id = id;
        if (id != null)
        {
            Res = TileRes.GetTileById(Id);
        }
    }
    public Tile()
    {
        Id = null;
    }
    public Vector2I[] GetSubTileForms(bool upLeft, bool up, bool upRight, bool right, bool downRight, bool down, bool downLeft, bool left, int variant)
    {
        int realVariant = Math.Abs(variant) % Res.VariantCount;
        Vector2I[] ret = new Vector2I[4];
        TileRes.TileForm form = up && upLeft && left ? TileRes.TileForm.Fill :
        up && left ? TileRes.TileForm.Inner :
        up && upLeft || upLeft && left ? TileRes.TileForm.Fill :
        left ? TileRes.TileForm.VericalSide :
        up ? TileRes.TileForm.HorizontalSide :
        TileRes.TileForm.Corner;
        ret[0] = Res.GetTileSetCoords(TileRes.TileQuarter.TopLeft, form, realVariant);
        form = up && upRight && right ? TileRes.TileForm.Fill :
        up && right ? TileRes.TileForm.Inner :
        up && upRight || upRight && right ? TileRes.TileForm.Fill :
        right ? TileRes.TileForm.VericalSide :
        up ? TileRes.TileForm.HorizontalSide :
        TileRes.TileForm.Corner;
        ret[1] = Res.GetTileSetCoords(TileRes.TileQuarter.TopRight, form, realVariant);
        form = down && downLeft && left ? TileRes.TileForm.Fill :
        down && left ? TileRes.TileForm.Inner :
        down && downLeft || downLeft && left ? TileRes.TileForm.Fill :
        left ? TileRes.TileForm.VericalSide :
        down ? TileRes.TileForm.HorizontalSide :
        TileRes.TileForm.Corner;
        ret[2] = Res.GetTileSetCoords(TileRes.TileQuarter.BottomLeft, form, realVariant);
        form = down && downRight && right ? TileRes.TileForm.Fill :
        down && right ? TileRes.TileForm.Inner :
        down && downRight || downRight && right ? TileRes.TileForm.Fill :
        right ? TileRes.TileForm.VericalSide :
        down ? TileRes.TileForm.HorizontalSide :
        TileRes.TileForm.Corner;
        ret[3] = Res.GetTileSetCoords(TileRes.TileQuarter.BottomRight, form, realVariant);
        return ret;
    }
}