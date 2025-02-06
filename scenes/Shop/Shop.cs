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

        RollUnits();
    }

    private void RollUnits()
    {
        for (int i = 0; i < 5; i++)
        {
            UnitStats.Rarity rarity = PlayerStats.GetRandomRarityForLevel();
            UnitCard newCard = UNIT_CARD.Instantiate<UnitCard>();
            newCard.UnitStats = UnitPool.GetRandomUnitByRarity(rarity);
            newCard.UnitBought += OnUnitBought;
            ShopCards.AddChild(newCard);
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
        PutBackRemainingToPool();
        RollUnits();
    }
}
