using Godot;
using System;

public class Item : Area2D
{
    [Signal]
    public delegate void SItemUsed(string itemKey);

    [Export]
    public int Amount = 0;

    [Export]
    public string InvKey = "Item";

    [Export]
    public bool CanUseSimultaneously = false;

    public Timer UseDurabilityTimer;

    [Export]
    public float UseDurabilityTime = 10.0f; // length in milliseconds

    //[Export]
    //public int DurabisityTime = 0; // length in milliseconds

    //[Export]
    //public int UseTime = 0; // length in milliseconds

    public void OnTUseDurabilityEnded()
    {
        //GD.Print("Timer");
        EmitSignal(nameof(SItemUsed), InvKey);
        UseDurabilityTimer.Stop();
        UseDurabilityTimer.QueueFree();
    }

    public Item()
    {
        //set ref and set UseDurability timer 
        UseDurabilityTimer = new Timer();
        UseDurabilityTimer.WaitTime = UseDurabilityTime;
        UseDurabilityTimer.Connect("timeout", this, nameof(OnTUseDurabilityEnded));
    }

    public override void _Ready()
    {
        
    }


    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

}