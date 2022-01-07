using System.Collections;
using System.Collections.Generic;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Satisfy.Entities
{
    public class MaterialsSetter : MonoBehaviour
    {
        [SerializeField, Editor_R] MeshRenderer meshRenderer;
        [SerializeField, Editor_R] SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField, Editor_R] List<Material> materials;

        public void SetMaterials()
        {
            if (meshRenderer != null)
            {
                var mats = meshRenderer.materials;

                for (int i = 0; i < materials.Count; i++)
                    mats[i] = materials[i];

                meshRenderer.materials = mats;

            }

            if (skinnedMeshRenderer != null)
            {
                var mats = skinnedMeshRenderer.materials;

                for (int i = 0; i < materials.Count; i++)
                    mats[i] = materials[i];

                skinnedMeshRenderer.materials = mats;

            }
        }

    }
}