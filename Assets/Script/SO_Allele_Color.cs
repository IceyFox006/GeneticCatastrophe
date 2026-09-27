using UnityEngine;

[CreateAssetMenu(fileName = "ColorAllele_", menuName = "Scriptable Objects/Genetics/Allele Color")]
public class SO_Allele_Color : SO_Allele
{
    [SerializeField]
    private Color _color;

    #region GS
    public override EGeneType GeneType { get => EGeneType.COLOR; }
    #endregion

    //Spawns the model for the gene at the connection point.
    public override void ApplyToPhenotype(ActGene actGene)
    {

    }
}