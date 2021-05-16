using Godot;
using System;

public class Usage : HBoxContainer
{
    private Label _amountLab;
    private Label _maximumLab;

    public void UpdateAmount(PropertyUsage usageProp)
    {
        _amountLab.Text = Convert.ToString(usageProp.GetAmount());
    }

    public void UpdateAmount(int amount)
    {
        _amountLab.Text = Convert.ToString(amount);
    }

    public void UpdateMaximum(PropertyUsage usageProp)
    {
        _maximumLab.Text = Convert.ToString(usageProp.GetMaximum());
    }

    public void UpdateMaximum(int maximum)
    {
        _maximumLab.Text = Convert.ToString(maximum);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _amountLab = GetNode<Label>("AmountLab");
        _maximumLab = GetNode<Label>("MaximumLab");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
