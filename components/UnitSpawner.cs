using Godot;

public partial class UnitSpawner : Node
{
    [Signal] public delegate void UnitSpawnedEventHandler(Unit unit);

    public static readonly PackedScene UNIT = GD.Load<PackedScene>("res://scenes/Unit/Unit.tscn");

    [Export] public PlayArea GameArea;
    [Export] public PlayArea Bench;

    public override void _Ready()
    {
        UnitStats Robin = GD.Load<UnitStats>("res://data/units/Robin.tres");
        Tween tween = CreateTween();

        for(int i = 0; i < 15; i++)
        {
            tween.TweenCallback(Callable.From(() => SpawnUnit(Robin)));
            tween.TweenInterval(0.5);
        }
    }

    private PlayArea GetFirstAvailableArea()
    {
        if (!Bench.GridService.IsGridFull()) 
            return Bench;
        else if (!GameArea.GridService.IsGridFull()) 
            return GameArea;

        return null;
    }

    public void SpawnUnit(UnitStats unit)
    {
        PlayArea area = GetFirstAvailableArea();
        if (area == null)
        {
            GD.PushError("No available space to add unit to!");
            return;
        }

        var newUnit = UNIT.Instantiate<Unit>();
        Vector2I tile = area.GridService.GetFirstEmptyTile();
        newUnit.stats = unit;
        area.GridService.AddChild(newUnit);
        area.GridService.AddUnit(tile, newUnit);
        newUnit.GlobalPosition = area.GetGlobalFromTile(tile) - Arena.HALF_CELL_SIZE;
        EmitSignal(SignalName.UnitSpawned, newUnit);
    }
}
