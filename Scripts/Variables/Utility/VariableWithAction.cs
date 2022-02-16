using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Satisfy.Variables
{
    [Serializable]
    public class VariableWithAction<T, V> where  V: Variable<T>
    {
        [HorizontalGroup("1")]
        [SerializeField, HorizontalGroup("1/1")]
        [LabelText(" ")]
        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true, DraggableItems = false)]
        private V[] variables;

        [SerializeField, HorizontalGroup("1/2")] [LabelText(" ")]
        private UnityEvent<T> action;

        public V[] Variables => variables;
        public UnityEvent<T> Action => action;
    } 
}