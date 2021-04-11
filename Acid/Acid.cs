using Godot;
using System;

public class Acid : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        //Vector2 movedPosition = new Vector2(Position.x, Position.y - 10 * delta);
        //Position = movedPosition;
        //GD.Print(Scale.y + 10 * delta / 4800 * Scale.y);
        Vector2 higherScale = new Vector2(1, Scale.y + 0.2f * delta );
        Scale = higherScale;
    }
}
