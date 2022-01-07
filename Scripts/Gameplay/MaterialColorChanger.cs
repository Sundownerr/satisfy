using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using Satisfy.Variables;
using DG.Tweening;
using Satisfy.Attributes;
using Satisfy.Variables.VariableReferences;

namespace Satisfy.Entities
{
    [Serializable]
    public class MaterialChangeSettings
    {
        [HorizontalGroup("1")]
        [HideLabel, ReadOnly] public Material material;
        [HideLabel, VerticalGroup("1/2")] public ColorVariableRef targetColor;
        [HideLabel, VerticalGroup("1/2"), EnableIf(nameof(changeEmission))] public ColorVariableRef targetEmisiionColor;
        [HideLabel, VerticalGroup("1/3")] public float time;
        [HideLabel, VerticalGroup("1/3")] public bool changeEmission;

        public MaterialChangeSettings(Material material, ColorVariableRef targetColor, ColorVariableRef targetEmisiionColor, float time)
        {
            this.material = material;
            this.targetColor = targetColor;
            this.targetEmisiionColor = targetEmisiionColor;
            this.time = time;
        }
    }

    [HideMonoScript]
    public class MaterialColorChanger : MonoBehaviour
    {
        [SerializeField, Editor_R, LabelText("Renderer"), OnValueChanged(nameof(PullMaterials))] Renderer targetRenderer;
        [ListDrawerSettings(Expanded = true, HideAddButton = false, AddCopiesLastElement = true, ShowIndexLabels = false, DraggableItems = false)]

        [InlineProperty]
        [SerializeField, Editor_R] List<MaterialChangeSettings> rendererMaterials;

        public Renderer TargetRenderer { get => targetRenderer; set => targetRenderer = value; }

        public void PullMaterials()
        {
            rendererMaterials.Clear();

            targetRenderer.sharedMaterials.ToList().ForEach(x =>
            {
                rendererMaterials.Add(new MaterialChangeSettings(
                    x,
                    new ColorVariableRef() { Value = x.HasProperty("_Color") ? x.GetColor("_Color") : x.GetColor("_BaseColor") },
                    new ColorVariableRef() { Value = x.GetColor("_EmissionColor") },
                    0.3f));
            });
        }

        [Button(ButtonSizes.Large), HideInEditorMode]
        public void ChangeColor()
        {
            // Debug.Log(gameObject, gameObject);
            var materialInstances = targetRenderer.materials;
            for (int i = 0; i < rendererMaterials.Count; i++)
            {
                rendererMaterials[i].material = materialInstances[i];
            }

            rendererMaterials.ForEach(x =>
            {
                x.material.DOColor(x.targetColor.Value, x.time);

                if (x.changeEmission)
                    x.material.DOColor(x.targetEmisiionColor.Value, "_EmissionColor", x.time);
            });
        }

        [Button(ButtonSizes.Large)]
        public void SetColorAlpha(float value)
        {
            rendererMaterials.ForEach(x =>
            {
                var col = x.targetColor.Value;
                col.a = value;

                x.targetColor.Value = col;
            });
        }

        [Button(ButtonSizes.Large)]
        public void SetVariable(ColorVariable variable)
        {
            rendererMaterials.ForEach(x =>
            {
                x.targetColor.SetVariable(variable);
            });
        }
    }
}