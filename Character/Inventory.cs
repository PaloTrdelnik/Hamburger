//#define INV_DEBUG

using Godot;
using System;
using System.Collections.Generic;

public class Inventory : Node
{
    [Export]
    public int INVENTORY_SIZE = 20;

    private int _numOfItemsInside = 0;
    
    private Dictionary<string, Item> _items = new Dictionary<string, Item>();

    public void Add(Item item)
    {
        Item itemToAdd = new Item();

        if (item.Amount + _numOfItemsInside > INVENTORY_SIZE)
        {
            
            itemToAdd.Amount = INVENTORY_SIZE - _numOfItemsInside;
            itemToAdd.Name = item.Name;
#if INV_DEBUG || LCH_DEBUG
            GD.Print("INV_DEBUG: " + "|WARNING|" + " Full amount of Item " + itemToAdd.Name + 
                " does not fit in inventory\n" + Convert.ToString(item.Amount - itemToAdd.Amount) + " throwed");
#endif
        }
        else 
        {
            itemToAdd.Amount = item.Amount;
            itemToAdd.Name = item.Name;
        }

        if (_items.ContainsKey(item.Name))
        {
            _items[item.Name].Amount += itemToAdd.Amount;

#if INV_DEBUG || LCH_DEBUG
            GD.Print("INV_DEBUG: "+"Amount of item " + itemToAdd.Name + 
                " has been increased by " + Convert.ToString(itemToAdd.Amount));
#endif
        }
        else
        {
            _items.Add(item.Name, itemToAdd);

#if INV_DEBUG || LCH_DEBUG
            GD.Print("INV_DEBUG: " + "Item " + itemToAdd.Name + 
                " has been aded with amount: " + Convert.ToString(itemToAdd.Amount));
#endif
        }
    }

    public void Remove(Item item)
    {
        
        if (_items.ContainsKey(item.Name))
        {
            if (_items[item.Name].Amount >= item.Amount)
            {
                _items[item.Name].Amount -= item.Amount;

#if INV_DEBUG || LCH_DEBUG
                GD.Print("INV_DEBUG: " + "Amount of item " + item.Name + 
                    " has been decreased by " + Convert.ToString(item.Amount));
#endif

                if (_items[item.Name].Amount == 0)
                {
                    _items.Remove(item.Name);

#if INV_DEBUG || LCH_DEBUG
                    GD.Print("INV_DEBUG: " + "Item " + item.Name + 
                        " has been removed from inventory");
#endif
                }
            }
            else
            {
#if INV_DEBUG || LCH_DEBUG
                GD.Print("INV_DEBUG: " + "|WARNING|" + " Amount of item " + item.Name +
                    " is lower tan you wnat to remove\n" + Convert.ToString(_items[item.Name].Amount) + 
                    " in inventory / " + Convert.ToString(item.Amount) + " to be removed" );
#endif
            }
        }
        else
        {
#if INV_DEBUG || LCH_DEBUG
            GD.Print("INV_DEBUG: " + "Item: " + item.Name + " was not found inventory");
#endif
        }
    }

    public bool IsInInv(Item item)
    {
        if (_items.ContainsKey(item.Name))
        {
            if (_items[item.Name].Amount >= item.Amount)
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

    public bool IsInInv(string itemName, int itemAmount)
    {
        if (_items.ContainsKey(itemName))
        {
            if (_items[itemName].Amount >= itemAmount)
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
