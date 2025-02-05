using Godot;

[GlobalClass]
public partial class PlayerStats : Resource
{
    public static readonly Godot.Collections.Dictionary<int, int> EXP_REQUIREMENTS = new Godot.Collections.Dictionary<int, int>
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

    public int GetCurrentExpRequirement()
    {
        return EXP_REQUIREMENTS[_level + 1];
    }
}
