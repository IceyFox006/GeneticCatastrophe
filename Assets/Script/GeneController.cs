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
    //GENOTYPES
    [SerializeField, BoxGroup("Genotype")]
    private Dictionary<SO_Gene, ActGene> _modelGenotype;

    [SerializeField, BoxGroup("Genotype")]
    private Dictionary<SO_Gene, ActGene> _textureGenotype;

    [SerializeField, BoxGroup("Genotype")]
    private Dictionary<SO_Gene, ActGene> _colorGenotype;

    //VISUAL
    [SerializeField, BoxGroup("Visual"), Label("Genetics Material")]
    private Material _mat;

    public override void Initialize()
    {
        base.Initialize();

        InitializeAndApplyGenotypes();
    }
    
    //Initializes and applies all genotypes.
    private void InitializeAndApplyGenotypes()
    {
        //Model
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _modelGenotype)
        {
            kvp.Value.Initialize(kvp.Key, transform);
            kvp.Value.Apply();
        }

        //Texture
        foreach (MeshRenderer mr in transform.GetComponentsInChildren<MeshRenderer>())
            mr.material = _mat;
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _textureGenotype)
        {
            kvp.Value.Initialize(kvp.Key, transform);
            kvp.Value.Apply();
        }

        //Color
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _colorGenotype)
        {
            kvp.Value.Initialize(kvp.Key, transform);
            kvp.Value.Apply();
        }
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

    //ALLELES
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
            case EDominanceType.INCOMPLETE: ApplyIncomplete(); break;
            default: Debug.LogError("Dominance type " + gene.DominanceType.ToString() + " is unimplemented."); break;
        }
    }

    //Applies the allele with the most dominance (lowest index).
    private void ApplyComplete()
    {
        GetDominantAllele().ApplyComplete(parent);
    }

    //Applies both alleles.
    private void ApplyIncomplete()
    {
        _allele1.ApplyIncomplete(parent, 0);
        _allele2.ApplyIncomplete(parent, 1);
    }

    //Returns the allele with the highest dominance.
    private SO_Allele GetDominantAllele()
        => (_allele1.Dominance < _allele2.Dominance)? _allele1 : _allele2;
}

