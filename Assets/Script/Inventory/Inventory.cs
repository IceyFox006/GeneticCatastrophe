/*
 * Marlow Greenan
 * Created:         11/04/2026
 * Last Updated:    11/11/2025
 */
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //INVENTORY
    [SerializeField]
    private int _slotCount = 100;
    [SerializeField]
    private List<ItemStack> _items;

    //SIMULATION
    [SerializeField, BoxGroup("Simulation")]
    private int _simIndex;
    [SerializeField, BoxGroup("Simulation")]
    private int _simAmount;
    [SerializeField, BoxGroup("Simulation")]
    private ItemStack _simStack;
    [SerializeField, BoxGroup("Simulation"), OnValueChanged("OnVC_SimInventory")]
    private Inventory _simInventory;


    #region GS
    public List<ItemStack> Items { get => _items; }
    #endregion

    #region Add
    // Adds the stack to _items.
    public int AddToStack(ItemStack stack)
    {
        int freeIndex = FindFirstStackWithSpace(stack.Item);
        if (freeIndex == -1) //No stacks match item OR all matching stacks are full.
            return AddStack(stack);  //Adds new stack.
        else //A stack matches the item and has empty space.
        {
            int remainder = _items[freeIndex].Add(stack.Amount); //Add until amount depleted or stack is full.
            if (remainder > 0)
                return AddToStack(new ItemStack(stack.Item, remainder)); //If there are items left over from the stack filling up, repeat.
            return remainder;
        }
    }

    //Adds a new stack to _items if there is space.
    public int AddStack(ItemStack stack)
    {
        if (!AtStackCapacity())
        {
            _items.Add(new ItemStack(stack));
            return 0;
        }
        else
        {
            Debug.Log($"{gameObject.name}'s inventory is full.{stack.Item.name} x{stack.Amount} could not be added.");
            return stack.Amount;
        }
    }
    #endregion

    #region Remove
    //Removes amount from the item stack at index.
    public void RemoveFromStack(int index, int amount)
    {
        if (!InBounds(index)) return;
        if (_items[index].Amount < amount) return;

        _items[index].Amount -= amount;
        if (_items[index].Amount <= 0)
            DropStack(index);
    }

    //Removes the stack from _items.
    public void DropStack(int index)
    {
        if (!InBounds(index)) return;

        _items.RemoveAt(index);
    }

    //Removes all stacks in _items.
    public void DropInventory()
        => _items.Clear();
    #endregion

    #region Transfer
    //Transfers the whole stack to this inventory.
    public void TransferStack(Inventory inv, int index)
        => TransferStack(inv, index, inv.Items[index].Amount);
    //Transfers the amount of the stack to this inventory.
    public void TransferStack(Inventory inv, int index, int amount)
    {
        if (!inv.InBounds(index)) return;

        int remainder = AddToStack(new ItemStack(inv.Items[index].Item, amount));
        inv.RemoveFromStack(index, amount - remainder);
    }

    //Transfers all items to this inventory.
    public void TransferInventory(Inventory inv)
    {
        for (int i = inv.Items.Count - 1; i >= 0; i--)
            TransferStack(inv, i);
    }
    #endregion

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
    private bool AtStackCapacity()
        => _items.Count >= _slotCount;

    #region Inspector
    //BUTTONS
    [Button("Add To Stack")]
    private void SimAddToStack()
        => AddToStack(_simStack);
    [Button("Remove From Stack")]
    private void SimRemoveFromStack()
        => RemoveFromStack(_simIndex, _simAmount);

    [Button("Drop Stack")]
    private void SimDropStack()
        => DropStack(_simIndex);

    [Button("Drop Inventory")]
    private void SimDropInventory()
        => DropInventory();

    [Button("Transfer Stack")]
    private void SimTransferStack()
        => TransferStack(_simInventory, _simIndex, _simAmount);

    [Button("Transfer Inventory")]
    private void SimTransferInventory()
        => TransferInventory(_simInventory);

    //ON VALUE CHANGED
    private void OnVC_SimInventory()
    {
        if (_simInventory == this)
        {
            _simInventory = null;
            Debug.LogError($"Sim inventory {gameObject.name} cannot be set to itself.\nChoose a different inventory.");
        }
    }
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

