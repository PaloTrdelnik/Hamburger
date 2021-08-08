using Godot;
using System;
using System.Collections.Generic;

public class Shop : Node2D
{
    [Signal]
    public delegate void SItemSold(string itemKey);

    [Signal]
    public delegate void SItemBought(string itemKey);

    [Signal]
    public delegate void SGLockBought(string gLockKey);

    [Signal]
    public delegate void SLackOfMoneyToBuy();

    [Signal]
    public delegate void SLackOfItemToSell();

    public Dictionary<string, ShopItem> ShopPrizes = new Dictionary<string, ShopItem>
    {
        {"Money", new ShopItem{ MarketValue = 1, ValueBuyOffset = 0, ValueSellOffset = 0 } },
        {"TimeDilation", new ShopItem{ MarketValue = 2, ValueBuyOffset = 0, ValueSellOffset = 0 } },
    };

    public Dictionary<string, ShopGoods> ShopGLocks = new Dictionary<string, ShopGoods>
    {
        {"GL_Jump", new ShopGoods{ MarketValue = 2 } },
        {"GL_AcidRaise", new ShopGoods{ MarketValue = 3 } },
        {"GL_FasterPlayer", new ShopGoods{ MarketValue = 4 } },
        {"GL_BiggerIventory", new ShopGoods{ MarketValue = 5 } },
        {"GL_TimeDilationLonger", new ShopGoods{ MarketValue = 2 } }
    };

    public void OnGUISBuyItem(Player player, string itemKeyToBuy, int amount)
    {
        string MoneyItemKey = "Money";

        //check if items are in inventory
        if (player.Inv.IsInInv(MoneyItemKey, 1))
        {
            int moneyAmount = player.Inv.GetItem(MoneyItemKey).Amount;
            int moneyValueAmount = ShopPrizes[MoneyItemKey].GetRealSellPrize() * moneyAmount;

            int itemToBuyValueAmount = ShopPrizes[itemKeyToBuy].GetRealBuyPrize() * amount;

            if (moneyValueAmount >= itemToBuyValueAmount)
            {
                int numOfItemsToRemove = (ShopPrizes[itemKeyToBuy].GetRealBuyPrize() * amount) / ShopPrizes[MoneyItemKey].GetRealSellPrize();
                if (player.Inv.IsInInv(MoneyItemKey, numOfItemsToRemove))
                {
                    Item iRem = new Item { Amount = numOfItemsToRemove, InvKey = MoneyItemKey };
                    Item iAdd = new Item { Amount = amount, InvKey = itemKeyToBuy };

                    player.Inv.Remove(iRem);
                    player.Inv.Add(iAdd);

                    EmitSignal(nameof(SItemSold), MoneyItemKey);
                    EmitSignal(nameof(SItemBought), itemKeyToBuy);
                }
            }
            else
            {
                EmitSignal(nameof(SLackOfMoneyToBuy));
            }
        }
        else
        {
            EmitSignal(nameof(SLackOfMoneyToBuy));
        }
    }

    public void OnGUISSellItem(Player player, string itemKeyToSell, int amount)
    {
        string MoneyItemKey = "Money";

        //check if items are in inventory
        if (player.Inv.IsInInv(itemKeyToSell, amount))
        {
            int moneyAmountToAdd = ShopPrizes[itemKeyToSell].GetRealSellPrize() * amount / ShopPrizes[MoneyItemKey].GetRealBuyPrize();

            Item iRem = new Item { Amount = amount, InvKey = itemKeyToSell };
            Item iAdd = new Item { Amount = moneyAmountToAdd, InvKey = MoneyItemKey };

            player.Inv.Remove(iRem);
            player.Inv.Add(iAdd);

            EmitSignal(nameof(SItemSold), itemKeyToSell);
            EmitSignal(nameof(SItemBought), MoneyItemKey);
        }
        else
        {
            EmitSignal(nameof(SLackOfItemToSell));
        }
    }

    public void OnGUISBuyGLock(Player player, string gLockKey)
    {
        string MoneyItemKey = "Money";

        //check if items are in inventory
        if (player.Inv.IsInInv(MoneyItemKey, 1))
        {
            int moneyAmount = player.Inv.GetItem(MoneyItemKey).Amount;
            int moneyValueAmount = ShopPrizes[MoneyItemKey].GetRealSellPrize() * moneyAmount;

            int gLockToBuyValueAmount = ShopGLocks[gLockKey].GetRealBuyPrize();

            if (moneyValueAmount >= gLockToBuyValueAmount)
            {
                int numOfItemsToRemove = ShopGLocks[gLockKey].GetRealBuyPrize();

                Item iRem = new Item { Amount = numOfItemsToRemove, InvKey = MoneyItemKey };

                player.Inv.Remove(iRem);

                EmitSignal(nameof(SItemSold), MoneyItemKey);
                EmitSignal(nameof(SGLockBought), gLockKey);
            }
            else
            {
                EmitSignal(nameof(SLackOfMoneyToBuy));
            }
        }
        else
        {
            EmitSignal(nameof(SLackOfMoneyToBuy));
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

    //actual implementation dont use it
    //works only for items with same value
    //private void Trade(Player player, string itemKeyFrom, string itemKeyTo)
    //{
    //    //check if items are in inventory
    //    if (player.Inv.IsInInv(itemKeyFrom, 1))
    //    {
    //        int itemFromAmount = player.Inv.GetItem(itemKeyFrom).Amount;
    //        int itemFromValueOfAmount = ShopPrizes[itemKeyFrom].GetRealSellPrize() * itemFromAmount;

    //        int itemToValueOfAmount = ShopPrizes[itemKeyTo].GetRealBuyPrize();

    //        if (itemFromValueOfAmount >= itemToValueOfAmount)
    //        {
    //            int numOfItemsToRemove = ShopPrizes[itemKeyTo].GetRealSellPrize() / ShopPrizes[itemKeyFrom].GetRealBuyPrize();

    //            if (player.Inv.IsInInv(itemKeyFrom, numOfItemsToRemove))
    //            {
    //                Item iRem = new Item { Amount = numOfItemsToRemove, InvKey = itemKeyFrom };
    //                Item iAdd = new Item { Amount = 1, InvKey = itemKeyTo };

    //                player.Inv.Remove(iRem);
    //                player.Inv.Add(iAdd);

    //                EmitSignal(nameof(SItemSold), itemKeyFrom);
    //                EmitSignal(nameof(SItemBought), itemKeyTo);
    //            }
    //        }
    //    }
    //}

}
