using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using UniRx;

namespace Satisfy.Variables.VariableReferences
{

    [Serializable]
    [InlineProperty]
    public class ColorVariableRef : VariableReference<ColorVariable, Color> { }

    [Serializable]
    [InlineProperty]
    public class FloatVariableRef : VariableReference<FloatVariable, float> { }

    [Serializable]
    [InlineProperty]
    public class IntVariableRef : VariableReference<IntVariable, int> { }

    [Serializable]
    [InlineProperty]
    public class GameObjectVariableRef : VariableReference<GameObjectVariable, GameObject> { }

    [Serializable]
    [InlineProperty]
    public class TransformVariableRef : VariableReference<TransformVariable, Transform> { }
}