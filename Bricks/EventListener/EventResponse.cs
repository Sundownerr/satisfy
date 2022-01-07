using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Collections;

namespace Satisfy.Bricks
{
    [Serializable]
    public class EventListenerData<EventType, ArgumentType>
        where EventType : Event<ArgumentType>
    {
        [HorizontalGroup("1")]

        [HorizontalGroup("1/1")]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false)]
        [PropertyOrder(-1), HideLabel, LabelText(" ")]
        [SerializeField] private List<EventType> events;

        [HorizontalGroup("1/2")]
        [SerializeField] private UnityEvent<ArgumentType> response;

        public List<EventType> Events => events;
        public UnityEvent<ArgumentType> Response => response;
    }
}