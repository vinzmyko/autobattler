using Godot;
using System.Threading.Tasks;  

public partial class Unit : Area2D
{
    private UnitStats _stats;

    [Export]
    public UnitStats stats
    { 
        get => _stats;
        private set => _stats = value;
    }

    public async Task UpdateStatsAsync(UnitStats value)
    {
        stats = value;
        
        if (value == null) return;
        if (!IsNodeReady()) await ToSignal(this, "ready");
        Vector2 pos = new Vector2(
            stats.skin_coordinates.X * Arena.CELL_SIZE.X,
            stats.skin_coordinates.Y * Arena.CELL_SIZE.Y
        );
        Vector2 size = new Vector2(32, 32);
        skin.SetRegionRect(new Rect2(pos, size));
    }

    Sprite2D skin;
    ProgressBar health_bar;
    ProgressBar mana_bar;

    public override async void _Ready()
    {
        skin = GetNode<Sprite2D>("Skin");
        health_bar = GetNode<ProgressBar>("HealthBar");
        mana_bar = GetNode<ProgressBar>("ManaBar");

        if (stats != null)
        {
            await UpdateStatsAsync(stats);
        }
    }
}
