using Godot;
using System;

public class GameplayLock_BiggerInventory : GameplayLock
{
    public int ReachedScore;
    public Player LockedPlayer;
    public int PlacesAdded = 5;
    public bool bIsLocked = true;

    public override void Unlock()
    {
        if (IsAvailable())
        {
            LockedPlayer.ResizeInvertory(LockedPlayer.Inv.UsageProp.GetMaximum() + PlacesAdded);

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
