using System;
using System.Linq;
using Godot;

public partial class Shop : VBoxContainer
{
    [Signal] public delegate void UnitBoughtEventHandler(UnitStats unit);

    public static readonly PackedScene UNIT_CARD = GD.Load<PackedScene>("res://scenes/UnitCard/UnitCard.tscn");

    [Export] public PlayerStats PlayerStats { get; private set; }
    [Export] public UnitPool UnitPool { get; private set; }

    public VBoxContainer ShopCards => GetNode<VBoxContainer>("%ShopCards");
    public Button RerollButton => GetNode<Button>("%RerollButton");

    public override void _Ready()
    {
        RerollButton.Pressed += OnRerollButtonPressed;

        UnitPool.GenerateUnitPool();

        foreach (Node child in ShopCards.GetChildren())
        {
            child.QueueFree();
        }

    }


    private void PutBackRemainingToPool()
    {
        foreach (UnitCard unitCard in ShopCards.GetChildren().Cast<UnitCard>())
        {
            if (!unitCard.Bought)
                UnitPool.AddUnit(unitCard.UnitStats);
            
            unitCard.QueueFree();
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
