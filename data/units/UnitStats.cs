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
        Rarity.COMMON => new Color("#6B7280"),      
        Rarity.UNCOMMON => new Color("#3B82F6"),    
        Rarity.RARE => new Color("#10B981"),        
        Rarity.EPIC => new Color("#A855F7"),        
        Rarity.LEGENDARY => new Color("#F59E0B"),   
        _ => throw new ArgumentException("Invalid rarity")
    };

    [Export] public string Name { get; set; } = "";  
    [ExportCategory("Data")]
    [Export] public Rarity RarityType { get; set; }  
    [Export(PropertyHint.Range, "1, 5")] public int GoldCost { get; set; } = 1;
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
    [Export] public int PoolCount = 5;
    [ExportCategory("Visuals")]
    [Export] public Vector2I SkinCoordinates { get; set; }

    public int GetCombinedUnitCount()
    {
        return (int)Math.Pow(3, _tier - 1);
    }

    public int GetGoldValue()
    {
        if (GetCombinedUnitCount() == 1)
            return GoldCost;
        
        return (GoldCost * GetCombinedUnitCount()) - 1;
    }

    public override string ToString()
    {
        return Name;
    }
}