using Godot;
using System;

public class ItemSpawner2D_Constaint : ItemSpawner2D
{
    public override void RespawnItems()
    {
        RemoveItems();
        SpawnItemByLevel(50);
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
