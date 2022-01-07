using UnityEngine;
using UniRx;
using System;

namespace Satisfy.Bricks
{
    [Serializable]
    public abstract class EventBase<T> : ScriptableObject
    {
        public abstract Subject<T> Raised { get; }

        [SerializeField] private string description;
        [SerializeField] protected bool debug;
    }
}