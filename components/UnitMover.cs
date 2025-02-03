using Godot;

public partial class UnitMover : Node
{
    [Export] public PlayArea[] PlayAreas;

    public override void _Ready()
    {
        var units = GetTree().GetNodesInGroup("Units");
        foreach (Unit unit in units)
        {
            SetupUnit(unit);
            int playAreaIndex = GetPlayAreaForPosition(unit.GlobalPosition);
            var tile = PlayAreas[playAreaIndex].GetTileFromGlobal(unit.GlobalPosition);
            PlayAreas[playAreaIndex].GridService.AddUnit(tile, unit);
        }
    }

    public void SetupUnit(Unit unit)
    {
        unit.DragAndDropComponent.DragStarted += () => OnUnitDragStarted(unit);
        unit.DragAndDropComponent.DragCancelled += (startingPosition) => OnUnitDragCancelled
        (
            unit, startingPosition
        );
        unit.DragAndDropComponent.Dropped += (startingPosition) => OnUnitDropped(unit, startingPosition);
    }

    private void OnUnitDragStarted(Unit unit)
    {
        SetHighlighters(true);

        int i = GetPlayAreaForPosition(unit.GlobalPosition);

        if (i > -1)
        {
            var tile = PlayAreas[i].GetTileFromGlobal(unit.GlobalPosition);
            PlayAreas[i].GridService.RemoveUnit(tile);
        }
    }

    private void OnUnitDragCancelled(Unit unit, Vector2 startingPosition)
    {
        SetHighlighters(false);
        ResetUnitToStartingPosition(startingPosition, unit);
    }

    private void OnUnitDropped(Unit unit, Vector2 startingPosition)
    {
        SetHighlighters(false);

        int oldAreaIndex = GetPlayAreaForPosition(startingPosition);
        int dropAreaIndex = GetPlayAreaForPosition(unit.GetGlobalMousePosition());

        if (dropAreaIndex == -1)
        {
            ResetUnitToStartingPosition(startingPosition, unit);
            return;
        }

        PlayArea oldArea = PlayAreas[oldAreaIndex];
        Vector2I oldTile = oldArea.GetTileFromGlobal(startingPosition);
        PlayArea newArea = PlayAreas[dropAreaIndex];
        Vector2I newTile = newArea.GetHoveredTile();

        // If unit is there, move it away (previous unit's position)
        if (newArea.GridService.IsTileOccupied(newTile))
        {
            var oldUnit = newArea.GridService.Units[newTile] as Unit;
            newArea.GridService.RemoveUnit(newTile);
            MoveUnit(oldUnit, oldArea, oldTile);
        }

        // Move unit to a new area at a given coordinate
        MoveUnit(unit, newArea, newTile);
    }


    private void SetHighlighters(bool enabled)
    {
        foreach (PlayArea playArea in PlayAreas)
        {
            playArea.TileHighlightService.Enabled = enabled;
        }
    }

    // Which play area is the mouse hitting, bench or game area
    private int GetPlayAreaForPosition(Vector2 global)
    {
        int droppedAreaIndex = -1;

        for (int i = 0; i < PlayAreas.Length; i++)
        {
            Vector2I tile = PlayAreas[i].GetTileFromGlobal(global);

            if (PlayAreas[i].IsTileInBounds(tile))
            {
                droppedAreaIndex = i;
            }
        }

        return droppedAreaIndex;
    }

    private void ResetUnitToStartingPosition(Vector2 startingPosition, Unit unit)
    {
        int i = GetPlayAreaForPosition(startingPosition);
        Vector2I tile = PlayAreas[i].GetTileFromGlobal(startingPosition);

        unit.ResetAfterDragging(startingPosition);
        PlayAreas[i].GridService.AddUnit(tile, unit);
    }

    private void MoveUnit(Unit unit, PlayArea playArea, Vector2I tile)
    {
        playArea.GridService.AddUnit(tile, unit);
        unit.GlobalPosition = playArea.GetGlobalFromTile(tile) - Arena.HALF_CELL_SIZE;
        unit.Reparent(playArea.GridService);
    }
}
