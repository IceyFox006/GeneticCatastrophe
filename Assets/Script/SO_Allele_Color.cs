using MarUtility;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorAllele_", menuName = "Scriptable Objects/Genetics/Allele/Color")]
public class SO_Allele_Color : SO_Allele
{
    [SerializeField, BoxGroup("General"), Tooltip("Key is the color being replaced.")]
    private Dictionary<SO_Color, SO_Color> _colorData;

    //COLOR
    [SerializeField, BoxGroup("Color")]
    private string[] _colorIDs;

    //Applies the allele to both colorID regions.
    public override void ApplyComplete(Transform p)
    {
        foreach (string id in _colorIDs)
            ApplyColor(p, id);
    }

    //Applies the allele to only one colorID region.
    public override void ApplyIncomplete(Transform p ,int aNum)
    {
        if (!(aNum > -1 && aNum < _colorIDs.Length))
        {
            Debug.LogError("Allele number " + aNum + " is out of range.");
            return;
        }

        ApplyColor(p, _colorIDs[aNum]);
    }

    //Replaces colors on the material with the new allele color.
    private void ApplyColor(Transform p, string id)
    {
        if (_colorData.Count <= 0) return;

        foreach (MeshRenderer mr in p.GetComponentsInChildren<MeshRenderer>())
        {
            if (!mr.material.HasColor(id)) continue;
            
            foreach (KeyValuePair<SO_Color, SO_Color> kvp in _colorData)
            {
                if (kvp.Key.Color == mr.material.GetColor(id))
                {
                    mr.material.SetColor(id, kvp.Value.Color);
                    break;
                }
            }
        }
    }
}
