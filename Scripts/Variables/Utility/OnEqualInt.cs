using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UniRx;

namespace Satisfy.Variables
{
    public class OnEqualInt : MonoBehaviour
    {
        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true, DraggableItems = false), LabelText(" ")]
        [GUIColor(1, 1, 0.95f), SerializeField] List<EqualInt> list;

        void Start()
        {
            list.ForEach(i => i.Variables.ForEach(var =>
                    var.Changed.Where(_ => enabled && gameObject.activeSelf)
                        .Where(v => var.Value == i.EqualValue)
                        .Subscribe(_ => i.Action?.Invoke()).AddTo(this))
            );
        }
    }
}