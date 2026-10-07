/*
 * Marlow Greenan
 * Created: 10/06/2026
 * Last Updated: 10/06/1026 by Marlow Greenan
 * 
 * 
 */
using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class GeneController : Manager
{
    [SerializeField, OnValueChanged("OnVC_ModelGenotype")]
    private Dictionary<SO_Gene, ActGene> _modelGenotype;

    public override void Initialize()
    {
        base.Initialize();

        InitializeGenotypes();
        ApplyGenotypes();
    }
    
    //Initializes all genotypes.
    private void InitializeGenotypes()
    {
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _modelGenotype)
            kvp.Value.Initialize(kvp.Key, transform);
    }

    //Applies all genotypes (spawns models, sets textures, and switches colors).
    private void ApplyGenotypes()
    {
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _modelGenotype)
            kvp.Value.Apply();
    }
}
//=====================================================================================================================
//ACT GENE
//=====================================================================================================================
[System.Serializable]
public class ActGene
{
    private SO_Gene gene;
    private Transform parent;

    [SerializeField]
    private SO_Allele _allele1;
    [SerializeField]
    private SO_Allele _allele2;

    public void Initialize(SO_Gene g, Transform p)
    {
        gene = g;
        parent = p;
    }

    public void Apply()
    {
        switch (gene.DominanceType)
        {
            case EDominanceType.COMPLETE: ApplyComplete(); break;
            default: Debug.LogError("Dominance type " + gene.DominanceType.ToString() + " is unimplemented."); break;
        }
    }

    //Applies the allele with the most dominance (lowest index).
    private void ApplyComplete()
    {
        GetDominantAllele().ApplyComplete(parent);
    }

    //Returns the allele with the highest dominance.
    private SO_Allele GetDominantAllele()
        => (_allele1.Dominance < _allele2.Dominance)? _allele1 : _allele2;
}

