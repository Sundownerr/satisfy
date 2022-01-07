using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Satisfy.Bricks
{
    public abstract class Event<T> : EventBase<T>
    {
        public override Subject<T> Raised { get; } = new Subject<T>();

        [Button, HideInEditorMode]
        public void Raise(T value)
        {
            Raised.OnNext(value);

            if (debug)
            {
                Debug.Log($"[raised] {name} with {value}", this);
            }
        }
    }

    [HideMonoScript]
    [Serializable, CreateAssetMenu(fileName = "Base", menuName = "Bricks/Event/Base")]
    public class Event : Event<UniRx.Unit>
    {
        public override Subject<UniRx.Unit> Raised { get; } = new Subject<UniRx.Unit>();

        [Button, HideInEditorMode]
        public void Raise()
        {
            Raised.OnNext(default);

            if (debug)
            {
                Debug.Log($"[raised] {name}", this);
            }
        }
    }
}

