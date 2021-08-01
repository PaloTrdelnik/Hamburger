using Godot;
using System;
using System.Collections.Generic;

public class GameplayLockConainer : Control
{
    [Signal]
    public delegate void SRequestUnlockGLock(string gLockKey);

    private HBoxContainer _hBoxLocksContainer;

    public void OnGUISUpdateShopPrizes(Shop shop)
    {
        foreach (GameplayLockGUI gLockGUI in _hBoxLocksContainer.GetChildren())
        {
            gLockGUI.SUpdatePrize(shop);
        }
    }

    public void OnGameplayLockGUISRequestUnlockGLock(string gLockKey)
    {
        EmitSignal(nameof(SRequestUnlockGLock), gLockKey);
    }

    public void UpdateAvailableGLocksVisibility(Dictionary<string, bool> dictOfLocks)
    {
        foreach (GameplayLockGUI gLockGUI in _hBoxLocksContainer.GetChildren())
        {
            //GD.Print(dictOfLocks[gLockGUI.GlockKey]);
            if (dictOfLocks[gLockGUI.GlockKey])
                gLockGUI.Show();
            else
                gLockGUI.Hide();
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _hBoxLocksContainer = GetNode<HBoxContainer>("ScrollContainer/CenterContainer/HBoxContainer");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
