using Godot;
using System;

public class ButtonImage : CenterContainer
{
    public AnimationPlayer AnimPlayer;
    private TextureRect TexRect;

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
        AnimPlayer.Play("ShowUp_anim");
        AnimPlayer.Seek(0.0f, true);
        AnimPlayer.Stop();
    }

    public void SwapTexture(Texture texture)
    {
        TexRect.Texture = texture;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AnimPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        TexRect = GetNode<TextureRect>("TextureRect");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
