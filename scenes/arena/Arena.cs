using Godot;

public partial class Arena : Node2D
{
    public static readonly Vector2 CELL_SIZE = new Vector2(32, 32);
    public static readonly Vector2 HALF_CELL_SIZE = new Vector2(16, 16);
    public static readonly Vector2 QUARTER_CELL_SIZE = new Vector2(8, 8);
}
