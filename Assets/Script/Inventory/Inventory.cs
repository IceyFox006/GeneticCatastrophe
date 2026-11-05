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
    private List<ActItem> _items;

    [SerializeField, BoxGroup("Simulation")]
    private ActItem _simItem;

    public void AddItem(ActItem item)
    {
        int freeIndex = FindFirstStackWithSpace(item.Item);
        if (freeIndex == -1) //No stacks match item OR all matching stacks are full.
            _items.Add(new ActItem(item));
        else
        {
            int remainder = _items[freeIndex].Add(item.Amount);
            Debug.Log(remainder);
            if (remainder > 0)
                AddItem(new ActItem(item.Item, remainder));
        }
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
        => AddItem(_simItem);
    #endregion
}
//=====================================================================================================================
// Act Item
//=====================================================================================================================
[Serializable]
public class ActItem
{
    [SerializeField, Required]
    private SO_Item _item;
    [SerializeField]
    private int _amount = 1;

    #region GS
    public int Amount 
        {   get => _amount; 
            set 
            { 
                _amount = Mathf.Clamp(value, 0, _item.StackSize);
                //Mathf.Clamp(_amount, 0, _item.StackSize);
            } 
        }
    public SO_Item Item { get => _item; }
    #endregion

    public ActItem(ActItem ai)
    {
        _item = ai.Item;
        _amount = ai.Amount;
    }
    public ActItem(SO_Item item, int amount)
    {
        _item = item;
        _amount = amount;
    }

    public int Add(int a)
    {
        int remainder = (_amount + a) - Item.StackSize;
        Amount += a;
        return (remainder < 0)? 0 : remainder;
    }

    public bool AtCapacity()
        => _amount >= _item.StackSize;
}

