using Godot;
using System;

public class TaskBarItem : AspectRatioContainer
{
    [Export]
    public string ItemKey = "Item";

    private bool _updatePBar = false;
    private Timer _uDurabilityTimer;

    public void UpdateAmount(Player player)
    {
        AmountGUI amountGui = GetNode<AmountGUI>("VBoxContainer/CenterContainer/AmountContainer");

        if (player.Inv.IsInInv(ItemKey, 1))
        {
            Item itemInv = (Item)player.Inv.GetItem(ItemKey);

            amountGui.UpdateAmount(Convert.ToString(itemInv.Amount));
        }
        else
        {
            amountGui.UpdateAmount(0);
        }
    }

    public void StartUpdateUseDurabilityBar(Player player)
    {
        _uDurabilityTimer = player.GetNode<ItemUser>("ItemUser").GetItem(ItemKey).UseDurabilityTimer;
        ProgressBar pBar = GetNode<ProgressBar>("ProgressBar");

        pBar.MaxValue = _uDurabilityTimer.WaitTime;
        GD.Print(_uDurabilityTimer.TimeLeft);
        _updatePBar = true;        
    }

    public void StopUpdateUseDurabilityBar()
    {
        ProgressBar pBar = GetNode<ProgressBar>("ProgressBar");

        pBar.Value = 0;
        _updatePBar = false;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (_updatePBar)
        {
            ProgressBar pBar = GetNode<ProgressBar>("ProgressBar");

            //GD.Print("Update progress bar");

            pBar.Value = _uDurabilityTimer.TimeLeft;
            pBar.Update();
        }
    }
}
