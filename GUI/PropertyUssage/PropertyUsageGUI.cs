using Godot;
using System;

public class PropertyUsageGUI : AspectRatioContainer
{
    public AnimationPlayer AnimPlayer;
    protected Usage _usageCont;

    public void UpdateAmount(PropertyUsage usageProp)
    {
        _usageCont.UpdateAmount(usageProp);
    }

    public void UpdateAmount(int amount)
    {
        _usageCont.UpdateAmount(amount);
    }

    public void UpdateMaximum(PropertyUsage usageProp)
    {
        _usageCont.UpdateMaximum(usageProp);
    }

    public void UpdateMaximum(int maximum)
    {
        _usageCont.UpdateMaximum(maximum);
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
