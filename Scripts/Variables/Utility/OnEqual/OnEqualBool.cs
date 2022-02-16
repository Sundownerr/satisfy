using System;
using System.Collections.Generic;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UniRx;

namespace Satisfy.Variables
{
    [HideMonoScript]
    public class OnEqualBool : MonoBehaviour
    {
     
        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true, DraggableItems = false), LabelText(" ")]
        [GUIColor(1, 1, 0.95f), SerializeField] List<EqualBool> list;

        private void Start()
        {
            foreach (var equalCheck in list)
            {
                foreach (var variable in equalCheck.Variables)
                {
                    if (variable.Value == equalCheck.EqualValue)
                        equalCheck.Action?.Invoke();

                    variable.Changed.Select(x=> x.Current).Where(v => v == equalCheck.EqualValue)
                        .Subscribe(_ => equalCheck.Action?.Invoke()).AddTo(this);
                }
            }

        }
    }
}