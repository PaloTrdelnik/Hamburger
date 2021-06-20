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

    private AmountGUI _sellPriceAmount;
    private AmountGUI _buyPriceAmount;

    public void UpdateAmount(Player player)
    {
        AmountGUI amountGui = GetNode<AmountGUI>("CenterContainer2/ItemAmountTextContainer/AmountContainer");

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
        _buyPriceAmount.UpdateAmount(shop.ShopPrizes[ItemKey].GetRealBuyPrize());
        _sellPriceAmount.UpdateAmount(shop.ShopPrizes[ItemKey].GetRealSellPrize());
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
        _buyPriceAmount = GetNode<AmountGUI>("CenterContainer/VBoxContainer/HBoxContainer/BuyPriceAmountContainer");
        _sellPriceAmount = GetNode<AmountGUI>("CenterContainer/VBoxContainer/HBoxContainer2/SellPriceAmountContainer");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
