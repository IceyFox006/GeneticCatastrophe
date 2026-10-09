/*
 * Marlow Greenan
 * Created: 10/06/2026
 * Last Updated: 10/06/1026 by Marlow Greenan
 * 
 * Contains data for a texture allele.
 */
using MarUtility;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TexAllele_", menuName = "Scriptable Objects/Genetics/Allele/Texture")]
public class SO_Allele_Texture : SO_Allele
{
    [SerializeField, BoxGroup("General"), Tooltip("Key is the model name (the game object the texture will be applied to).")]
    private Dictionary<string, Texture> _texData;

    //TEXTURE
    [SerializeField, BoxGroup("Texture"), Label("Texture ID")]
    private string _texID = "_Tex_";

    //Set mesh renderers materials and textures.
    public override void ApplyComplete(Transform p)
    {
        MeshRenderer[] mrs;

        //Set material & apply texture to all meshrenderers.
        mrs = p.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in mrs)
        {
            if (!_texData.ContainsKey(p.name)) //Check if the data has the key of the body stored in it.
            {
                Debug.LogError(p.name + " does not match the name of any of the textures.");
                break;
            }
            
            MarData.SetTexture(mr, _texID, _texData[p.name]);
        }

        //Set part textures.
        List<Transform> parent;
        foreach (KeyValuePair<string, Texture> kvp in _texData)
        {
            if (kvp.Key.Equals(p.name)) continue; //Skip applying to body (already applied).

            parent = MarData.FindChildrenWithName(p, kvp.Key + "(Clone)");
            if (parent == null) continue;

            foreach (Transform c in parent)
            {
                mrs = c.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer mr in mrs)
                    MarData.SetTexture(mr, _texID, kvp.Value);
            }
        }
    }
}
