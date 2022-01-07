using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using UniRx;

namespace Satisfy.Bricks
{
    [Serializable]
    [HideMonoScript]
    public class EventListener<T, V> : MonoBehaviour where T : Event<V>
    {
        [SerializeField] private EventListenerEmbedded<T, V> listenerEmbedded;

        private void Start()
        {
            listenerEmbedded.Initialize();

            this.ObserveEveryValueChanged(x => x.gameObject.activeSelf)
                .Where(x => x == false)
                .Subscribe(x =>
                {
                    listenerEmbedded.Disable();
                }).AddTo(this);

            this.ObserveEveryValueChanged(x => x.gameObject.activeInHierarchy)
                .Where(x => x == false)
                .Subscribe(x =>
                {
                    listenerEmbedded.Disable();
                }).AddTo(this);

            this.ObserveEveryValueChanged(x => x.gameObject.activeSelf)
                .Where(x => x == true)
                .Subscribe(x =>
                {
                    listenerEmbedded.Enable();
                }).AddTo(this);

            this.ObserveEveryValueChanged(x => x.gameObject.activeInHierarchy)
                .Where(x => x == true)
                .Subscribe(x =>
                {
                    listenerEmbedded.Enable();
                }).AddTo(this);
        }

        private void OnEnable()
        {
            listenerEmbedded.Enable();
        }

        private void OnDisable()
        {
            listenerEmbedded.Disable();
        }
    }

    [Serializable]
    public class BaseListener : EventListenerEmbedded<Event, UniRx.Unit> { }
    [Serializable]
    public class EventListener : EventListener<Event, UniRx.Unit>
    {
    }
}