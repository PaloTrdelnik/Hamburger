using Godot;
using System;

public class ShopItem : ShopGoods
{
    public int ValueBuyOffset = 0;
    public int ValueSellOffset = 0;

    public override int GetRealBuyPrize()
    {
        return MarketValue + ValueBuyOffset;
    }
    public override int GetRealSellPrize()
    {
        return MarketValue + ValueSellOffset;
    }

    //// Called when the node enters the scene tree for the first time.
    //public override void _Ready()
    //{
        
    //}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
