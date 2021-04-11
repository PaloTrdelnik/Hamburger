using Godot;
using System;

public class Level : Node2D
{
    [Signal]
    public delegate void SPlayerGO(Player player);

    [Signal]
    public delegate void SPlayerInvItemChange(Player player, string itemInvKey);

    public void OnPlayerSInvItemChange(Player player, Item item)
    {
        EmitSignal(nameof(SPlayerInvItemChange), player, item.InvKey);
    }

    public void OnAcidBodyEntered(Node body)
    {
        if (body is Player)
        {
            Player player = (Player)body;

            EmitSignal(nameof(SPlayerGO), body);
            player.bFreeze = true;
        }
    }

    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
