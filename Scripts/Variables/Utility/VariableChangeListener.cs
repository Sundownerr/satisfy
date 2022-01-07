using System;
using System.Collections;
using System.Collections.Generic;
using Satisfy.Variables;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Satisfy.Variables
{
    [HideMonoScript]
    public class VariableChangeListener : MonoBehaviour
    {
        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true, DraggableItems = false), LabelText(" ")]
        [SerializeField] private VariableWithValueAction[] list;

        private void Start()
        {
            foreach (var variableAction in list)
            {
                foreach (var variable in variableAction.Variables)
                {
                    variable.Changed
                       .Subscribe(_ =>
                       {
                           variableAction.Action?.Invoke();
                       }).AddTo(this);

                }
            }
        }
    }
}