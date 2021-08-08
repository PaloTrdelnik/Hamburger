//#define INV_DEBUG

using Godot;
using System;
using System.Collections.Generic;

public class Inventory : Node
{
    public PropertyUsage UsageProp;

    //public int INVENTORY_SIZE = 30;

    public bool bAddedItem = false;
    public bool bRemovedItem = false;

    //private int _numOfItemsInside = 0;
    
    private Dictionary<string, Item> _items = new Dictionary<string, Item>();

    public Item GetItem(Item item)
    {
        return _items[item.InvKey];
    }
    public Item GetItem(string itemInvKey)
    {
        return _items[itemInvKey];
    }
    public void Add(Item item)
    {
        Item itemToAdd = new Item();

        if (item.Amount + UsageProp.GetAmount() > UsageProp.GetMaximum())
        {

            itemToAdd.Amount = UsageProp.GetMaximum() - UsageProp.GetAmount();
            itemToAdd.InvKey = item.InvKey;

            if (itemToAdd.Amount == 0)
            {
                bAddedItem = false;
            }

#if INV_DEBUG || LCH_DEBUG
            GD.Print("INV_DEBUG: " + "|WARNING|" + " Full amount of Item " + itemToAdd.InvKey +
                " does not fit in inventory\n" + Convert.ToString(item.Amount - itemToAdd.Amount) + " throwed");
#endif
        }
        else 
        {
            itemToAdd.Amount = item.Amount;
            itemToAdd.InvKey = item.InvKey;
        }

        if (_items.ContainsKey(item.InvKey))
        {
            _items[itemToAdd.InvKey].Amount += itemToAdd.Amount;
            bAddedItem = true;

#if INV_DEBUG || LCH_DEBUG
            GD.Print("INV_DEBUG: "+"Amount of item " + itemToAdd.InvKey + 
                " has been increased by " + Convert.ToString(itemToAdd.Amount));
#endif
        }
        else
        {
            if (itemToAdd.Amount > 0)
            {
                _items.Add(itemToAdd.InvKey, itemToAdd);
                bAddedItem = true;
            }
            
#if INV_DEBUG || LCH_DEBUG
            GD.Print("INV_DEBUG: " + "Item " + itemToAdd.InvKey + 
                " has been aded with amount: " + Convert.ToString(itemToAdd.Amount));
#endif
        }
        UsageProp += itemToAdd.Amount;
    }

    public void Remove(Item item)
    {
        
        if (_items.ContainsKey(item.InvKey))
        {
            if (_items[item.InvKey].Amount >= item.Amount)
            {
                _items[item.InvKey].Amount -= item.Amount;

#if INV_DEBUG || LCH_DEBUG
                GD.Print("INV_DEBUG: " + "Amount of item " + item.InvKey +
                    " has been decreased by " + Convert.ToString(item.Amount));
#endif

                if (_items[item.InvKey].Amount == 0)
                {
                    _items.Remove(item.InvKey);

#if INV_DEBUG || LCH_DEBUG
                    GD.Print("INV_DEBUG: " + "Item " + item.InvKey +
                        " has been removed from inventory");
#endif
                }
                UsageProp -= item.Amount;
                bRemovedItem = true;
            }
            else
            {
#if INV_DEBUG || LCH_DEBUG
                GD.Print("INV_DEBUG: " + "|WARNING|" + " Amount of item " + item.InvKey +
                    " is lower tan you wnat to remove\n" + Convert.ToString(_items[item.InvKey].Amount) +
                    " in inventory / " + Convert.ToString(item.Amount) + " to be removed");
#endif
                bRemovedItem = false;
            }
        }
        else
        {
#if INV_DEBUG || LCH_DEBUG
            GD.Print("INV_DEBUG: " + "Item: " + item.InvKey + " was not found inventory");
#endif
            bRemovedItem = false;
        }
    }

    public bool IsInInv(Item item)
    {
        if (_items.ContainsKey(item.InvKey))
        {
            if (_items[item.InvKey].Amount >= item.Amount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public bool IsInInv(string itemInvKey, int itemAmount)
    {
        if (_items.ContainsKey(itemInvKey))
        {
            if (_items[itemInvKey].Amount >= itemAmount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public bool ISInvFull()
    {
        return UsageProp.IsUsageMaximal();
        //return false;
    }

    public Inventory()
    {
        UsageProp = new PropertyUsage(0, 30);
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
