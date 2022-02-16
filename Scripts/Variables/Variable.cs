using System;
using System.Collections;
using System.Collections.Generic;
using Satisfy.Utility;
using Sirenix.OdinInspector;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Satisfy.Variables
{
    public class Variable<T> : ScriptableObject
    {
        public IObservable<Pair<T>> Changed => this.ObserveEveryValueChanged(x => x.value).Pairwise();
        public T Value => value;
        public T DefaultValue => defaultValue;
        
        [SerializeField] protected T defaultValue;
        
        [HideInEditorMode]
        [NonSerialized]
        [LabelText("Runtime value")]
        [ShowInInspector] protected T value;

        [HideInInlineEditors] [SerializeField] protected bool disableReset;
        
        [HideInInlineEditors]
        [SerializeField] protected bool debug;
        
        public void SetValue(T newValue)
        {
            if (Value != null && Value .Equals( newValue))
                return;
            
            value = newValue;

            if (debug)
                Debug.Log($"[ changed ]  {name} = {newValue}", this);
        }

        public void HardReset()
        {
            if(disableReset)
                return;
            
            value = defaultValue;

            if (debug)
                Debug.Log($"[ reset ]  {name} = {defaultValue}", this);
        }

        [Button("Force Changed", ButtonSizes.Medium), PropertyOrder(-3), HideInInlineEditors, HideInEditorMode]
        private void ForceChanged()
        {
            var cur = value;

            SetValue(default(T));
            SetValue(cur);
        }
    }
}