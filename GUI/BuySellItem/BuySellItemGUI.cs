using Godot;
using System;

public class BuySellItemGUI : VBoxContainer
{
    [Export]
    public string ItemKey = "Item";

    [Signal]
    public delegate void SRequestBuyItem(string itemKey);

    [Signal]
    public delegate void SRequestSellItem(string itemKey);

    public void UpdateAmount(Player player)
    {
        AmountGUI amountGui = GetNode<AmountGUI>("ItemAmountContainer/AmountContainer");

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

    public void OnGUISUpdateShopPrizes(Shop shop)
    {
        Label buyPrice = GetNode<Label>("CenterContainer/VBoxContainer/HBoxContainer/BuyPriceLabel");
        Label sellPrice = GetNode<Label>("CenterContainer/VBoxContainer/HBoxContainer/SellPriceLabel");

        sellPrice.Text = Convert.ToString(shop.ShopPrizes[ItemKey].MarketValue + shop.ShopPrizes[ItemKey].ValueSellOffset);
        buyPrice.Text = Convert.ToString(shop.ShopPrizes[ItemKey].MarketValue + shop.ShopPrizes[ItemKey].ValueBuyOffset);

    }

    public void OnBuyButtonDown()
    {
        EmitSignal(nameof(SRequestBuyItem), ItemKey);
    }

    public void OnSellButtonDown()
    {
        EmitSignal(nameof(SRequestSellItem), ItemKey);
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
