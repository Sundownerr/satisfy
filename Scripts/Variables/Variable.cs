using System;
using System.Collections;
using System.Collections.Generic;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Satisfy.Variables
{
    [CreateAssetMenu(fileName = "Base", menuName = "Variables/Base")]
    [Serializable]
    public class Variable : ScriptableObject
    {
        public Subject<Variable> Published { get; } = new Subject<Variable>();

        [SerializeField, HideInInlineEditors] protected bool debug;

        [Button("Publish", ButtonSizes.Medium), PropertyOrder(-3), HideInInlineEditors, HideInEditorMode]
        public void Publish()
        {
            Published.OnNext(this);

            if (debug)
                Debug.Log($"[ published ]  {name}", this);
        }
    }
}