using Godot;
using System;

public class PropertyAmount : AspectRatioContainer
{
    public AnimationPlayer AnimPlayer;

    protected AmountGUI _amountGui;

    public void UpdateAmount(string amount)
    {
        _amountGui.UpdateAmount(amount);
    }

    public void UpdateAmount(int amount)
    {
        _amountGui.UpdateAmount(amount);
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
