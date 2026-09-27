using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeneController : Manager
{
    [SerializeField]
    private Dictionary<SO_Gene, ActGene> _genotype;
    [SerializeField, ReadOnly]
    private Dictionary<SO_Gene, List<SO_Allele>> phenotype;

    public override void Initialize()
    {
        InitializeGenotype();
        InitializePhenotype();
        ApplyPhenotype();

        base.Initialize();
    }

    private void InitializeGenotype()
    {
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _genotype)
            kvp.Value.Initialize(transform);
    }

    #region Phenotype
    //Creates the phenotype. Must have initialized the genotype beforehand.
    private void InitializePhenotype()
    {
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _genotype) //Iterate through the genotype.
        {
            phenotype[kvp.Key] = new List<SO_Allele>();
            switch (kvp.Key.DominanceType)
            {
                case EDominanceType.COMPLETE: AddCompleteGeneToPhenotype(kvp); break;
                case EDominanceType.INCOMPLETE: break;
                case EDominanceType.CODOMINANCE: break;
                default: Debug.LogError("FAILED TO INITIALIZE.\n" + kvp.Key.Name + " does not have a dominance type."); break;
            }
            
        }
    }

    //Applies phenotype genes to the model.
    private void ApplyPhenotype()
    {
        foreach (KeyValuePair<SO_Gene, List<SO_Allele>> kvp in phenotype)
        {
            foreach (SO_Allele allele in kvp.Value)
                allele.ApplyToPhenotype(transform, _genotype[kvp.Key]);
        }
    }

    //Adds the single most dominant allele to the phenotype.
    private void AddCompleteGeneToPhenotype(KeyValuePair<SO_Gene, ActGene> kvp)
    {
        SO_Allele dominantAllele = kvp.Key.GetDominantAllele(kvp.Value.Alleles);
        phenotype[kvp.Key].Add(dominantAllele);
    }
    #endregion
}

//ACT GENES
//=====================================================================================================================
[System.Serializable]
public class ActGene
{
    [SerializeField]
    private EGeneType _type;

    [SerializeField]
    private List<SO_Allele> _alleles = new List<SO_Allele>();

    //MODEL
    private Transform parent;
    [SerializeField, AllowNesting, ShowIf("_type", EGeneType.MODEL), Tooltip("CASE SENSITIVE. The name of the gameObjects the model will be childed to.")]
    private string[] _connectPointID;
    private List<Transform> connectionPoints;


    #region GS
    public List<SO_Allele> Alleles { get => _alleles; set => _alleles = value; }
    public string[] ConnectPointID { get => _connectPointID; }
    #endregion

    public void Initialize(Transform p)
    {
        parent = p;
        AssignConnectionPoints();
    }

    /*  
     *  Searches through the children transforms until it finds one with the same names as one of the connection points.
     *  Once it finds a match, add it to the connection point list then continue on to find a match for the next id.
     */ 
    private void AssignConnectionPoints()
    {
        for (int cpID = 0; cpID < _connectPointID.Length; cpID++)
        {
            for (int c = 0; c < parent.childCount; c++)
            {
                Debug.Log(parent.GetChild(c).name);
                if (parent.GetChild(c).name.Equals(_connectPointID[cpID])) //Found connection point.
                {
                    connectionPoints.Add(parent.GetChild(c));
                    break;
                }

                if (c == parent.childCount - 1) //Failed to find a connection point.
                    Debug.LogError("Failed to find connection point \"" + _connectPointID[cpID] + "\".");
            }
        }
    }

    #region Inspector
    private void OnVC_Alleles()
    {
        Debug.Log("h");
        if (_alleles.Count > 2)
        {
            List<SO_Allele> replacement = new List<SO_Allele>();
            for (int i = 0; i < 2; i++)
                replacement[i] = _alleles[i];
            _alleles = replacement;
        }
    }
    #endregion
}