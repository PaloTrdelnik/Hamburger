using Godot;
using System;

public class ToolBarItem : MarginContainer
{
    [Export]
    public string ItemKey = "Item";

    public AnimationPlayer AnimPlayer;

    private Timer _uDurabilityTimer;
    private ItemAmountImageContainer _itemAmountGui;
    private ProgressBar _pBar;

    public void UpdateAmount(Player player)
    {
        _itemAmountGui.UpdateAmount(player);
    }

    public void PlayShowUpAnim()
    {
        AnimPlayer.Play("ShowUp_anim");
    }

    public void PlayUseItemAnim(Player player)
    {
        _uDurabilityTimer = player.GetNode<ItemUser>("ItemUser").GetItem(ItemKey).UseDurabilityTimer;

        _itemAmountGui.PlayUpdateAmountAnim();

        ResetAnim("UseItem_anim");
        AnimPlayer.Play("UseItem_anim", customSpeed: 1/ _uDurabilityTimer.WaitTime);

    }

    public void PlayHideAnim()
    {
        AnimPlayer.Play("Hide_anim");
    }

    public void ResetAnim()
    {
        ResetAnim("UseItem_anim");

        AnimPlayer.Play("ShowUp_anim");
        AnimPlayer.Seek(0.0f, true);
        AnimPlayer.Stop();
    }

    public void ResetAnim(string animationName)
    {
        AnimPlayer.Play(animationName);
        AnimPlayer.Seek(0.0f, true);
        AnimPlayer.Stop();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _itemAmountGui = GetNode<ItemAmountImageContainer>("ItemAmountImageContainer");
        _pBar = GetNode<ProgressBar>("ProgressBar");
        AnimPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }
}
