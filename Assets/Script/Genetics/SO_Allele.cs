/*
 * Marlow Greenan
 * Created: 10/06/2026
 * Last Updated: 10/06/1026 by Marlow Greenan
 * 
 * Contains data for an allele. Must be a subtype to be created.
 */
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class SO_Allele : ScriptableObject
{
    [SerializeField, BoxGroup("General")]
    protected string _name;
    [SerializeField, BoxGroup("General"), ResizableTextArea]
    protected string _description;

    //Dictionary<string, TYPE> data containing information for what object it affects and how it is effecting it.

    //Applies the allele.
    public virtual void ApplyComplete(GeneController gc) 
    {
        List<Transform> holder;
        ApplyComplete(gc, out holder);
    }
    public virtual void ApplyComplete(GeneController gc, out List<Transform> affected) { affected = new List<Transform>(); }
    public virtual void ApplyIncomplete(GeneController gc, int aNum) { }
}
