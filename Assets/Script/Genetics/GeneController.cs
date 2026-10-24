/*
 * Marlow Greenan
 * Created: 10/06/2026
 * Last Updated: 10/24/1026 by Marlow Greenan
 * 
 * 
 */
using MarUtility;
using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

//=================================================================================================================
// GENE CONTROLLER
//=================================================================================================================
public class GeneController : Manager
{
    //GENOTYPES
    //Model
    [SerializeField, BoxGroup("Genotype")]
    private Dictionary<SO_Gene, ActGene> _modelGenotype;
    private MeshRenderer[] bodyMeshes;
    [SerializeField, ReadOnly]
    private List<Transform> partTransforms = new List<Transform>();

    //Texture
    [SerializeField, BoxGroup("Genotype")]
    private Dictionary<SO_Gene, ActGene> _textureGenotype;
    //Piebald
    [SerializeField, BoxGroup("Genotype")]
    private PiebaldGenotype _piebaldGenotype;

    //Color
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

        InitializeGenotypes();
        ApplyGenotypes();
    }

    //Initializes Genotypes
    private void InitializeGenotypes()
    {
        //Model
        bodyMeshes = GetComponentsInChildren<MeshRenderer>();
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _modelGenotype)
            kvp.Value.Initialize(kvp.Key, this);

        //Texture
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _textureGenotype)
            kvp.Value.Initialize(kvp.Key, this);
        _piebaldGenotype.Initialize(this);

        //Color
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _colorGenotype)
            kvp.Value.Initialize(kvp.Key, this);
    }

    //Applies genotypes.
    private void ApplyGenotypes()
    {
        //MODEL
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _modelGenotype)
            MarData.CombineTransformLists(ref partTransforms, kvp.Value.Apply());

        //TEXTURE
        //Sets the material of every renderer that is not already a variation of the _mat shader to _mat.
        foreach (MeshRenderer mr in transform.GetComponentsInChildren<MeshRenderer>())
        {
            if (mr.material.shader != _mat.shader)
                mr.material = _mat;
        }
        //Initializes default texture genes.
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _textureGenotype)
            kvp.Value.Apply();
        //Piebald
        _piebaldGenotype.Apply();

        //COLOR
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _colorGenotype)
            kvp.Value.Apply();
    }

    private void EraseGenotype()
    {
        //Destroy parts that were spawned in from the previous genotype.
        for (int i = 0; i < partTransforms.Count; i++)
            Destroy(partTransforms[i].gameObject);
        partTransforms.Clear();

        //Reset colors to default.
        foreach (MeshRenderer mr in bodyMeshes)
            mr.material = _mat;
    }
    #region Inspector
    //Randomizes all alleles.
    [Button]
    private void RandomizeGenotype()
    {
        EraseGenotype();

        //Model
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _modelGenotype)
            kvp.Value.Randomize();

        //Texture
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _textureGenotype)
            kvp.Value.Randomize();
        _piebaldGenotype.Randomize();

        //Color
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _colorGenotype)
            kvp.Value.Randomize();

        ApplyGenotypes();
    }
    #endregion

    //=================================================================================================================
    // PIEBALD GENOTYPE
    //=================================================================================================================
    [System.Serializable]
    private class PiebaldGenotype
    {
        private GeneController gc;

        //Piebald
        [AllowNesting, SerializeField, BoxGroup("Piebald")]
        private SO_Gene _piebaldGene;
        [AllowNesting, SerializeField, BoxGroup("Piebald")]
        private SO_Allele_Float _piebaldAllele1;
        [AllowNesting, SerializeField, BoxGroup("Piebald")]
        private SO_Allele_Float _piebaldAllele2;

        //Spot Density
        [AllowNesting, SerializeField, BoxGroup("Spot Density")]
        private SO_Gene _spotDensityGene;
        [AllowNesting, SerializeField, BoxGroup("Spot Density")]
        private SO_Allele_Float _spotDensityAllele1;
        [AllowNesting, SerializeField, BoxGroup("Spot Density")]
        private SO_Allele_Float _spotDensityAllele2;

        //Patterns
        [SerializeField]
        private SO_Allele_Float _noSpotsAllele;
        [SerializeField]
        private SO_Allele_Texture[] _gradeAlleles;

        public void Initialize(GeneController gc)
            => this.gc = gc;

        public void Apply()
        {
            //No spots phenotype (ss).
            if (_piebaldAllele1 == _noSpotsAllele && _piebaldAllele2 == _noSpotsAllele)
                return;

            //Piebald1 + Piebald2 + SpotDensity1 + SpotDensity2 + Random(0 - 2)
            float densityGrade = _piebaldAllele1.FloatData + _piebaldAllele2.FloatData + _spotDensityAllele1.FloatData + _spotDensityAllele2.FloatData + Random.Range(0, 3);
            int gradeIndex = Mathf.Clamp((int)(densityGrade - 1), 0, 9);
           
            _gradeAlleles[gradeIndex].ApplyComplete(gc);
        }

        public void Randomize()
        {
            _piebaldAllele1 = (SO_Allele_Float)_piebaldGene.RandomAllele();
            _piebaldAllele2 = (SO_Allele_Float)_piebaldGene.RandomAllele();

            _spotDensityAllele1 = (SO_Allele_Float)_spotDensityGene.RandomAllele();
            _spotDensityAllele2 = (SO_Allele_Float)_spotDensityGene.RandomAllele();
        }
    }
}
//=====================================================================================================================
// ACT GENE
//=====================================================================================================================
[System.Serializable]
public class ActGene
{
    [SerializeField]
    private bool _autoInitialize = true;

    [AllowNesting, SerializeField, HideIf("_autoInitialize")]
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

    #region Application
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
    #endregion

    //Randomizes both alleles.
    public void Randomize()
    {
        _allele1 = gene.RandomAllele();
        _allele2 = gene.RandomAllele();
    }

    //Returns the allele with the highest dominance.
    private SO_Allele GetDominantAllele()
        => (gene.FindAllele(_allele1) < gene.FindAllele(_allele2))? _allele1 : _allele2;
}
