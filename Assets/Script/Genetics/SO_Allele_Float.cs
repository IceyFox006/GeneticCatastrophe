/*
 * Marlow Greenan
 * Created: 10/22/2026
 * Last Updated: 10/22/1026 by Marlow Greenan
 * 
 * Contains data for a float allele.
 */
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatAllele_", menuName = "Scriptable Objects/Genetics/Allele/Float")]
public class SO_Allele_Float : SO_Allele
{
    [SerializeField, BoxGroup("General")]
    private float _floatData;

    #region GS
    public float FloatData { get => _floatData; }
    #endregion
}
