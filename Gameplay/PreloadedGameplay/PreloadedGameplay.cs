using Godot;
using System;

public abstract class PreloadedGameplay : Node
{
    //TODO:Extend functionality with Loader class (you have to create it), to be able to find out which datas are actual.

    public Node DataChild;

    public bool DataChildLoaded = false;

    public abstract void MoveDataToDataChild();

    public abstract void CopyDataFromDataChild();

    //// Called when the node enters the scene tree for the first time.
    //public override void _Ready()
    //{

    //}

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
