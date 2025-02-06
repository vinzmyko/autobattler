using System.Threading.Tasks;
using Godot;

public partial class UnitCard : Button
{
    [Signal] public delegate void UnitBoughtEventHandler(UnitStats unit);

    private static readonly Color HOVER_BORDER_COLOUR = Color.FromHtml("#fafa82");

    [Export] public PlayerStats PlayerStats { get; private set; }
    private UnitStats _unitStats;  
    [Export] 
    public UnitStats UnitStats
    {
        get => _unitStats;
        set
        {
            _unitStats = value;
            if (!IsNodeReady())
            {
                _ = SetUnitStatsAsync(value);
            }
            else
            {
                InitializeUnitStats(value);
            }
        }
    }


    public Label Traits => GetNode<Label>("%Traits");
    public Panel Bottom => GetNode<Panel>("%Bottom");
    public Label UnitName => GetNode<Label>("%UnitName");
    public Label GoldCost => GetNode<Label>("%GoldCost");
    public Panel Border => GetNode<Panel>("%Border");
    public Panel EmptyPlaceHolder => GetNode<Panel>("%EmptyPlaceHolder");
    public TextureRect UnitIcon => GetNode<TextureRect>("%UnitIcon");
    public StyleBoxFlat BorderStyleBox => Border.GetThemeStylebox("panel") as StyleBoxFlat;
    public StyleBoxFlat BottomStyleBox => Bottom.GetThemeStylebox("panel") as StyleBoxFlat;

    public bool Bought = false;
    private Color _borderColour;

        private async Task SetUnitStatsAsync(UnitStats value)
    {
        await ToSignal(this, "ready");
        InitializeUnitStats(value);
    }

    private void InitializeUnitStats(UnitStats value)
    {
        if (value == null)
        {
            EmptyPlaceHolder.Show();
            Disabled = true;
            Bought = true;
            return;
        }

        _borderColour = UnitStats.GetRarityColor(value.RarityType);
        BorderStyleBox.BorderColor = _borderColour;
        BottomStyleBox.BgColor = _borderColour;
        UnitName.Text = value.Name;
        GoldCost.Text = value.GoldCost.ToString();
        var atlasTexture = UnitIcon.Texture as AtlasTexture;
        if (atlasTexture != null)
        {
            var newRegion = atlasTexture.Region;
            newRegion.Position = new Vector2(value.SkinCoordinates.X, value.SkinCoordinates.Y) * Arena.CELL_SIZE;
            
            var newAtlasTexture = new AtlasTexture
            {
                Atlas = atlasTexture.Atlas,
                Region = newRegion
            };
            
            UnitIcon.Texture = newAtlasTexture;
        }
    }

    public override void _Ready()
    {
        Pressed += OnUnitCardPressed;
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;

        PlayerStats.Changed += OnPlayerStatsChanged;
        OnPlayerStatsChanged();
    }

    private void OnPlayerStatsChanged()
    {
        if (UnitStats == null) return;

        bool _hasEnoughGold = PlayerStats.Gold >= UnitStats.GoldCost;
        Disabled = !_hasEnoughGold;

        if (_hasEnoughGold || Bought)
        {
            Modulate = new Color(Color.FromHtml("#ffffff"), 1.0f);
        }
        else
        {
            Modulate = new Color(Color.FromHtml("#ffffff"), 0.5f);
        }
    }

    private void OnMouseExited()
    {
        BorderStyleBox.BorderColor = _borderColour;
    }

    private void OnMouseEntered()
    {
        if (!Disabled)
        {
            BorderStyleBox.BorderColor = HOVER_BORDER_COLOUR;
        }
    }

    private void OnUnitCardPressed()
    {
        if (Bought) return;

        Bought = true;
        EmptyPlaceHolder.Show();
        PlayerStats.Gold -= UnitStats.GoldCost;
        EmitSignal(SignalName.UnitBought, UnitStats);
    }
}
