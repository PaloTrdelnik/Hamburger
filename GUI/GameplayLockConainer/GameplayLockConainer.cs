using Godot;
using System;
using System.Collections.Generic;

public class GameplayLockConainer : Control
{
    [Signal]
    public delegate void SRequestUnlockGLock(string gLockKey);

    private HBoxContainer _hBoxLocksContainer;
    private Label _noAvaliabLabel;

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
        bool bIsSomeGLockAvailable = false;

        foreach (GameplayLockGUI gLockGUI in _hBoxLocksContainer.GetChildren())
        {
            if (dictOfLocks[gLockGUI.GlockKey])
            {
                gLockGUI.Show();
                bIsSomeGLockAvailable = true;
            }
            else
                gLockGUI.Hide();
        }
        if (bIsSomeGLockAvailable)
        {
            _noAvaliabLabel.Hide();
        }
        else
        {
            _noAvaliabLabel.Show();
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _hBoxLocksContainer = GetNode<HBoxContainer>("ScrollContainer/CenterContainer/HBoxContainer");
        _noAvaliabLabel = GetNode<Label>("ScrollContainer/CenterContainer/NoAvailabilityLabel");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
