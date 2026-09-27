using UnityEngine;

[CreateAssetMenu(fileName = "MoAllele_", menuName = "Scriptable Objects/Genetics/Allele Model")]
public class SO_Allele_Model : SO_Allele
{
    [SerializeField]
    private GameObject _model;

    public override void ApplyToPhenotype(Transform parent, ActGene actGene)
    {
        //Find connection point game objects.
        //Spawn models under the connection points.
    }
}
