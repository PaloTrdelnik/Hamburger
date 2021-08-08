using Godot;
using System;

public class ItemContainer : Control
{
    [Signal]
    public delegate void SRequestBuyItem(string itemKey);

    [Signal]
    public delegate void SRequestSellItem(string itemKey);

    private VBoxContainer _vBoxItemContainer;

    public void UpdateAmount(Player player)
    {
        foreach (BuySellItemGUI bsItemGUI in _vBoxItemContainer.GetChildren())
        { 
            bsItemGUI.UpdateAmount(player);
        }
    }

    public void UpdateAmount(Player player, string itemKey)
    {
        foreach (BuySellItemGUI bsItemGUI in _vBoxItemContainer.GetChildren())
        {
            if (bsItemGUI.ItemKey == itemKey)
                bsItemGUI.UpdateAmount(player);
        }
    }

    public void OnGUISUpdateShopPrizes(Shop shop)
    {
        foreach (BuySellItemGUI bsItemGUI in _vBoxItemContainer.GetChildren())
        {
            bsItemGUI.UpdateShopPrizes(shop);
        }
    }

    public void OnBuySellItemGUISRequestBuyItem(string itemKey)
    {
        EmitSignal(nameof(SRequestBuyItem), itemKey);
    }

    public void OnBuySellItemGUISRequestSellItem(string itemKey)
    {
        EmitSignal(nameof(SRequestSellItem), itemKey);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _vBoxItemContainer = GetNode<VBoxContainer>("ScrollContainer/CenterContainer/VBoxContainer");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
