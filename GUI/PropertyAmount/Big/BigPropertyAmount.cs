using Godot;
using System;

public class BigPropertyAmount : CenterContainer
{
    public AnimationPlayer NewRecAnimPlayer;

    private PropertyAmountV _propAmount;
    private CenterContainer _newRecordLab;
    private PropertyAmountH _actualMaximum;

    public void UpdatePropertyAmount(Property property)
    {
        _propAmount.UpdateAmount(property.GetAmount());

        if (property.IsNewRecord)
        {
            _newRecordLab.Show();
            _actualMaximum.Hide();

            _actualMaximum.UpdateAmount(property.GetRecord());
        }
        else
        {
            _newRecordLab.Hide();
            _actualMaximum.Show();
        }
    }

    public void HideNewRecord()
    {
        _newRecordLab.Hide();
    }

    public void PlayShowUpAnim()
    {
        _propAmount.AnimPlayer.Play("ScaleJumpShowUp_anim");
        _actualMaximum.AnimPlayer.Play("ScaleJumpShowUp_anim");
    }

    public void PlayNewRecAnim()
    {
        NewRecAnimPlayer.Play("Fall_anim");
    }

    public void PlayHideAnim()
    {
        _propAmount.PlayHideAnim();
        _actualMaximum.PlayHideAnim();
    }

    public void ResetAnim()
    {
        _propAmount.ResetAnim();
        _actualMaximum.ResetAnim();

        NewRecAnimPlayer.Play("Fall_anim");
        NewRecAnimPlayer.Seek(0.0f, true);
        NewRecAnimPlayer.Stop();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        NewRecAnimPlayer = GetNode<AnimationPlayer>("CanvasLayer/CenterContainer/AnimationPlayer");

        _propAmount = GetNode<PropertyAmountV>("VBoxContainer/PropertyAmountV");
        _newRecordLab = GetNode<CenterContainer>("CanvasLayer/CenterContainer");
        _actualMaximum = GetNode<PropertyAmountH>("VBoxContainer/CenterContainer/PropertyAmountH");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
