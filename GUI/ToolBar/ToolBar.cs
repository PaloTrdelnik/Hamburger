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

    public async void PlayShowUpAnim()
    {
        foreach (var child in GetChildren())
        {
            if (child is ToolBarItem)
            {
                ToolBarItem toolBarItemChild = (ToolBarItem)child;

                toolBarItemChild.PlayShowUpAnim();
                await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
            }
        }
    }

    public async void PlayHideAnim()
    {
        foreach (var child in GetChildren())
        {
            if (child is ToolBarItem)
            {
                ToolBarItem toolBarItemChild = (ToolBarItem)child;

                toolBarItemChild.PlayHideAnim();
                await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
            }
        }
    }

    public void ResetAnim()
    {
        foreach (var child in GetChildren())
        {
            if (child is ToolBarItem)
            {
                ToolBarItem toolBarItemChild = (ToolBarItem)child;

                toolBarItemChild.ResetAnim();
            }
        }
    }

    public void ResetAnim(string animationName)
    {
        foreach (var child in GetChildren())
        {
            if (child is ToolBarItem)
            {
                ToolBarItem toolBarItemChild = (ToolBarItem)child;

                toolBarItemChild.ResetAnim(animationName);
            }
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
