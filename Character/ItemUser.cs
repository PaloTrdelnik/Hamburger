using Godot;
using System;
using System.Collections.Generic;

public class ItemUser : Node
{
    [Signal]
    public delegate void SItemUseStart(string ItemKey);

    [Signal]
    public delegate void SItemUseEnd(string ItemKey);

    public int MaxItemTypesInUse = 1;

    private int _itemsInUse = 0;
    private Dictionary<string, Item> _items = new Dictionary<string, Item>();
    private Player _player;

    public Item GetItem(string itemKey)
    {
        return _items[itemKey];
    }

    public bool UseItem(string itemKey)
    {
        Item itemToUse = new Item();
        itemToUse.InvKey = itemKey;

        if (_items.ContainsKey(itemKey))
        {
            //GD.Print("ContainItem");

        }
        else
        {
            if (_player.Inv.IsInInv(itemKey, 1))
            {
                Item rem = new Item { InvKey = itemKey, Amount = 1 };

                //GD.Print("UseItem");

                _player.Inv.Remove(rem);
                _items.Add(itemToUse.InvKey, itemToUse);

                if (_itemsInUse < MaxItemTypesInUse)
                {
                    Item item = _items[itemKey];

                    item.Connect("SItemUsed", this, nameof(OnSItemUsed));
                    item.UseDurabilityTimer.WaitTime = _player.ItemProp.TimeDilation_UseDurabilityTime;

                    AddChild(item.UseDurabilityTimer);
                    item.UseDurabilityTimer.Start();

                    EmitSignal(nameof(SItemUseStart), itemKey);

                    _itemsInUse++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public void ResetItemUse()
    {
        //remove all runing timers
        foreach (var child in GetChildren())
        {
            if (child is Timer)
            {
                Timer tChild = (Timer)child;

                //GD.Print(tChild.Name);

                tChild.Stop();
                tChild.WaitTime = 0.000000001f;
                //GD.Print(tChild.WaitTime);
                tChild.Start();
                //tChild.QueueFree();
            }
        }
        //reset number of items in use and clear queued items
        _itemsInUse = 0;
        //_items.Clear();
    }

    public bool IsItemUsed(string itemKey)
    {
        if (_items.ContainsKey(itemKey))
        {
            return GetItem(itemKey).UseDurabilityTimer.IsStopped();
        }
        return false;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _player = (Player)GetParent();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void OnSItemUsed(string itemKey)
    {
        //GD.Print("Remove");

        if (_items[itemKey].CanUseSimultaneously)
        {

        }
        else
        {
            _items.Remove(itemKey);
            _itemsInUse--;
            EmitSignal(nameof(SItemUseEnd), itemKey);
        }
    }

}
