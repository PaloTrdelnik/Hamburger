using Godot;
using System;
using System.Collections.Generic;

public class Shop : Node2D
{
    [Signal]
    public delegate void SItemSold(string itemKey);

    [Signal]
    public delegate void SItemBought(string itemKey);

    //RealPrizes have to be same for all items
    public Dictionary<string, ShopItem> ShopPrizes = new Dictionary<string, ShopItem>
    {
        {"Money", new ShopItem{ MarketValue = 1, ValueBuyOffset = 0, ValueSellOffset = 0 } },
        {"TimeDilation", new ShopItem{ MarketValue = 1, ValueBuyOffset = 0, ValueSellOffset = 0 } },
    };

    public void OnGUISBuyItem(Player player, string itemKey)
    {
        Trade(player, "Money", itemKey);
    }

    public void OnGUISSellItem(Player player, string itemKey)
    {
        Trade(player, itemKey, "Money");
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

    private void Trade(Player player, string itemKeyFrom, string itemKeyTo)
    {
        //check if items are in inventory
        if (player.Inv.IsInInv(itemKeyFrom, 1))
        {
            int itemFromAmount = player.Inv.GetItem(itemKeyFrom).Amount;
            int itemFromValueOfAmount = ShopPrizes[itemKeyFrom].GetRealSellPrize() * itemFromAmount;

            int itemToValueOfAmount = ShopPrizes[itemKeyTo].GetRealBuyPrize();

            if (itemFromValueOfAmount >= itemToValueOfAmount)
            {
                int numOfItemsToRemove = ShopPrizes[itemKeyTo].GetRealSellPrize() / ShopPrizes[itemKeyFrom].GetRealBuyPrize();

                if (player.Inv.IsInInv(itemKeyFrom, numOfItemsToRemove))
                {
                    Item iRem = new Item { Amount = numOfItemsToRemove, InvKey = itemKeyFrom };
                    Item iAdd = new Item { Amount = 1, InvKey = itemKeyTo };

                    player.Inv.Remove(iRem);
                    player.Inv.Add(iAdd);

                    EmitSignal(nameof(SItemSold), itemKeyFrom);
                    EmitSignal(nameof(SItemBought), itemKeyTo);
                }
            }
        }
    }

}
