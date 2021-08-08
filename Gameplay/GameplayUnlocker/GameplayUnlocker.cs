using Godot;
using System;
using System.Collections.Generic;


public class GameplayUnlocker : Node2D
{
    [Signal]
    public delegate void SGameplayLockUnlocked(string gLockKey);

    private GUI _gui;
    private Player _player;
    private PreloadedGameplay_Level _preloadedLevel;
    //private Level _level;

    public Dictionary<string, GameplayLock> GLocks = new Dictionary<string, GameplayLock>{ };

    private Dictionary<string, bool> _prevAvailabGLocks = new Dictionary<string, bool> { };

    public void OnShopSGLockBought(string gLockKey)
    {
        GLocks[gLockKey].Unlock();

        EmitSignal(nameof(SGameplayLockUnlocked), gLockKey);
    }

    public Dictionary<string, bool> GetDictOfLocksAvilability()
    {
        Dictionary<string, bool> dictOfLocks = new Dictionary<string, bool>{ };

        foreach (KeyValuePair<string, GameplayLock> dictPair in GLocks)
        {
            dictOfLocks.Add(dictPair.Key, dictPair.Value.IsAvailable());
        }

        _prevAvailabGLocks = dictOfLocks;
        return dictOfLocks;
    }

    public bool IsNewGLockAvailable()
    {
        bool bEqalAvailability = true;

        foreach (KeyValuePair<string, GameplayLock> dictPair in GLocks)
        {
            bEqalAvailability = bEqalAvailability && _prevAvailabGLocks[dictPair.Key] == dictPair.Value.IsAvailable();
        }

        return !bEqalAvailability;
    }

    public override void _Ready()
    {
        _gui = GetParent().GetNode<GUI>("GUI");
        _player = GetParent().GetNode<Player>("Player");
        _preloadedLevel = GetParent().GetNode<PreloadedGameplay_Level>("PreloadedGameplay_Level");

        //define all GameplayLocks
        GLocks.Add("GL_Jump", new GameplayLock_Jump { LockedPlayer = _player , ReachedScore = 20});
        GLocks.Add("GL_AcidRaise", new GameplayLock_AcidRaise { LockedPlayer = _player, PercentageMultiplier = 0.85f, PreloadedLevel = _preloadedLevel, ReachedScore = 25 });
        GLocks.Add("GL_FasterPlayer", new GameplayLock_FasterPlayer { LockedPlayer = _player, PercentageMultiplier = 1.15f, ReachedScore = 30 });
        GLocks.Add("GL_BiggerIventory", new GameplayLock_BiggerInventory { LockedPlayer = _player, PlacesAdded = 5, ReachedScore = 50 }) ;
        GLocks.Add("GL_TimeDilationLonger", new GameplayLock_TimeDilationLonger { LockedPlayer = _player,TimeAdded = 1f, ReachedScore = 700 });

        //call this function to fill _prevAvailabGLocks dictionary
        //TODO:Remake this code, because is sets dictionary two times (function, here)
        _prevAvailabGLocks = GetDictOfLocksAvilability();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
