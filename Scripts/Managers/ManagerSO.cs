using System.Collections;
using Satisfy.Variables;
using Satisfy.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using UnityEngine.Events;
using Satisfy.Attributes;

namespace Satisfy.Managers
{
    public abstract class ScriptableObjectSystem : ScriptableObject
    {
        [SerializeField, PropertyOrder(-1), PropertySpace(10)] private UnityEvent onInitialize;

        public virtual void Initialize()
        {
            onInitialize?.Invoke();
        }
    }
}
