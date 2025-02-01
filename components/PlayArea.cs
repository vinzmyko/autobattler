using Godot;

public partial class PlayArea : TileMapLayer
{
    [Export] public UnitGrid GridService;

    private Rect2I _bounds;

    public override void _Ready()
    {
        _bounds = new Rect2I(Vector2I.Zero, GridService.Size);
    }

    public Vector2I GetTileFromGlobal(Vector2 global)
    {
        return LocalToMap(ToLocal(global));
    }

    public Vector2 GetGlobalFromTile(Vector2I tile)
    {
        return ToGlobal(MapToLocal(tile));
    }

    public Vector2I GetHoveredTile()
    {
        return LocalToMap(GetLocalMousePosition());
    }

    public bool IsTileInBounds(Vector2I tile)
    {
        return _bounds.HasPoint(tile);
    }
}
