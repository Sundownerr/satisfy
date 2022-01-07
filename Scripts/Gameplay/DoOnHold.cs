using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Satisfy.Entities
{
    public class DoOnHold : MonoBehaviour
    {
        [BoxGroup("Settings"), SerializeField] float holdTime;
        [BoxGroup("Settings"), SerializeField] UnityEvent onHold;

        void Start()
        {
            Observable.EveryGameObjectUpdate()
                .Where(x => Input.GetMouseButton(0))
                .Buffer(TimeSpan.FromSeconds(holdTime))
                .Subscribe(x =>
                {
                    onHold?.Invoke();
                }).AddTo(this);
        }
    }
}
