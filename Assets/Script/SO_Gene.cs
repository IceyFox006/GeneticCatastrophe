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

    [SerializeField]
    private SO_Allele[] _alleles;

    #region GS
    public EDominanceType DominanceType { get => _dominanceType; }
    #endregion

    //Returns the index of the allele.
    public int FindAllele(SO_Allele a)
    {
        for (int i = 0; i < _alleles.Length; i++)
        {
            if (_alleles[i] == a)
                return i;
        }
        return -1;
    }
}

public enum EDominanceType
{
    COMPLETE = 000,
    INCOMPLETE = 100,
    CODOMINANT = 200
}
