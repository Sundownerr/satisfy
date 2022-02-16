using System;
using System.Collections;
using System.Collections.Generic;
using Satisfy.Variables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Satisfy.Variables
{
    [Serializable]
    public class EqualVariable<T, Y> where T : Variable<Y>
    {
        [HorizontalGroup("ses")]
        [HorizontalGroup("ses/Settings"), GUIColor(1, 1, 0.95f), SerializeField, LabelText("Значание равно")]
        Y equalValue;

        [HorizontalGroup("ses/Messages"), GUIColor(0.95f, 1, 1), SerializeField]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false), PropertyOrder(-1), HideLabel, LabelText(" ")]
        List<T> variables;

        public List<T> Variables { get => variables; set => variables = value; }
        public Y EqualValue { get => equalValue; set => equalValue = value; }

        [LabelText(" ")]
        [GUIColor(1, 1, 0.95f), SerializeField] UnityEvent action;

        public UnityEvent Action { get => action; set => action = value; }
    }

    [Serializable]
    public class EqualFloat : EqualVariable<FloatVariable, float> { }

    [Serializable]
    public class EqualInt : EqualVariable<IntVariable, int> { }

    [Serializable]
    public class EqualBool : EqualVariable<BoolVariable, bool> { }
}