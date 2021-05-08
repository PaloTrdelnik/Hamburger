using Godot;
using System;

public class ItemAmountGUI : HBoxContainer
{
    [Export]
    public string ItemKey = "Item";

    public void UpdateAmount(Player player)
    {
        AmountGUI amountGui = GetNode<AmountGUI>("AmountContainer");

        if (player.Inv.IsInInv(ItemKey, 1))
        {
            Item itemInv = (Item)player.Inv.GetItem(ItemKey);

            amountGui.UpdateAmount(Convert.ToString(itemInv.Amount));
        }
        else
        {
            amountGui.UpdateAmount(0);
        }
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
