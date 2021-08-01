using Godot;
using System;

public class Acid : Area2D
{
    public float SpeedOfRaise = 1f;
    public float MaxHeight = 1f;
    public float DistToPlayer_Offset = 500f;

    private float _distToPlayer = 1f;
    private bool _bRaise = false;
    private Level _level;
    private Player _player;

    public bool IsRaising()
    {
        return _bRaise;
    }

    public void StartRaise()
    {
        _bRaise = true;
    }

    public void StopRaise()
    {
        _bRaise = false;
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
        if (_bRaise)
        {
            //calculating player distance form acid
            _distToPlayer = _player.Position.y + DistToPlayer_Offset - (Position.y - Scale.y);
            _distToPlayer = Math.Min(_distToPlayer, 0f);
            _distToPlayer = Math.Abs(_distToPlayer);

            AcidRaise(delta, 20f);
        }
    }

    private void AcidRaise(float delta, float pixPerSec)
    {
        if (Scale.y < MaxHeight)
        {
            Vector2 higherScale = new Vector2(Scale.x, Scale.y + (pixPerSec + _level.TimeSpeed * _level.Difficulty + _distToPlayer) * delta);
            Scale = higherScale;
        }
    }
}
