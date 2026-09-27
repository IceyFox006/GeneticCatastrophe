using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gene_", menuName = "Scriptable Objects/Genetics/Gene")]
public class SO_Gene : ScriptableObject
{
    //GENERAL
    [SerializeField, BoxGroup("General")]
    protected string _name;

    [SerializeField, BoxGroup("General"), ResizableTextArea]
    protected string _description;

    [SerializeField, ReadOnly]
    private EGeneType geneType;

    //GENETICS
    [SerializeField, BoxGroup("Genetics")]
    protected EDominanceType dominanceType;

    [SerializeField, BoxGroup("Genetics"), OnValueChanged("OnVC_Alleles"), Tooltip("Index determines dominance. The smaller the index, the higher the dominance.\nEx. 1 has more dominance than 5.")]
    protected SO_Allele[] _alleles;

    #region GS
    public EDominanceType DominanceType { get => dominanceType; }
    public string Name { get => _name; }
    public EGeneType GeneType { get => geneType; }
    #endregion

    //Returns the index(dominance) of the allele given. If the allele is not in the gene, returns -1.
    public int GetDominance(SO_Allele allele)
    {
        for (int i = 0; i < _alleles.Length; i++)
            if (_alleles[i] == allele) return i;
        return -1;
    }

    //Return the allele with the most dominance.
    public SO_Allele GetDominantAllele(List<SO_Allele> alleles)
    {
        int dI = 0;
        for (int i = 1; i < alleles.Count; i++)
        {
            if (GetDominance(alleles[i]) < GetDominance(alleles[dI]))
                dI = i;
        }
        return alleles[dI];
    }

    #region Inspector
    private void OnVC_Alleles()
    {
        if (_alleles.Length > 0)
        {
            if (_alleles[0] != null)
                geneType = _alleles[0].GeneType;
        }
    }
    #endregion
}
#region Enums
public enum EGeneType
{
    NONE = 000,
    MODEL = 100,
    TEXTURE = 200,
    COLOR = 300
}

public enum EDominanceType
{
    NONE = 000,
    COMPLETE = 100,
    INCOMPLETE = 200,
    CODOMINANCE = 300,
}
#endregion
