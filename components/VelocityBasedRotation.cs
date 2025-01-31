using System;
using Godot;

public partial class VelocityBasedRotation : Node
{
    private bool _enabled = true;
    [Export] public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;

            if (Target != null && !_enabled) Target.Rotation = 0.0f;
        }
    } 
    [Export] public Node2D Target;
    [Export(PropertyHint.Range, "0.25, 1.25")] public float LerpSeconds = 0.4f;
    [Export] public int RotationMultiplier = 120;
    [Export] public float XVelocityThreshold = 3.0f;

    private Vector2 _lastPostion;
    private Vector2 _velocity;
    private float _angle;
    private float _progress;
    private float _timeElapsed = 0.0f;

    public override void _Process(double delta)
    {
        if (!Enabled || Target == null) return;

        _velocity = Target.GlobalPosition - _lastPostion;
        _lastPostion = Target.GlobalPosition;
        _progress = _timeElapsed / LerpSeconds;
    
        if (Math.Abs(_velocity.X) > XVelocityThreshold)
        {
            _angle = _velocity.Normalized().X * RotationMultiplier * (float)delta;
        }
        else { _angle = 0.0f; }

        Target.Rotation = Mathf.LerpAngle(Target.Rotation, _angle, _progress);
        _timeElapsed += (float)delta;

        if (_progress > 1.0f) _timeElapsed = 0.0f;
    }
}
