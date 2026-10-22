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
    public override void ApplyComplete(GeneController gc, out List<Transform> affected)
    {
        base.ApplyComplete(gc, out affected);

        //Set body textures.
        foreach (MeshRenderer bMR in gc.BodyMeshes)
            MarData.SetTexture(bMR, _texID, _texData[gc.name]);

        //Set part textures.
        MeshRenderer[] partRenderers;
        foreach (Transform pT in gc.PartTransforms)
        {
            string pKey = MarData.WithoutClone(pT.name);
            if (_texData.ContainsKey(pKey))
            {
                partRenderers = pT.GetComponentsInChildren<MeshRenderer>();

                foreach (MeshRenderer mr in partRenderers)
                    MarData.SetTexture(mr, _texID, _texData[pKey]);
            }
        }
    }
}
