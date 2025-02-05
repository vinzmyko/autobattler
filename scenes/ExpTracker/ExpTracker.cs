using Godot;

public partial class ExpTracker : VBoxContainer
{
    [Export] public PlayerStats PlayerStats { get; private set; }

    private ProgressBar progressBar => GetNode<ProgressBar>("%ProgressBar");
    private Label expLabel => GetNode<Label>("%ExpLabel");
    private Label levelLabel => GetNode<Label>("%LevelLabel");

    public override void _Ready()
    {
        PlayerStats.Changed += OnPlayerStatsChanged;
        OnPlayerStatsChanged();
    }

    // TEST CODE
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept"))
        {
            PlayerStats.Exp += 4;
        }
    }

    private void OnPlayerStatsChanged()
    {
        if (PlayerStats.Level < 10)
        {
            SetExpBarValues();
        }
        else
        {
            SetMaxLevelValues();
        }

        levelLabel.Text = $"lvl: {PlayerStats.Level}";
    }

    private void SetExpBarValues()
    {
        float expRequirement = PlayerStats.GetCurrentExpRequirement();
        expLabel.Text = $"{PlayerStats.Exp}/{expRequirement}";
        progressBar.Value = PlayerStats.Exp/expRequirement * 100;
    }

    private void SetMaxLevelValues()
    {
        expLabel.Text = "MAX";
        progressBar.Value = 100;
    }
}
