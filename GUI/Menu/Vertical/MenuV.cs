using Godot;
using System;

public class MenuV : VBoxContainer
{
    public async void PlayShowUpAnim()
    {
        foreach (var child in GetChildren())
        {
            if (child is CenterContainer)
            {
                CenterContainer centerCild = (CenterContainer)child;

                if (centerCild.GetChild(0) is ButtonText)
                {
                    ButtonText button = (ButtonText)centerCild.GetChild(0);

                    button.PlayShowUpAnim();
                    await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
                }
                if (centerCild.GetChild(0) is ButtonImage)
                {
                    ButtonImage button = (ButtonImage)centerCild.GetChild(0);

                    button.PlayShowUpAnim();
                    await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
                }
            }
        }
    }

    public async void PlayHideAnim()
    {
        foreach (var child in GetChildren())
        {
            if (child is CenterContainer)
            {
                CenterContainer centerCild = (CenterContainer)child;

                if (centerCild.GetChild(0) is ButtonText)
                {
                    ButtonText button = (ButtonText)centerCild.GetChild(0);

                    button.PlayHideAnim();
                    await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
                }
                if (centerCild.GetChild(0) is ButtonImage)
                {
                    ButtonImage button = (ButtonImage)centerCild.GetChild(0);

                    button.PlayHideAnim();
                    await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
                }
            }
        }
    }

    public void ResetAnim()
    {
        foreach (var child in GetChildren())
        {
            if (child is CenterContainer)
            {
                CenterContainer centerCild = (CenterContainer)child;

                if (centerCild.GetChild(0) is ButtonText)
                {
                    ButtonText button = (ButtonText)centerCild.GetChild(0);

                    button.ResetAnim();
                }
                if (centerCild.GetChild(0) is ButtonImage)
                {
                    ButtonImage button = (ButtonImage)centerCild.GetChild(0);

                    button.ResetAnim();
                }
            }
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
