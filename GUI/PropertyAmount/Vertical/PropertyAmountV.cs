using Godot;
using System;

public class PropertyAmountV : PropertyAmount
{
    public void OnAmountResized()
    {
        //Update Pivot offset becouse of animation from center
        Label amountLab = _amountGui.GetNode<Label>("Amount");
        amountLab.ForceUpdateTransform();
        //amountLab.Show();
        amountLab.RectPivotOffset = amountLab.RectSize / 2.0f;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AnimPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        _amountGui = GetNode<AmountGUI>("VBoxContainer/CenterContainer/AmountContainer");
    }

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

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
