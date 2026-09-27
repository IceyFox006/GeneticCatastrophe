using MarUtility.ExecutionManagement;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using MarUtility;

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

    //Initializes the genotype.
    private void InitializeGenotype()
    {
        foreach (KeyValuePair<SO_Gene, ActGene> kvp in _genotype)
            kvp.Value.Initialize(transform); //Initialize act genes.
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

    //Applies phenotype genes to the model. Must have initialized the phenotype beforehand.
    private void ApplyPhenotype()
    {
        foreach (KeyValuePair<SO_Gene, List<SO_Allele>> kvp in phenotype)
        {
            foreach (SO_Allele allele in kvp.Value)
                allele.ApplyToPhenotype(_genotype[kvp.Key]);
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
    private List<Transform> connectionPoints = new List<Transform>();


    #region GS
    public List<SO_Allele> Alleles { get => _alleles; set => _alleles = value; }
    public string[] ConnectPointID { get => _connectPointID; }
    public List<Transform> ConnectionPoints { get => connectionPoints; }
    #endregion

    public void Initialize(Transform p)
    {
        parent = p;
        AssignConnectionPoints();
    }

    //Finds the child with a name matching the connection point and adds it to the connection point lsit.
    private void AssignConnectionPoints()
    {
        connectionPoints.Clear();
        Transform curConPt;
        for (int cpID = 0; cpID < _connectPointID.Length; cpID++)
        {
            curConPt = MarData.FindChildWithName(parent, _connectPointID[cpID]);   

            if (curConPt != null)
                connectionPoints.Add(curConPt);
            else
                Debug.LogError("Failed to find connection point \"" + _connectPointID[cpID] + "\".");
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

    //-----------------------------------------------------------------------------------------------------------------
}