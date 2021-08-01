using Godot;
using System;

public class ButtonImageMute : ButtonImage
{
    [Export]
    public Texture PlayingTexture;

    [Export]
    public Texture MutedTexture;

    public void SwapTexture(bool bMuted)
    {
        Texture tex;

        if (bMuted)
        {
            tex = PlayingTexture;
        }
        else
        {
            tex = MutedTexture;
        }

        base.SwapTexture(tex);
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
