using System;
using System.Collections;
using System.Collections.Generic;
using Satisfy.Utility;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Satisfy.Variables
{
    [Serializable]
    public class Variable<T> : ScriptableObject
    {
        public IObservable<Memo<T>> Changed => value.Changed.Select(_ => value);
        public T Value => value.Current;
        public T Previous => value.Previous;

        [SerializeField, HideLabel] protected Memo<T> value = new Memo<T>(default(T));
        [SerializeField] protected bool debug;

        public void SetValue(T newValue)
        {
            if (Value != null && Value .Equals( newValue))
                return;
            value.Current = newValue;

            if (debug)
            {
                Debug.Log($"[ changed ]  {name} = {newValue}", this);
            }
        }

        [Button("Force Changed", ButtonSizes.Medium), PropertyOrder(-3), HideInInlineEditors, HideInEditorMode]
        private void ForceChanged()
        {
            var cur = value.Current;

            SetValue(value.Previous);
            SetValue(cur);
        }
    }

    public class VariableWithValue : Variable
    {
        public Subject<int> Changed { get; } = new Subject<int>();
    }
}