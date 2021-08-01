using Godot;
using System;

public class ShopGoods : Node
{
    public int MarketValue = 0;

    public virtual int GetRealBuyPrize()
    {
        return MarketValue;
    }
    public virtual int GetRealSellPrize()
    {
        return MarketValue;
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
