using Godot;
using System;

public class UseTimer : Timer
{
    [Signal]
    public delegate void SRefTimeout(Timer timer);

    public void OnUseTimertimeout()
    {
        EmitSignal(nameof(SRefTimeout), this);
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
