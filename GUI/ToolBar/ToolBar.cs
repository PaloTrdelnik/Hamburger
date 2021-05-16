using Godot;
using System;

public class ToolBar : HBoxContainer
{
    public void UpdateAmount(Player player, string itemKey)
    {
        ToolBarItem tBarItem = GetNode<ToolBarItem>("ToolBar" + itemKey);

        tBarItem.UpdateAmount(player);
    }

    public void UpdateAmountOfAllitems(Player player)
    {
        var allToolBarItems = GetChildren();

        foreach(ToolBarItem item in allToolBarItems)
        {
            item.UpdateAmount(player);
        }
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
