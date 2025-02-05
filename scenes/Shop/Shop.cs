using System;
using System.Linq;
using Godot;

public partial class Shop : VBoxContainer
{
    [Signal] public delegate void UnitBoughtEventHandler(UnitStats unit);

    [Export] public PlayerStats PlayerStats { get; private set; }

    public VBoxContainer ShopCards => GetNode<VBoxContainer>("%ShopCards");
    public Button RerollButton => GetNode<Button>("%RerollButton");

    public override void _Ready()
    {
        RerollButton.Pressed += OnRerollButtonPressed;

        foreach (UnitCard unitCard in ShopCards.GetChildren().Cast<UnitCard>())
        {
            unitCard.UnitBought += OnUnitBought;
        }
    }

    private void OnUnitBought(UnitStats unit)
    {
        EmitSignal(SignalName.UnitBought, unit);
    }

    private void OnRerollButtonPressed()
    {
        GD.Print("Rerolled units to new ones");
    }
}
