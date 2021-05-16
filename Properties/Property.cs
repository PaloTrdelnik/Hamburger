using Godot;
using System;

public class Property : Node
{
    private int _amount = 0;
    private int _record = 0;
    public bool IsNewRecord = false;

    public void SetAmount(int amount)
    {
        _amount = amount;

        if (_record < amount)
        {
            IsNewRecord = true;
            _record = amount;
        }
        else
        {
            IsNewRecord = false;
        }
    }

    public int GetAmount()
    {
        return _amount;
    }

    public int GetRecord()
    {
        return _record;
    }

    public void SetRecord(int record)
    {
        _record = record;
    }

    //Operator overloads
    
    public static Property operator +(Property a, Property b)
    {
        Property tmpProp = new Property { _amount = a.GetAmount(), _record = a.GetRecord(), IsNewRecord = a.IsNewRecord } ;
        tmpProp.SetAmount(tmpProp.GetAmount() + b.GetAmount());
        return tmpProp;
    }

    public static Property operator +(Property a, int b)
    {
        Property tmpProp = new Property { _amount = a.GetAmount(), _record = a.GetRecord(), IsNewRecord = a.IsNewRecord };
        tmpProp.SetAmount(tmpProp.GetAmount() + b);
        return tmpProp;
    }

    public static Property operator -(Property a, Property b)
    {
        Property tmpProp = new Property { _amount = a.GetAmount(), _record = a.GetRecord(), IsNewRecord = a.IsNewRecord };
        tmpProp.SetAmount(tmpProp.GetAmount() - b.GetAmount());
        return tmpProp;
    }

    public static Property operator -(Property a, int b)
    {
        Property tmpProp = new Property { _amount = a.GetAmount(), _record = a.GetRecord(), IsNewRecord = a.IsNewRecord };
        tmpProp.SetAmount(tmpProp.GetAmount() - b);
        return tmpProp;
    }

    public static Property operator *(Property a, Property b)
    {
        Property tmpProp = new Property { _amount = a.GetAmount(), _record = a.GetRecord(), IsNewRecord = a.IsNewRecord };
        tmpProp.SetAmount(tmpProp.GetAmount() * b.GetAmount());
        return tmpProp;
    }

    public static Property operator *(Property a, int b)
    {
        Property tmpProp = new Property { _amount = a.GetAmount(), _record = a.GetRecord(), IsNewRecord = a.IsNewRecord };
        tmpProp.SetAmount(tmpProp.GetAmount() * b);
        return tmpProp;
    }

    public static Property operator /(Property a, Property b)
    {
        Property tmpProp = new Property { _amount = a.GetAmount(), _record = a.GetRecord(), IsNewRecord = a.IsNewRecord };
        tmpProp.SetAmount(tmpProp.GetAmount() - b.GetAmount());
        return tmpProp;
    }

    public static Property operator /(Property a, int b)
    {
        Property tmpProp = new Property { _amount = a.GetAmount(), _record = a.GetRecord(), IsNewRecord = a.IsNewRecord };
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
