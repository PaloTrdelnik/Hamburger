using Godot;
using System;

public class ShopItem : Node
{
    public int MarketValue = 0;
    public int ValueBuyOffset = 0;
    public int ValueSellOffset = 0;

    public int GetRealBuyPrize()
    {
        return MarketValue + ValueBuyOffset;
    }
    public int GetRealSellPrize()
    {
        return MarketValue + ValueSellOffset;
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
