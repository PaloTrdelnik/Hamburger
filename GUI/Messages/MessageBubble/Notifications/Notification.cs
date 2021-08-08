using Godot;
using System;

public class Notification : MessageBubble
{
    public void Satisfy()
    {
        Hide();
    }

    public void UnSatisfy()
    {
        Show();
    }
    public void CheckSatisfaction(bool satisfy)
    {
        if (satisfy)
            UnSatisfy();
        else
            Satisfy();
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
