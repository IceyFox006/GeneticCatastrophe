using UnityEngine;

[CreateAssetMenu(fileName = "TexAllele_", menuName = "Scriptable Objects/Genetics/Allele Texture")]
public class SO_Allele_Texture : SO_Allele
{
    [SerializeField]
    private Sprite _texture;

    #region GS
    public override EGeneType GeneType { get => EGeneType.TEXTURE; }
    #endregion

    //Spawns the model for the gene at the connection point.
    public override void ApplyToPhenotype(ActGene actGene)
    {

    }
}