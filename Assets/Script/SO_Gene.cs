/*
 * Marlow Greenan
 * Created: 10/06/2026
 * Last Updated: 10/06/1026 by Marlow Greenan
 * 
 * Contains data for a gene.
 */
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Gene_", menuName = "Scriptable Objects/Genetics/Gene")]
public class SO_Gene : ScriptableObject
{
    [SerializeField]
    private string _name;

    [SerializeField, ResizableTextArea]
    private string _description;

    [SerializeField]
    private EDominanceType _dominanceType;

    [SerializeField, OnValueChanged("OnVC_Alleles")]
    private SO_Allele[] _alleles;

    #region GS
    public EDominanceType DominanceType { get => _dominanceType; }
    #endregion

    #region Inspector
    //Updates the dominance of the alleles based on their array index. Lower index = higher dominance.
    private void OnVC_Alleles()
    {
        for (int i = 0; i < _alleles.Length; i++)
        {
            if (_alleles[i] == null) continue; //Null check.

            _alleles[i].DominanceType = _dominanceType;
            _alleles[i].Dominance = i;
        }
    }
    #endregion
}

public enum EDominanceType
{
    COMPLETE = 000,
    INCOMPLETE = 100,
    CODOMINANT = 200
}
