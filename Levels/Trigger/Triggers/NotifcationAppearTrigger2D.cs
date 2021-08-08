using Godot;
using System;

public class NotifcationAppearTrigger2D : Area2D
{
    [Export]
    public NodePath NotifyPath;

    public Notification Notify;
    private bool _bFirstShow = true;

    public void OnNotifcationAppearTrigger2DBodyEntered(Node node)
    {
        if (_bFirstShow)
            Notify.PlayShowUpAnim();
    }

    public void OnNotifcationAppearTrigger2DBodyExited(Node node)
    {
        if (_bFirstShow)
        {
            Notify.PlayHideAnim();
            _bFirstShow = false;
        } 
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Notify = GetNode<Notification>(NotifyPath);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
