using Godot;
using System;

public class PropertyAmountV : PropertyAmount
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _amountGui = GetNode<AmountGUI>("VBoxContainer/CenterContainer/AmountContainer");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}