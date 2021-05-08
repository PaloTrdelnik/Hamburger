using Godot;
using System;

public class AmountGUI : HBoxContainer
{
    public void UpdateAmount(int amount)
    {
        Label amountLab = GetNode<Label>("Amount");

        amountLab.Text = Convert.ToString(amount);
    }

    public void UpdateAmount(string amount)
    {
        Label amountLab = GetNode<Label>("Amount");

        amountLab.Text = amount;
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
