using Godot;
using System;

public class Acid : Area2D
{
    public float SpeedOfRaise = 1f;

    private float _aToP_Distance = 1f;
    private float _aToP_Offset = 500f;
    private Level _level;
    private Player _player;


    private void AcidRaise(float delta, float pixPerSec)
    {
        Vector2 higherScale = new Vector2(Scale.x, Scale.y + (pixPerSec + _level.TimeSpeed * _level.Difficulty + _aToP_Distance) * delta);
        Scale = higherScale;
        //_aToP_Distance
    }

    public void ResetAcid()
    {
        Vector2 resetScale = new Vector2(Scale.x, 1.0f);
        Scale = resetScale;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _player = GetTree().Root.GetNode<Node2D>("Main").GetNode<Player>("Player");
        _level = (Level)GetParent();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        //calculating player distance form acid
        _aToP_Distance = _player.Position.y + _aToP_Offset - (Position.y - Scale.y);
        _aToP_Distance = Math.Min(_aToP_Distance, 0f);
        _aToP_Distance = Math.Abs(_aToP_Distance);

        AcidRaise(delta, 20f);
    }
}
