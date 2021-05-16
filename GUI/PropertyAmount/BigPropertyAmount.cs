using Godot;
using System;

public class BigPropertyAmount : CenterContainer
{
    private PropertyAmountV _propAmount;
    private Label _newRecordLab;
    private PropertyAmountH _actualRecord;

    public void UpdatePropertyAmount(Property property)
    {
        _propAmount.UpdateAmount(property.GetAmount());

        if (property.IsNewRecord)
        {
            _newRecordLab.Show();
            _actualRecord.Hide();

            _actualRecord.UpdateAmount(property.GetRecord());
        }
        else
        {
            _newRecordLab.Hide();
            _actualRecord.Show();
        }
    }

    public void HideNewRecord()
    {
        _newRecordLab.Hide();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _propAmount = GetNode<PropertyAmountV>("VBoxContainer/PropertyAmountV");
        _newRecordLab = GetNode<Label>("CanvasLayer/NewRecordLab");
        _actualRecord = GetNode<PropertyAmountH>("VBoxContainer/CenterContainer/PropertyAmountH");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
