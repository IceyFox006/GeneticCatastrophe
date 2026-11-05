/*
 * Marlow Greenan
 * Created: 10/27/2026
 * Last Updated: 10/27/2026 by Marlow Greenan
 * 
 * Contains data relevant to all items.
 */
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Scriptable Objects/Item/Item")]
public class SO_Item : ScriptableObject
{
    [SerializeField, BoxGroup("General")]
    private string _name;
    [SerializeField, BoxGroup("General")]
    private string _description;
    [SerializeField, BoxGroup("General"), Tooltip("How many items can be contained in a single stack.")]
    private int _stackSize;

    #region GS
    public int StackSize { get => _stackSize; }
    public string Name { get => _name; }
    #endregion
}
