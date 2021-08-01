using Godot;
using System;

public class GameplayLockGUI : VBoxContainer
{
    [Export]
    public string GlockKey = "GL_";

    [Signal]
    public delegate void SRequestUnlockGLock(string gLockKey);

    private AmountGUI _buyPriceAmount;

    public void SUpdatePrize(Shop shop)
    {
        if (Visible)
        {
            _buyPriceAmount.UpdateAmount(shop.ShopGLocks[GlockKey].MarketValue);
        }
    }
    public void OnUnlockButtonTextButtonDown()
    {
        EmitSignal(nameof(SRequestUnlockGLock), GlockKey);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _buyPriceAmount = GetNode<AmountGUI>("CenterContainer/HBoxContainer/BuyPriceAmountContainer");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
