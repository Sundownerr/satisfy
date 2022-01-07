using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using UniRx;
using System;

namespace Satisfy.Bricks
{
    [Serializable, HideLabel]
    public class EventListenerEmbedded<T, V> where T : Event<V>
    {
        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true, DraggableItems = false), LabelText(" ")]
        [PropertyOrder(-1), HideLabel]
        [SerializeField] private List<EventListenerData<T, V>> dataList;

        private bool enabled = true;

        public void Initialize()
        {
            foreach (var data in dataList)
            {
                Observable.Merge(data.Events.Select(x => x.Raised))
                    .Where(_ => enabled)
                    .Subscribe(arg =>
                    {
                        data.Response?.Invoke(arg);
                    });
            }
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
        }
    }

    // public class EnemyDataListener : EventListenerEmbedded<EnemyData, >
}