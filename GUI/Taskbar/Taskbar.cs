using Godot;
using System;

public class Taskbar : HBoxContainer
{
    public void UpdateAmount(Player player, string itemKey)
    {
        TaskBarItem tBarItem = GetNode<TaskBarItem>("TaskBar" + itemKey);

        tBarItem.UpdateAmount(player);
    }

    public void UpdateAmountOfAllitems(Player player)
    {
        var allTaskBarItems = GetChildren();

        foreach(TaskBarItem item in allTaskBarItems)
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
