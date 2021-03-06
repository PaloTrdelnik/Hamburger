using Godot;
using System;

public class ItemSpawner2D : Node2D
{
    [Export]
    public int SpawnLevel = 0;

    [Export]
    public PackedScene ItemScene;

    private Random _randGen = new Random();

    public void ONLevelSRespawnItems()
    {
        RespawnItems();
    }

    //spawn items under Spawner
    public void SpawnItemByLevel()
    {
        foreach(var child in GetChildren())
        {
            if (child is SpawnPoint2D)
            {
                SpawnPoint2D sPos = (SpawnPoint2D)child;

                if (sPos.SpawnLevel <= SpawnLevel)
                {
                    //prepare scene
                    var itemToSpawn = ItemScene;
                    var itemNode = itemToSpawn.Instance();
                    Node2D it = (Node2D)itemNode;
                    it.Position = sPos.Position;

                    //Reverse item fix
                    //this fix requires scaled parent to be direct parent of the child 
                    Node2D parent = (Node2D)GetParent();
                    if (parent.Scale.x == -1)
                    {
                        it.Scale *= new Vector2(-1, 1);
                    }

                    //spawn item scene
                    CallDeferred("add_child", it);
                }
            }
        }
    }

    public void SpawnItemByLevel(int spawnLevel)
    {
        foreach (var child in GetChildren())
        {
            if (child is SpawnPoint2D)
            {
                SpawnPoint2D sPos = (SpawnPoint2D)child;

                if (sPos.SpawnLevel <= spawnLevel)
                {
                    //prepare scene
                    var itemToSpawn = ItemScene;
                    var itemNode = itemToSpawn.Instance();
                    Node2D it = (Node2D)itemNode;
                    it.Position = sPos.Position;

                    //Reverse item fix
                    //this fix requires scaled parent to be direct parent of the child 
                    Node2D parent = (Node2D)GetParent();
                    if (parent.Scale.x == -1)
                    {
                        it.Scale *= new Vector2(-1, 1);
                    }

                    //spawn item scene
                    CallDeferred("add_child", it);
                }
            }
        }
    }

    //Spawn items under parent
    public void SpawnItemByLevel(Node spawnUnder)
    {
        foreach (var child in GetChildren())
        {
            if (child is SpawnPoint2D)
            {
                SpawnPoint2D sPos = (SpawnPoint2D)child;

                if (sPos.SpawnLevel <= SpawnLevel)
                {
                    //prepare scene
                    var itemToSpawn = ItemScene;
                    var itemNode = itemToSpawn.Instance();
                    Node2D it = (Node2D)itemNode;
                    it.Position = sPos.Position;

                    //spawn item scene
                    spawnUnder.CallDeferred("add_child", it);
                }
            }
        }
    }

    public void SpawnItemByLevel(int spawnLevel, Node spawnUnder)
    {
        foreach (var child in GetChildren())
        {
            if (child is SpawnPoint2D)
            {
                SpawnPoint2D sPos = (SpawnPoint2D)child;

                if (sPos.SpawnLevel <= spawnLevel)
                {
                    //prepare scene
                    var itemToSpawn = ItemScene;
                    var itemNode = itemToSpawn.Instance();
                    Node2D it = (Node2D)itemNode;
                    it.Position = sPos.Position;

                    //Reverse item fix
                    //this fix requires scaled parent to be direct parent of the child 
                    Node2D parent = (Node2D)GetParent();
                    if (parent.Scale.x == -1)
                    {
                        it.Scale *= new Vector2(-1, 1);
                    }

                    //spawn item scene
                    spawnUnder.CallDeferred("add_child", it);
                }
            }
        }
    }

    //Remove items under Spawner
    public void RemoveItems()
    {
        var inScene = ItemScene.Instance();
        foreach (var child in GetChildren())
        {
            if (child.GetType() == inScene.GetType())
            {
                Node2D node = (Node2D)child;
                node.QueueFree();
            }
        }
    }

    //Remove items under parent(Spawner)
    //Warning: remves all instances of ItemScene under parent node no matter what spawned them
    public void RemoveItems(Node spawnedUnder)
    {
        Item inScene = (Item)ItemScene.Instance();
        foreach (var child in spawnedUnder.GetChildren())
        {
            if (child.GetType() == inScene.GetType())
            {
                Node2D node = (Node2D)child;
                node.QueueFree();
            }
        }
    }

    public void RandomizeSpawnPointLevels(int minLevel, int maxLevel)
    {
        foreach (var child in GetChildren())
        {
            if (child is SpawnPoint2D)
            {
                SpawnPoint2D sPos = (SpawnPoint2D)child;

                sPos.SpawnLevel = _randGen.Next(minLevel, maxLevel);
            }
        }
    }

    public void RandomizeSpawnLevel(int minLevel, int maxLevel)
    {
        SpawnLevel = _randGen.Next(minLevel, maxLevel);
    }

    public virtual void RespawnItems()
    {
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetParent().GetParent().GetParent().Connect("SRespawnItems", this, nameof(ONLevelSRespawnItems));
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
