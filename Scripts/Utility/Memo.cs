using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;

namespace Satisfy.Utility
{
    [Serializable]
    public class Memo<T>
    {
        [SerializeField, LabelWidth(50)] private T current;
        [SerializeField, LabelWidth(50), ReadOnly] private T previous;
        public IObservable<T> Changed => this.ObserveEveryValueChanged(x => x.current);

        public Memo(T current)
        {
            this.previous = current;
            this.current = current;
        }

        public T Current
        {
            get => current;
            set
            {
                previous = current;
                current = value;
            }
        }

        public T Previous => previous;
    }
}