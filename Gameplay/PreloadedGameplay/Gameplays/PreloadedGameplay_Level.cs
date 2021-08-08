using Godot;
using System;

public class PreloadedGameplay_Level : PreloadedGameplay
{
    public float DifficultyStep = 5f;
    public bool bFirstPlay = true;

    public void OnMainSLevelLoaded(Node level)
    {
        if (level is Level)
        {
            DataChild = (Level)level;
        }

        MoveDataToDataChild();
        DataChildLoaded = true;
    }

    public void OnMainSLevelUnloaded()
    {
        CopyDataFromDataChild();
        DataChild = null;
        DataChildLoaded = false;
    }

    public override void MoveDataToDataChild()
    {
        if (DataChild is Level)
        {
            Level level = (Level)DataChild;

            level.DifficultyStep = DifficultyStep;
            level.bFirstPlay = bFirstPlay;
        }
    }

    public override void CopyDataFromDataChild()
    {
        if (DataChild is Level)
        {
            Level level = (Level)DataChild;

            DifficultyStep = level.DifficultyStep;
            bFirstPlay = level.bFirstPlay;
        }
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
