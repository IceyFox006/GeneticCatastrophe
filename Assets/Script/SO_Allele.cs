using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Allele_", menuName = "Scriptable Objects/Genetics/Allele")]
public class SO_Allele : ScriptableObject
{
    //GENERAL
    [SerializeField, BoxGroup("General")]
    protected string _name;

    [SerializeField, BoxGroup("General"), ResizableTextArea]
    protected string _description;

    public virtual void ApplyToPhenotype(Transform parent, ActGene actGene) { }
}
