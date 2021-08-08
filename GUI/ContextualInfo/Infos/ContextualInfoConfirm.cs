using Godot;
using System;

public class ContextualInfoConfirm : ContextualInfo
{
    [Signal]
    public delegate void SConfirmed();

    [Signal]
    public delegate void SCanceled();

    public void OnConfirmationButtonTextButtonDown()
    {
        Hide();
        _cContainer.MouseFilter = MouseFilterEnum.Ignore;
        EmitSignal(nameof(SConfirmed));
    }

    public void OnCancelButtonTextButtonDown()
    {
        Hide();
        _cContainer.MouseFilter = MouseFilterEnum.Ignore;
        EmitSignal(nameof(SCanceled));
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
