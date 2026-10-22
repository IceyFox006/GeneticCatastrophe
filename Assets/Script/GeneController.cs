/*
 * Marlow Greenan
 * Created: 10/06/2026
 * Last Updated: 10/06/1026 by Marlow Greenan
 * 
 * 
 */
using MarUtility;
using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class GeneController : Manager
{
    //GENOTYPES
    [SerializeField, BoxGroup("Genotype")]
    private Dictionary<SO_Gene, ActGene> _modelGenotype;
    private MeshRenderer[] bodyMeshes;
    [SerializeField, ReadOnly]
    private List<Transform> partTransforms = new List<Transform>();

    [SerializeField, BoxGroup("Genotype")]
    private Dictionary<SO_Gene, ActGene> _textureGenotype;

    [SerializeField, BoxGroup("Genotype")]
    private Dictionary<SO_Gene, ActGene> _colorGenotype;

    //VISUAL
    [SerializeField, BoxGroup("Visual"), Label("Genetics Material")]
    private Material _mat;

    #region GS
    public MeshRenderer[] BodyMeshes { get => bodyMeshes; }
    public List<Transform> PartTransforms { get => partTransforms; }
    #endregion

    public override void Initialize()
    {
        base.Initialize();

        InitializeAndApplyGenotypes();
    }
    
    //Initializes and applies all genotypes.
    private void InitializeAndApplyGenotypes()
    {
        //MODEL
        bodyMeshes = GetComponentsInChildren<MeshRenderer>();
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _modelGenotype)
        {
            kvp.Value.Initialize(kvp.Key, this);
            MarData.CombineTransformLists(ref partTransforms, kvp.Value.Apply());
        }

        //TEXTURE
        //Sets the material of every renderer that is not already a variation of the _mat shader to _mat.
        foreach (MeshRenderer mr in transform.GetComponentsInChildren<MeshRenderer>())
        {
            if (mr.material.shader != _mat.shader)
                mr.material = _mat;
        }
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _textureGenotype)
        {
            kvp.Value.Initialize(kvp.Key, this);
            kvp.Value.Apply();
        }

        //COLOR
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _colorGenotype)
        {
            kvp.Value.Initialize(kvp.Key, this);
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
    private GeneController gc;

    //ALLELES
    [SerializeField]
    private SO_Allele _allele1;
    [SerializeField]
    private SO_Allele _allele2;

    public void Initialize(SO_Gene g, GeneController gc)
    {
        gene = g;
        this.gc = gc;
    }

    public List<Transform> Apply()
    {
        List<Transform> affected = new List<Transform>();
        switch (gene.DominanceType)
        {
            case EDominanceType.COMPLETE: ApplyComplete(out affected); break;
            case EDominanceType.INCOMPLETE: ApplyIncomplete(); break;
            default: Debug.LogError("Dominance type " + gene.DominanceType.ToString() + " is unimplemented."); break;
        }
        return affected;
    }

    //Applies the allele with the most dominance (lowest index).
    private void ApplyComplete(out List<Transform> affected)
    {
        GetDominantAllele().ApplyComplete(gc, out affected);
    }

    //Applies both alleles.
    private void ApplyIncomplete()
    {
        _allele1.ApplyIncomplete(gc, 0);
        _allele2.ApplyIncomplete(gc, 1);
    }

    //Returns the allele with the highest dominance.
    private SO_Allele GetDominantAllele()
        => (gene.FindAllele(_allele1) < gene.FindAllele(_allele2))? _allele1 : _allele2;
}

