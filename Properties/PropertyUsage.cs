using Godot;
using System;

public class PropertyUsage : Node
{
    private int _amount = 0;
    private int _maximum = 0;

    public void SetAmount(int amount)
    {
        _amount = amount;
    }

    public int GetAmount()
    {
        return _amount;
    }

    public void SetMaximum(int amount)
    {
        _maximum = amount;
    }

    public int GetMaximum()
    {
        return _maximum;
    }

    public bool IsUsageMaximal()
    {
        return _amount == _maximum;
    }

    public PropertyUsage(int amount, int maximum)
    {
        _amount = amount;
        _maximum = maximum;
    }

    public static PropertyUsage operator +(PropertyUsage a, PropertyUsage b)
    {
        PropertyUsage tmpProp = new PropertyUsage( a.GetAmount(),  a.GetMaximum());
        tmpProp.SetAmount(tmpProp.GetAmount() + b.GetAmount());
        return tmpProp;
    }

    public static PropertyUsage operator +(PropertyUsage a, int b)
    {
        PropertyUsage tmpProp = new PropertyUsage( a.GetAmount(),  a.GetMaximum());
        tmpProp.SetAmount(tmpProp.GetAmount() + b);
        return tmpProp;
    }

    public static PropertyUsage operator -(PropertyUsage a, PropertyUsage b)
    {
        PropertyUsage tmpProp = new PropertyUsage( a.GetAmount(),  a.GetMaximum());
        tmpProp.SetAmount(tmpProp.GetAmount() - b.GetAmount());
        return tmpProp;
    }

    public static PropertyUsage operator -(PropertyUsage a, int b)
    {
        PropertyUsage tmpProp = new PropertyUsage( a.GetAmount(),  a.GetMaximum());
        tmpProp.SetAmount(tmpProp.GetAmount() - b);
        return tmpProp;
    }

    public static PropertyUsage operator *(PropertyUsage a, PropertyUsage b)
    {
        PropertyUsage tmpProp = new PropertyUsage( a.GetAmount(),  a.GetMaximum());
        tmpProp.SetAmount(tmpProp.GetAmount() * b.GetAmount());
        return tmpProp;
    }

    public static PropertyUsage operator *(PropertyUsage a, int b)
    {
        PropertyUsage tmpProp = new PropertyUsage( a.GetAmount(),  a.GetMaximum());
        tmpProp.SetAmount(tmpProp.GetAmount() * b);
        return tmpProp;
    }

    public static PropertyUsage operator /(PropertyUsage a, PropertyUsage b)
    {
        PropertyUsage tmpProp = new PropertyUsage( a.GetAmount(),  a.GetMaximum());
        tmpProp.SetAmount(tmpProp.GetAmount() - b.GetAmount());
        return tmpProp;
    }

    public static PropertyUsage operator /(PropertyUsage a, int b)
    {
        PropertyUsage tmpProp = new PropertyUsage( a.GetAmount(),  a.GetMaximum());
        tmpProp.SetAmount(tmpProp.GetAmount() - b);
        return tmpProp;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
