using System;
using Godot;

public partial class ExpButton : Button
{
    [Export] public PlayerStats PlayerStats { get; private set; }

    public VBoxContainer VBoxContainer => GetNode<VBoxContainer>("VBoxContainer");

    public override void _Ready()
    {
        Pressed += OnExpButtonPressed;

        PlayerStats.Changed += OnPlayerStatsChanged;
        OnPlayerStatsChanged();
    }

    private void OnPlayerStatsChanged()
    {
        bool hasEnoughGold = PlayerStats.Gold >= 4;
        bool level10 = PlayerStats.Level == 10;
        Disabled = !hasEnoughGold || level10;

        if (hasEnoughGold && !level10)
        {
            Modulate = new Color(Color.FromHtml("ffffff"), 1.0f);
        }
        else
        {
            Modulate = new Color(Color.FromHtml("ffffff"), 0.5f);
        }
    }

    private void OnExpButtonPressed()
    {
        PlayerStats.Gold -= 4;
        PlayerStats.Exp += 4;
    }
}
