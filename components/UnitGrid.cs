using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class UnitGrid : Node
{
    [Signal] public delegate void UnitGridChangedEventHandler();

    [Export] public Vector2I Size;

    public Dictionary<Vector2I, Node> Units = new();

    public override void _Ready()
    {
        for (int i = 0; i < Size.X; i++)
        {
            for (int j = 0; j < Size.Y; j++)
            {
                Units[new Vector2I(i, j)] = null;
            }
        }
    }

    public void AddUnit(Vector2I tile, Node unit)
    {
        Units[tile] = unit;
        EmitSignal(SignalName.UnitGridChanged);
    }

    public bool IsTileOccupied(Vector2I tile)
    {
        return Units[tile] != null;
    }

    public bool IsGridFull()
    {
        // ContainsValue returns true if it finds even one null, thus invert it
        return !Units.ContainsValue(null);
    }

    public Vector2I GetFirstEmptyTile()
    {
        foreach (KeyValuePair<Vector2I, Node> pair in Units)
        {
            if (!IsTileOccupied(pair.Key))
            {
                return pair.Key;
            }
        }

        throw new InvalidOperationException("No empty tiles found");
    }

    public List<Node> GetAllUnits()
    {
        List<Node> numberOfUnits = new List<Node>();

        foreach (KeyValuePair<Vector2I, Node> pair in Units)
        {
            if (pair.Value != null) { numberOfUnits.Append(pair.Value); }
        }

        return numberOfUnits;
    }
}
