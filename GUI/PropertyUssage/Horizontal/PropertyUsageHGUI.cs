using Godot;
using System;

public class PropertyUsageHGUI : PropertyUsageGUI
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _usageCont = GetNode<Usage>("HBoxContainer/Usage");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
