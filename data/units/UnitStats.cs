using Godot;
using System;

[GlobalClass]
public partial class UnitStats : Resource
{
    public enum Rarity
    {
        COMMON, UNCOMMON, RARE, LEGENDARY
    }

    public static Color GetRarityColor(Rarity rarity) => rarity switch
    {
        Rarity.COMMON => new Color("#124a2e"),
        Rarity.UNCOMMON => new Color("#1c527c"),
        Rarity.RARE => new Color("#ab0974"),
        Rarity.LEGENDARY => new Color("#ea940b"),
        _ => throw new ArgumentException("Invalid rarity")
    };

    [Export] public string name { get; set; } = "";  
    [ExportCategory("Data")]
    [Export] public Rarity rarity { get; set; }  
    [Export] public int gold_cost { get; set; } 
    [ExportCategory("Visuals")]
    [Export] public Vector2I skin_coordinates { get; set; }

    public override string ToString()
    {
        return name;
    }
}