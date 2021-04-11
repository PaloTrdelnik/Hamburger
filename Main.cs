using Godot;
using System;

public class Main : Node2D
{
    [Signal]
    public delegate void SLevelLoaded(Node level);
    
    public void OnGUISLoadlevel(string resPath)
    {
        LoadLevel(resPath);
    }

    public void LoadLevel(string resPath)
    {
        var levelToLoad = (PackedScene)ResourceLoader.Load(resPath);
        var levelNode = levelToLoad.Instance();
        
        this.AddChild(levelNode);

        EmitSignal(nameof(SLevelLoaded), levelNode);
    }
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
