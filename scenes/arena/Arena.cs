using Godot;

public partial class Arena : Node2D
{
    public static readonly Vector2 CELL_SIZE = new Vector2(32, 32);
    public static readonly Vector2 HALF_CELL_SIZE = new Vector2(16, 16);
    public static readonly Vector2 QUARTER_CELL_SIZE = new Vector2(8, 8);

    public UnitMover UnitMoverService;
    public UnitSpawner UnitSpawnerService;
    public SellPortal SellPortalService;
    public Shop ShopService => GetNode<Shop>("%Shop");

    public override void _Ready()
    {
        UnitMoverService = GetNode<UnitMover>("UnitMover");
        UnitSpawnerService = GetNode<UnitSpawner>("UnitSpawner");
        SellPortalService = GetNode<SellPortal>("SellPortal");

        UnitSpawnerService.UnitSpawned += UnitMoverService.SetupUnit;
        UnitSpawnerService.UnitSpawned += SellPortalService.SetupUnit;
        ShopService.UnitBought += UnitSpawnerService.SpawnUnit;
    }
}
