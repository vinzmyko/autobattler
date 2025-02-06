using System;
using System.Linq;
using Godot;

[GlobalClass]
public partial class UnitPool : Resource
{
    [Export] public Godot.Collections.Array<UnitStats> AvailableUnits = new();

    public Godot.Collections.Array<UnitStats> UnitsInPool  = new();

    public void GenerateUnitPool()
    {
        foreach (UnitStats unit in AvailableUnits)
        {
            for (int i = 0; i < unit.PoolCount; i++)
            {
                UnitsInPool.Add(unit);
            }
        }
    }

    public UnitStats GetRandomUnitByRarity(UnitStats.Rarity rarity)
    {
        var units = UnitsInPool.Where(unit => unit.RarityType == rarity).ToArray();

        if (units.Length == 0)
            return null;
        
        var random = new Random();
        UnitStats pickedUnit = units[random.Next(units.Length)];

        UnitsInPool.Remove(pickedUnit);

        return pickedUnit;
    }

    public void AddUnit(UnitStats unit)
    {
        int combinedCount = unit.GetCombinedUnitCount();
        unit.Duplicate();
        unit.Tier = 1;

        for (int i = 0; i < combinedCount; i++)
        {
            UnitsInPool.Add(unit);
        }
    }
}
