using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using UniRx;
using Satisfy.Variables;
using Satisfy.Attributes;

namespace Satisfy.Variables.Utility
{
    [HideMonoScript]
    public abstract class VariableComparer<T, V> : MonoBehaviour where T : Variable<V>
    {
        [HorizontalGroup("1"), HideLabel]
        [SerializeField] protected T variable;
        [HorizontalGroup("1", 60), LabelText("==")]
        [SerializeField] protected V value;
        [SerializeField, Tweakable] UnityEvent onEqual;

        private void Start()
        {
            variable.Changed.Where(_ => enabled && gameObject.activeSelf)
                .Select(x => x.Current)
                .Where(_ => Compare())
                .Subscribe(_ =>
                {
                    onEqual?.Invoke();
                }).AddTo(this);
        }

        public abstract bool Compare();

    }
}