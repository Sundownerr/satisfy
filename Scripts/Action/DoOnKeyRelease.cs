using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Satisfy.Entities
{
    public class DoOnKeyRelease : MonoBehaviour
    {
        [BoxGroup("Settings", true, true), SerializeField, InlineProperty, HideLabel] UnityEventWithDelay action;
        [SerializeField, Tweakable] KeyCode key;

        void Start()
        {
            Observable.EveryUpdate()
                .Where(x => enabled)
                .Where(x => gameObject.activeSelf)
                .Where(x => Input.GetKeyUp(key))
                .Subscribe(x => { action?.Perform(); }).AddTo(this);
        }
    }
}