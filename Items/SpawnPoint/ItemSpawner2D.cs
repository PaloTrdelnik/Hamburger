using Godot;
using System;

public class ItemSpawner2D : Node2D
{
    [Export]
    public int SpawnLevel = 0;

    [Export]
    public PackedScene ItemScene;

    public void SpawnItemByLevel(int spawnLevel, Node spawnUnderParent)
    {
        foreach (SpawnPoint2D sPos in GetChildren())
        {
            if (spawnLevel + sPos.SpawnLevel == SpawnLevel)
            {
                //prepare scene
                var itemToSpawn = ItemScene;
                var itemNode = itemToSpawn.Instance();
                Node2D it = (Node2D)itemNode;
                it.Position = sPos.Position;

                //spawn item scene
                spawnUnderParent.CallDeferred("add_child", it);
            }
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Node testParent = (Node)GetParent();
        SpawnItemByLevel(0, testParent);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
