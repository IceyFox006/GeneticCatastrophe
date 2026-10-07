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
    private string _colorID1 = "_Color_";
    [SerializeField, BoxGroup("Color"), ShowIf("dominanceType", EDominanceType.INCOMPLETE)]
    private string _colorID2 = "_Color_";

    public override void ApplyIncomplete(Transform p ,int aNum)
    {
        switch (aNum)
        {
            case 1: ApplyColor(p, _colorID1); break;
            case 2: ApplyColor(p, _colorID2); break;
            default: Debug.LogError("Allele numer " + aNum + " is out of range."); return;
        }
    }

    private void ApplyColor(Transform p, string id)
    {
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
