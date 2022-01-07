using System.Collections;
using System.Collections.Generic;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Satisfy.Entities
{
    public class DoOnKey : MonoBehaviour
    {
        [BoxGroup("Settings", true, true), SerializeField, InlineProperty, HideLabel] UnityActionWithDelay action;
        [SerializeField, Tweakable] KeyCode key;

        void Start()
        {
            Observable.EveryUpdate()
                .Where(x => enabled)
                .Where(x => gameObject.activeSelf)
                .Where(x => Input.GetKeyDown(key))
                .Subscribe(x => { action?.Perform(); }).AddTo(this);
        }
    }
}