using Godot;
using System;

public class ButtonText : Button
{
    public AnimationPlayer AnimPlayer;

    public void PlayShowUpAnim()
    {
        AnimPlayer.Play("ShowUp_anim");
    }

    public void PlayHideAnim()
    {
        AnimPlayer.Play("Hide_anim");
    }

    public void ResetAnim()
    {
        AnimPlayer.Stop();
        AnimPlayer.Play("ShowUp_anim");
        AnimPlayer.Seek(0.0f, true);
        AnimPlayer.Stop();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AnimPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
