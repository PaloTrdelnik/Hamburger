using Godot;
using System;

public class ContextualInfo : PanelContainer
{
    protected Label _infoLabel;
    protected CenterContainer _cContainer;

    public void ShowInfo(string infoText)
    {
        SetInfoText(infoText);
        _cContainer.MouseFilter = MouseFilterEnum.Stop;
        Show();
    }

    public void SetInfoText(string infoText)
    {
        _infoLabel.Text = infoText;
    }

    public string GetInfoText()
    {
        return _infoLabel.Text;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _infoLabel = GetNode<Label>("VBoxContainer/CenterContainer/Label");
        _cContainer = (CenterContainer)GetParent();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
