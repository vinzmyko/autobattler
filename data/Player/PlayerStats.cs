using Godot;

[GlobalClass]
public partial class PlayerStats : Resource
{
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
}
