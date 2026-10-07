/*
 * Marlow Greenan
 * Created: 10/06/2026
 * Last Updated: 10/06/1026 by Marlow Greenan
 * 
 * Contains data for a model allele.
 */
using MarUtility;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoAllele_", menuName = "Scriptable Objects/Genetics/Allele/Model")]
public class SO_Allele_Model : SO_Allele
{
    [SerializeField, BoxGroup("Model"), Tooltip("Key is the connection point name (the game object the model will spawn as a child of).")]
    private Dictionary<string, GameObject> _moData;

    //Spawn models under their connection points.
    public override void ApplyComplete(Transform p)
    {
        Transform parent;
        foreach (KeyValuePair<string, GameObject> kvp in _moData)
        {
            parent = MarData.FindChildWithName(p, kvp.Key); //Find connection point.

            if (parent == null) //Null catch.
            {
                Debug.LogError("Cannot apply model allele " + _name + ".\n" + p.name + " does not have a connection point named " + kvp.Key + ".");
                continue;
            }

            Instantiate(kvp.Value, parent); //Spawn model as a child of the connection point.
        }
    }
}
