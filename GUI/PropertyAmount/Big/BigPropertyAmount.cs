using Godot;
using System;

public class BigPropertyAmount : CenterContainer
{
    public AnimationPlayer NewRecAnimPlayer;

    private PropertyAmountV _propAmount;
    public CenterContainer _newRecordLabContainer;
    private PropertyAmountH _actualMaximum;

    public void UpdatePropertyAmount(Property property)
    {
        _propAmount.UpdateAmount(property.GetAmount());

        if (property.IsNewRecord)
        {
            _newRecordLabContainer.Show();
            _actualMaximum.Hide();
            _actualMaximum.UpdateAmount(property.GetRecord());
        }
        else
        {
            _newRecordLabContainer.Hide();
            _actualMaximum.Show();
        }
    }

    public void HideNewRecord()
    {
        _newRecordLabContainer.Hide();
    }

    public void PlayShowUpAnim()
    {
        _propAmount.AnimPlayer.Play("ScaleJumpShowUp_anim");
        _actualMaximum.AnimPlayer.Play("ScaleJumpShowUp_anim");
    }

    public void PlayNewRecAnim()
    {
        //GD.Print(_newRecordLabContainer.Visible);
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
        _newRecordLabContainer = GetNode<CenterContainer>("CanvasLayer/CenterContainer");
        _actualMaximum = GetNode<PropertyAmountH>("VBoxContainer/CenterContainer/PropertyAmountH");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
