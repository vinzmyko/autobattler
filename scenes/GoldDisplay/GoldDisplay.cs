using System;
using Godot;

public partial class GoldDisplay : HBoxContainer
{
    [Export] public PlayerStats PlayerStats { get; private set; }

    public Label Gold => GetNode<Label>("Gold");

    public override void _Ready()
    {
        PlayerStats.Changed += OnPlayerStatsChanged;
        OnPlayerStatsChanged();
    }

    private void OnPlayerStatsChanged()
    {
        Gold.Text = $"{PlayerStats.Gold}";
    }
}
