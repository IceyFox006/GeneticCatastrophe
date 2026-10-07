/*
 * Marlow Greenan
 * Created: 10/07/2026
 * Last Updated: 10/07/1026 by Marlow Greenan
 * 
 * Contains data for a reused color.
 */
using UnityEngine;

[CreateAssetMenu(fileName = "Color_", menuName = "Scriptable Objects/Color")]
public class SO_Color : ScriptableObject
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private Color _color = Color.white;

    #region GS
    public Color Color { get => _color; }
    #endregion
}
