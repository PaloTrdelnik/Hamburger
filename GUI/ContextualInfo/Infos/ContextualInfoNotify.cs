using Godot;
using System;

public class ContextualInfoNotify : ContextualInfo
{
    [Signal]
    public delegate void SConfirmed();

    public void OnConfirmationButtonTextButtonDown()
    {
        Hide();
        _cContainer.MouseFilter = MouseFilterEnum.Ignore;
        EmitSignal(nameof(SConfirmed));
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
