using UnityEngine;

[CreateAssetMenu(fileName = "MoAllele_", menuName = "Scriptable Objects/Genetics/Allele Model")]
public class SO_Allele_Model : SO_Allele
{
    [SerializeField]
    private GameObject _model;

    #region GS
    public override EGeneType GeneType { get => EGeneType.MODEL; }
    #endregion

    //Spawns the model for the gene at the connection point.
    public override void ApplyToPhenotype(ActGene actGene)
    {
        foreach (Transform conPt in actGene.ConnectionPoints)
            Instantiate(_model, conPt);
    }
}
