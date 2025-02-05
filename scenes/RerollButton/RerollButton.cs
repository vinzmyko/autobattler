using System;
using Godot;

public partial class RerollButton : Button
{
    [Export] public PlayerStats PlayerStats;

    public HBoxContainer HBoxContainer => GetNode<HBoxContainer>("HBoxContainer");
    public override void _Ready()
    {
        Pressed += OnRerollButtonPressed;

        PlayerStats.Changed += OnPlayerStatsChanged;
        OnPlayerStatsChanged();
    }

    private void OnPlayerStatsChanged()
    {
        bool _hasEnoughGold = PlayerStats.Gold >= 2;
        Disabled = !_hasEnoughGold;

        if (_hasEnoughGold)
        {
            HBoxContainer.Modulate = new Color(Color.FromHtml("#ffffff"), 1.0f);
        }
        else
        {
            HBoxContainer.Modulate = new Color(Color.FromHtml("#ffffff"), 0.5f);
        }
    }

    private void OnRerollButtonPressed()
    {
        PlayerStats.Gold -= 2;
    }
}
