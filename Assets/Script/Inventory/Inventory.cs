/*
 * Marlow Greenan
 * Created:         11/04/2026
 * Last Updated:    11/04/2025
 */
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private int _slotCount = 100;
    [SerializeField]
    private List<ItemStack> _items;

    [SerializeField, BoxGroup("Simulation")]
    private int _simIndex;
    [SerializeField, BoxGroup("Simulation")]
    private ItemStack _simStack;

    // Adds the stack to _items.
    public void AddItem(ItemStack stack)
    {
        int freeIndex = FindFirstStackWithSpace(stack.Item);
        if (freeIndex == -1) //No stacks match item OR all matching stacks are full.
            _items.Add(new ItemStack(stack)); //Adds new stack.
        else //A stack matches the item and has empty space.
        {
            int remainder = _items[freeIndex].Add(stack.Amount); //Add until amount depleted or stack is full.
            if (remainder > 0)
                AddItem(new ItemStack(stack.Item, remainder)); //If there are items left over from the stack filling up, repeat.
        }
    }

    //Removes the stack from _items.
    public void DropStack(int index)
    {
        if (!InBounds(index)) return;

        _items.RemoveAt(index);
    }

    #region Find
    //Returns the index of the first matching stack in items with space.
    private int FindFirstStackWithSpace(SO_Item item)
    {
        int curIndex = FindFirstStack(item);
        while (InBounds(curIndex) && _items[curIndex].AtCapacity())
            curIndex = FindFirstStack(item, curIndex + 1);

        return curIndex;
    }

    //Returns the index of the first matching stack in items.
    private int FindFirstStack(SO_Item item)
        => FindFirstStack(item, 0);
    private int FindFirstStack(SO_Item item, int startIndex)
    {
        if (!InBounds(startIndex)) return -1;

        for (int i = startIndex; i < _items.Count; i++)
        {
            if (_items[i].Item == item)
                return i;
        }
        return -1;
    }
    #endregion

    private bool InBounds(int i)
        => (i >= 0 && i < _items.Count);

    #region Inspector
    [Button]
    public void AddItem()
        => AddItem(_simStack);

    [Button]
    public void DropStack()
        => DropStack(_simIndex);
    #endregion
}
//=====================================================================================================================
// Act Item
//=====================================================================================================================
[Serializable]
public class ItemStack
{
    [SerializeField, Required]
    private SO_Item _item;
    [SerializeField]
    private int _amount = 1;

    #region GS
    public int Amount 
        {   get => _amount; set => _amount = Mathf.Clamp(value, 0, _item.StackSize); }
    public SO_Item Item { get => _item; }
    #endregion

    public ItemStack(ItemStack stack)
    {
        _item = stack.Item;
        _amount = stack.Amount;
    }
    public ItemStack(SO_Item item, int amount)
    {
        _item = item;
        _amount = amount;
    }

    //Adds the amount. If the new amount is larger than the stack size, returns the remainder.
    public int Add(int a)
    {
        int remainder = (_amount + a) - Item.StackSize;
        Amount += a;
        return (remainder < 0)? 0 : remainder;
    }

    //Returns true if the stack is full.
    public bool AtCapacity()
        => _amount >= _item.StackSize;
}

