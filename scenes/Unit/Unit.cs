using Godot;
using System.Threading.Tasks;  

public partial class Unit : Area2D
{
    private UnitStats _stats;

    [Export]
    public UnitStats stats
    { 
        get => _stats;
        private set => _stats = value;
    }

    public async Task UpdateStatsAsync(UnitStats value)
    {
        stats = value;
        
        if (value == null) return;
        if (!IsNodeReady()) await ToSignal(this, "ready");
        Vector2 pos = new Vector2(
            stats.skin_coordinates.X * Arena.CELL_SIZE.X,
            stats.skin_coordinates.Y * Arena.CELL_SIZE.Y
        );
        Vector2 size = new Vector2(32, 32);
        skin.SetRegionRect(new Rect2(pos, size));
    }

    [Export] public OutlineHighlighter OutlineHighlighterComponent;
    [Export] public DragAndDrop DragAndDropComponent;
    [Export] public VelocityBasedRotation VelocityBasedRotationComponent;

    Sprite2D skin;
    ProgressBar health_bar;
    ProgressBar mana_bar;

    public override async void _Ready()
    {
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;

        (DragAndDropComponent as DragAndDrop).DragStarted += OnDragStarted;
        (DragAndDropComponent as DragAndDrop).DragCancelled += OnDragCancelled;

        skin = GetNode<CanvasGroup>("Visuals").GetNode<Sprite2D>("Skin");
        health_bar = GetNode<ProgressBar>("HealthBar");
        mana_bar = GetNode<ProgressBar>("ManaBar");

        if (stats != null)
        {
            await UpdateStatsAsync(stats);
        }
    }

    private void OnDragStarted()
    {
        (VelocityBasedRotationComponent as VelocityBasedRotation).Enabled = true;
    }

    private void OnDragCancelled(Vector2 startingPosition)
    {
        ResetAfterDragging(startingPosition);
    }

    public void ResetAfterDragging(Vector2 startingPosition)
    {
        (VelocityBasedRotationComponent as VelocityBasedRotation).Enabled = false;
        GlobalPosition = startingPosition;
    }

    private void OnMouseEntered()
    {
        if (OutlineHighlighterComponent == null) GD.PushError("Unit does not have OutlineHighlighter assigned");

        if ((DragAndDropComponent as DragAndDrop)._dragging) return;

        (OutlineHighlighterComponent as OutlineHighlighter).SetHighlight();
        ZIndex = 1;
    }
    private void OnMouseExited()
    {
        if (OutlineHighlighterComponent == null) GD.PushError("Unit does not have OutlineHighlighter assigned");

        if ((DragAndDropComponent as DragAndDrop)._dragging) return;

        (OutlineHighlighterComponent as OutlineHighlighter).ClearHighlight();
        ZIndex = 0;
    }
}