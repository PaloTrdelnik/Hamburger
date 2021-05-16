using Godot;
using System;

public class MoveIntervalChecker : Node2D
{
    [Signal]
    public delegate void SIntervalReached();

    public Node2D InstanceToCheck;

    private Interval _actualInterval;
    //private Vector2 _actualPos;
    private Vector2 _previousInterEndPos;
    private int _actualIntervalNumber = 1;
    private int _intervalChildIndex = 0;

    public void ResetIntervals()
    {
        _actualIntervalNumber = 1;
        _intervalChildIndex = 0;

        _actualInterval = GetChild<Interval>(_intervalChildIndex);
        _previousInterEndPos = Position;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _actualInterval = GetChild<Interval>(_intervalChildIndex);
        _previousInterEndPos = Position;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (_actualInterval.NumberOfIntervals == 0)
        {
            if (IsIntervalReached(InstanceToCheck.Position.y, _previousInterEndPos.y))
            {
                _actualIntervalNumber++;

                EmitSignal(nameof(SIntervalReached));
            }
        }
        else
        {
            if (IsIntervalReached(InstanceToCheck.Position.y, _previousInterEndPos.y))
            {
                if (_actualInterval.NumberOfIntervals == _actualIntervalNumber)
                {
                    _actualIntervalNumber = 0;
                    _intervalChildIndex++;
                    _actualInterval = GetChild<Interval>(_intervalChildIndex);
                }
                else
                {
                    _actualIntervalNumber++;

                    EmitSignal(nameof(SIntervalReached));
                }
            }
        }
    }

    private bool IsIntervalReached(float instPos, float interPos)
    {
        return instPos < interPos - _actualInterval.IntervalLength * _actualIntervalNumber;
    }
}
