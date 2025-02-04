using System;
using Godot;

public partial class SellPortal : Area2D
{
    [ExportCategory("Exports")]
    [Export] public PlayerStats PlayerStats;
    public OutlineHighlighter OutlineHighlighter;
    
    public HBoxContainer Gold;
    public Label GoldLabel;

    private Unit currentUnit;

    public override void _Ready()
    {
        Gold = GetNode<HBoxContainer>("%Gold");
        GoldLabel = GetNode<Label>("%GoldLabel");
        OutlineHighlighter = GetNode<OutlineHighlighter>("OutlineHighlighter");

        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;

        var units = GetTree().GetNodesInGroup("Units");
        foreach (Unit unit in units)
        {
            SetupUnit(unit);
        }
    }

    public void SetupUnit(Unit unit)
    {
        unit.DragAndDropComponent.Dropped += (startingPosition) => OnUnitDropped(unit, startingPosition);
        unit.QuickSellPressed += () => SellUnit(unit);
    }

    private void SellUnit(Unit unit)
    {
        PlayerStats.Gold += unit.stats.GetGoldValue();
        // TODO: Give items back to item pool
        // TODO: Put units back into pool
        GD.Print(PlayerStats.Gold);

        unit.QueueFree();
    }

    private void OnUnitDropped(Unit unit, Vector2 startingPosition)
    {
        if (unit != null && unit == currentUnit)
        {
            SellUnit(unit);
        }
    }

    private void OnAreaExited(Area2D area)
    {
        if (area is not Unit unit) return;

        if (unit != null && unit == currentUnit)
            currentUnit = null;

        OutlineHighlighter.ClearHighlight();
        Gold.Hide();
    }

    private void OnAreaEntered(Area2D area)
    {
        if (!(area is Unit unit)) return;

        currentUnit = unit;
        OutlineHighlighter.SetHighlight();
        GoldLabel.Text = $"{unit.stats.GetGoldValue()}";
        Gold.Show();
    }
}
