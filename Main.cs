using Godot;
using System;

public class Main : Node2D
{
	[Signal]
	public delegate void SLevelLoaded(Node level);

	[Signal]
	public delegate void SLevelUnloaded();

	public Level ActualLevel;
	
	public void OnGUISLoadlevel(string resPath)
	{
		var levelToLoad = (PackedScene)ResourceLoader.Load(resPath);
		var levelNode = levelToLoad.Instance();

		this.AddChild(levelNode);
		ActualLevel = (Level)levelNode;

		EmitSignal(nameof(SLevelLoaded), levelNode);
	}

	public void OnGUISUnloadLevel()
	{
		ActualLevel.QueueFree();
		EmitSignal(nameof(SLevelUnloaded));
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
