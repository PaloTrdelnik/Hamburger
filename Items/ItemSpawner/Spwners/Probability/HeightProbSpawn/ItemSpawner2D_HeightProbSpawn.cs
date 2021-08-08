using Godot;
using System;

public class ItemSpawner2D_HeightProbSpawn : ItemSpawner2D
{
    //NOTE: To Use this spawner properly, level sould be placed at 0.0 coordinates of y axis(vertical)

    [Export]
    public float HeightOffset = 0.0f;
    [Export]
    public float LevelHeight = 10f * 23360.0f;
    [Export]
    public float SpawnProbability = 0.001f;
    // SpawnProbability is stored in float but it is not as precise (0.6 works same as 0.5)
    // This is because items are spawned by levels, so float SpawnProbability have to be converted to int (number of levels)

    public override void RespawnItems()
    {
        //GD.Print("Respawning");
        RemoveItems();
        RandomizeSpawnPointLevels(0, (int)(0.0f + 1.0f / SpawnProbability));
        SpawnItemByLevel((int)(Math.Abs(GlobalPosition.y - HeightOffset) / (LevelHeight * SpawnProbability)));
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
