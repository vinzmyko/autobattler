using Godot;
using System;

[GlobalClass]
public partial class UnitStats : Resource
{
    public enum Rarity
    {
        COMMON, UNCOMMON, RARE, EPIC, LEGENDARY
    }

    public static Color GetRarityColor(Rarity rarity) => rarity switch
    {
        Rarity.COMMON => new Color("#124a2e"),
        Rarity.UNCOMMON => new Color("#1c527c"),
        Rarity.RARE => new Color("#ab0974"),
        Rarity.EPIC => new Color("#6a1b9a"),
        Rarity.LEGENDARY => new Color("#ea940b"),
        _ => throw new ArgumentException("Invalid rarity")
    };

    [Export] public string Name { get; set; } = "";  
    [ExportCategory("Data")]
    [Export] public Rarity RarityType { get; set; }  
    [Export(PropertyHint.Range, "1, 5")] public int GoldCost { get; set; } 
    private int _tier = 1;
    [Export(PropertyHint.Range, "1, 3")] public int Tier
    {
        get => _tier;
        set
        {
            _tier = value;
            EmitChanged();
        }
    }
    [ExportCategory("Visuals")]
    [Export] public Vector2I SkinCoordinates { get; set; }

    public override string ToString()
    {
        return Name;
    }
}