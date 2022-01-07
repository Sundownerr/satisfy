using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using Satisfy.Variables;

namespace Satisfy.Entities
{
    [Serializable]
    public class UnityActionWithDelay
    {
        [SuffixLabel("seconds")]
        [SerializeField, LabelWidth(80)] public float delay;

        [LabelText(" ")]
        [SerializeField, DrawWithUnity] UnityEvent action;

        public UnityEvent Action => action;

        public void Perform()
        {
            if (delay <= 0)
                action?.Invoke();
            else
                Get.Delay(delay).Subscribe(_ => { action?.Invoke(); });

        }
    }
}