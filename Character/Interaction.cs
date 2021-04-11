using Godot;
using System;

public class Interaction : Area2D
{
    [Signal]
    public delegate void SAddToInv(Item item);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_Interaction_area_entered(Area2D area)
    { 
        if (area is Item)
        {
            Collect((Item)area);
        }
    }

    private void Collect(Item item)
    {
        EmitSignal(nameof(SAddToInv), item);
        item.QueueFree();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
