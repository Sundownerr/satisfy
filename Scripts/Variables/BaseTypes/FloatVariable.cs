using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using Satisfy.Attributes;

namespace Satisfy.Variables
{
    [CreateAssetMenu(fileName = "Float", menuName = "Variables/Float")]
    [Serializable]
    public class FloatVariable : Variable<float>
    {
        public IObservable<float> Increased => Changed.Where(x => x.Current > x.Previous)
                                                      .Select(x => x.Current - x.Previous);

        public IObservable<float> Decreased => Changed.Where(x => x.Current < x.Previous)
                                                      .Select(x => x.Current - x.Previous);

        public void IncreaseBy(float value)
        {
            SetValue(this.value + value);

            if (debug)
                Debug.Log($"{name} increased by {value} = {Value}", this);
        }

        public void DecreaseBy(float value)
        {
            SetValue(this.value - value);

            if (debug)
                Debug.Log($"{name} decreased by {value} = {Value}", this);
        }

        public void SetValue(FloatVariable v)
        {
            SetValue(v.Value);
        }

        public void IncreaseBy(FloatVariable variable)
        {
            IncreaseBy(variable.Value);
        }

        public void DecreaseBy(FloatVariable variable)
        {
            DecreaseBy(variable.value);
        }
    }
}
