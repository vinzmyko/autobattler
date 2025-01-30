using Godot;

public partial class DragAndDrop : Node
{
    [Signal] public delegate void DragCancelledEventHandler(Vector2 startingPosition);
    [Signal] public delegate void DragStartedEventHandler();
    [Signal] public delegate void DroppedEventHandler(Vector2 startPosition);

    [Export] public bool Enabled = true;
    [Export] public Area2D Target;

    private Vector2 _startingPosition;
    // So there is jump from where you clicked the unit and the GlobalPosition of the unit which is the top left
    private Vector2 _offset;
    private bool _dragging;

    public override void _Ready()
    {
        if (Target == null)
        {
            GD.PrintErr("No target set for DragAndDrop Component!");
        }

        Target.InputEvent += OnTargetInputEvent;
    }

    public override void _Process(double delta)
    {
        if (_dragging && Target != null)
        {
            Target.GlobalPosition = Target.GetGlobalMousePosition() + _offset;
        }
    }

    // The InputEvent.OnTargetInputEvent doesn't seem to work with keyboard clicks so overriding in the _Input function
    // Could interfere if you have escape key bound to pause so if you want to have escape to also cancel drag keep it in and
    // find a solution
    // public override void _Input(InputEvent @event)
    // {
    //     if (_dragging && Target != null && @event.IsActionPressed("cancel_drag"))
    //     {
    //         CancelDragging();
    //     }
    // }

    private void EndDragging()
    {
        _dragging = false;
        Target.RemoveFromGroup("Dragging");
        Target.ZIndex = 0;
    }

    private void CancelDragging()
    {
        EndDragging();
        EmitSignal(SignalName.DragCancelled, _startingPosition);
    }

    private void StartDragging()
    {
        _dragging = true;
        _startingPosition = Target.GlobalPosition;
        Target.AddToGroup("Dragging");
        // Top most layer independent of parent ie. If you are moving the unit for higher z index parent to lower one
        Target.ZIndex = 99;
        _offset = Target.GlobalPosition - Target.GetGlobalMousePosition();
        EmitSignal(SignalName.DragStarted);
    }

    private void Drop()
    {
        EndDragging();
        EmitSignal(SignalName.Dropped, _startingPosition);
    }

    private void OnTargetInputEvent(Node viewport, InputEvent @event, long _shapeIdx)
    {
        if (!Enabled) return;

        // Only be able to drag one unit at a time
        Node draggingObject = GetTree().GetFirstNodeInGroup("Dragging");

        if (!_dragging && draggingObject != null) return;

        if (_dragging && @event.IsActionPressed("cancel_drag"))
        {
            CancelDragging();
        }
        else if (!_dragging && @event.IsActionPressed("select"))
        {
            StartDragging();
        }
        else if (_dragging && @event.IsActionPressed("select"))
        {
            Drop();
        }
    }
}