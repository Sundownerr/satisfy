
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Satisfy.Attributes
{
    [IncludeMyAttributes]
    [BoxGroup("Refs", false)]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class Editor_R : Attribute
    {

    }

    [IncludeMyAttributes]
    [BoxGroup("Settings", false)]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class Tweakable : Attribute
    {

    }

    [IncludeMyAttributes]
    [FoldoutGroup("Debug", false), PropertyOrder(100)]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class Debugging : Attribute
    {

    }

    [IncludeMyAttributes]
    [BoxGroup("Messages", false)]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class Variable_R : Attribute
    {

    }

    [IncludeMyAttributes]
    [BoxGroup("Messages", false), InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class VariableExpanded_R : Attribute
    {

    }

}