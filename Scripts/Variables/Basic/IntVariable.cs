using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using Satisfy.Attributes;

namespace Satisfy.Variables
{
    [CreateAssetMenu(fileName = "Int", menuName = "Variables/Int")]
    [Serializable]
    public class IntVariable : Variable<int>
    {
        public IObservable<int> Increased => Changed.Where(x => x.Current > x.Previous)
                                                    .Select(x => x.Current - x.Previous);

        public IObservable<int> Decreased => Changed.Where(x => x.Current < x.Previous)
                                                    .Select(x => x.Current - x.Previous);

        public void IncreaseBy(int value)
        {
            SetValue(this.value.Current + value);

            if (debug)
                Debug.Log($"{name} increased by {value} = {Value}", this);
        }

        public void DecreaseBy(int value)
        {
            SetValue(this.value.Current - value);

            if (debug)
                Debug.Log($"{name} decreased by {value} = {Value}", this);
        }

        public void IncreaseBy(IntVariable variable)
        {
            IncreaseBy(variable.Value);
        }

        public void DecreaseBy(IntVariable variable)
        {
            DecreaseBy(variable.value.Current);
        }
    }
}