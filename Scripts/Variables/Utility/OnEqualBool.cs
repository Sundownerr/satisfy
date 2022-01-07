using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UniRx;

namespace Satisfy.Variables
{
    public class OnEqualBool : MonoBehaviour
    {
        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true, DraggableItems = false), LabelText(" ")]
        [GUIColor(1, 1, 0.95f), SerializeField] List<EqualBool> list;

        void Start()
        {
            list.ForEach(variableEquals =>
                variableEquals.Variables.ForEach(var =>
                    var.Changed.Where(v => var.Value == variableEquals.EqualValue)
                        .Subscribe(_ => variableEquals.Action?.Invoke()).AddTo(this))
            );
        }
    }
}