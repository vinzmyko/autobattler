using Godot;

public partial class TileHighLighter : Node
{
    private bool _enabled = true;
    [Export] public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;

            if (!_enabled && AreaService != null) HighlightLayer.Clear();
        }
    }

    [Export] public PlayArea AreaService;
    [Export] public TileMapLayer HighlightLayer;
    [Export] public Vector2I Tile;

    private int SourceID;

    public override void _Ready()
    {
        // Get the first image/source in the tile map layer
        SourceID = AreaService.TileSet.GetSourceId(0);
    }

    public override void _Process(double delta)
    {
        if (!Enabled) return;

        Vector2I selectedTile = AreaService.GetHoveredTile();

        if (!AreaService.IsTileInBounds(selectedTile))
        {
            HighlightLayer.Clear();
            return;
        }

        UpdateTile(selectedTile);
    }

    private void UpdateTile(Vector2I selectedTile)
    {
        HighlightLayer.Clear();
        HighlightLayer.SetCell(selectedTile, SourceID, Tile);
    }
}
