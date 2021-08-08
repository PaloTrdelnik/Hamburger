using Godot;
using System;

public class GameplayLock_TimeDilationLonger : GameplayLock
{
    public int ReachedScore;
    public Player LockedPlayer;
    public float TimeAdded = 1f;
    public bool bIsLocked = true;

    public override void Unlock()
    {
        if (IsAvailable())
        {
            LockedPlayer.ItemProp.TimeDilation_UseDurabilityTime += TimeAdded;

            bIsLocked = false;
        }
    }

    public override bool IsAvailable()
    {
        if (LockedPlayer.ScoreProp.GetRecord() < ReachedScore || !bIsLocked)
            return false;
        else
            return true;
    }

    //// Called when the node enters the scene tree for the first time.
    //public override void _Ready()
    //{

    //}

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
