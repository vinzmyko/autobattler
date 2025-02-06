using Godot;
using Godot.NativeInterop;

[GlobalClass]
public partial class PlayerStats : Resource
{
    public static readonly Godot.Collections.Dictionary<int, int> EXP_REQUIREMENTS = new()
    {
        { 1, 0 },
        { 2, 2 },
        { 3, 2 },
        { 4, 6 },
        { 5, 10 },
        { 6, 20 },
        { 7, 36 },
        { 8, 48 },
        { 9, 76 },
        { 10, 76 }
    };
    public static readonly Godot.Collections.Dictionary<int, Godot.Collections.Array<UnitStats.Rarity>> ROLL_RARITIES = new()
    {
        { 1, new Godot.Collections.Array<UnitStats.Rarity> { UnitStats.Rarity.COMMON } },
        { 2, new Godot.Collections.Array<UnitStats.Rarity> { UnitStats.Rarity.COMMON } },
        { 3, new Godot.Collections.Array<UnitStats.Rarity> { UnitStats.Rarity.COMMON, UnitStats.Rarity.UNCOMMON } },
        { 4, new Godot.Collections.Array<UnitStats.Rarity> { UnitStats.Rarity.COMMON, UnitStats.Rarity.UNCOMMON, UnitStats.Rarity.RARE } },
        { 5, new Godot.Collections.Array<UnitStats.Rarity> { UnitStats.Rarity.COMMON, UnitStats.Rarity.UNCOMMON, UnitStats.Rarity.RARE, UnitStats.Rarity.EPIC } },
        { 6, new Godot.Collections.Array<UnitStats.Rarity> { UnitStats.Rarity.COMMON, UnitStats.Rarity.UNCOMMON, UnitStats.Rarity.RARE, UnitStats.Rarity.EPIC } },
        { 7, new Godot.Collections.Array<UnitStats.Rarity> { UnitStats.Rarity.COMMON, UnitStats.Rarity.UNCOMMON, UnitStats.Rarity.RARE, UnitStats.Rarity.EPIC, UnitStats.Rarity.LEGENDARY } },
        { 8, new Godot.Collections.Array<UnitStats.Rarity> { UnitStats.Rarity.COMMON, UnitStats.Rarity.UNCOMMON, UnitStats.Rarity.RARE, UnitStats.Rarity.EPIC, UnitStats.Rarity.LEGENDARY } },
        { 9, new Godot.Collections.Array<UnitStats.Rarity> { UnitStats.Rarity.COMMON, UnitStats.Rarity.UNCOMMON, UnitStats.Rarity.RARE, UnitStats.Rarity.EPIC, UnitStats.Rarity.LEGENDARY } },
        { 10, new Godot.Collections.Array<UnitStats.Rarity> { UnitStats.Rarity.COMMON, UnitStats.Rarity.UNCOMMON, UnitStats.Rarity.RARE, UnitStats.Rarity.EPIC, UnitStats.Rarity.LEGENDARY } }
    };

    public static readonly Godot.Collections.Dictionary<int, float[]> ROLL_CHANCE = new()
    {
        { 1, new float[] { 1.0f } },                                   
        { 2, new float[] { 1.0f } },                                   
        { 3, new float[] { 0.75f, 0.25f } },                           
        { 4, new float[] { 0.60f, 0.30f, 0.10f } },                    
        { 5, new float[] { 0.60f, 0.30f, 0.10f } },                    
        { 6, new float[] { 0.60f, 0.30f, 0.10f } },                    
        { 7, new float[] { 0.40f, 0.35f, 0.20f, 0.05f } },             
        { 8, new float[] { 0.40f, 0.35f, 0.20f, 0.05f } },             
        { 9, new float[] { 0.40f, 0.35f, 0.20f, 0.05f } },             
        { 10, new float[] { 0.40f, 0.35f, 0.20f, 0.05f } },            
    };


    private int _gold = 0;
    [Export(PropertyHint.Range, "0, 99")] public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            EmitChanged();
        }
    }
    private int _exp = 0;
    [Export(PropertyHint.Range, "0, 99")] public int Exp
    {
        get => _exp;
        set
        {
            _exp = value;
            EmitChanged();

            if (_level == 10) return;

            int expRequirement = GetCurrentExpRequirement();

            while (_exp >= expRequirement && _level < 10)
            {
                _level += 1;
                if (_level == 10)
                {
                    _exp = EXP_REQUIREMENTS[10];
                    EmitChanged();
                    break;
                }
                _exp -= expRequirement;
                expRequirement = GetCurrentExpRequirement();
                EmitChanged();
            }
        }
    }
    private int _level = 1;
    [Export(PropertyHint.Range, "1, 10")] public int Level
    {
        get => _level;
        set
        {
            _level = value;
            EmitChanged();
        }
    }

    public UnitStats.Rarity GetRandomRarityForLevel()
    {
        var rng = new RandomNumberGenerator();
        var array = ROLL_RARITIES[_level];
        
        int selectedIndex = (int)rng.RandWeighted(ROLL_CHANCE[_level]);

        return array[selectedIndex];
    }

    public int GetCurrentExpRequirement()
    {
        return EXP_REQUIREMENTS[_level + 1];
    }
}
