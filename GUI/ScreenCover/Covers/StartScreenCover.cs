using Godot;
using System;

public class StartScreenCover : ScreenCover
{
    public AnimationPlayer AnimPlayer;

    public void PlayBlinkAnim()
    {
        AnimPlayer.Play("Blink_anim");
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AnimPlayer = GetNode<AnimationPlayer>("ColorRect/CenterContainer/AnimationPlayer");

        //get refs to nodes initialized in parent class by calling parents _Ready() function
        //Childs _Ready() in this case runs first
        base._Ready();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
