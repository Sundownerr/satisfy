using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Satisfy.Variables.VariableReferences
{
    [Serializable]
    [InlineProperty]
    public class VariableReference<T, V> where T : Variable<V>
    {
        [HorizontalGroup("VarRef")]
        [SerializeField, ValueDropdown(nameof(valueList)), HideLabel, LabelWidth(40)]
        protected bool useValue = true;

        static ValueDropdownList<bool> valueList = new ValueDropdownList<bool>()
        {
            {"Value", true},
            {"Reference", false}
        };


        [HorizontalGroup("VarRef/1")]
        [SerializeField, HideLabel, HideIf(nameof(useValue), Animate = false), InlineEditor(InlineEditorModes.GUIOnly, InlineEditorObjectFieldModes.Boxed)] T variable;
        [HorizontalGroup("VarRef/1")]
        [SerializeField, HideLabel, ShowIf(nameof(useValue), Animate = false)] V value;

        public V Value
        {
            get => useValue ? value : variable.Value;
            set
            {
                if (useValue)
                    this.value = value;
                else
                    variable.SetValue(value);
            }
        }

        public void SetUseValue(bool useValue)
        {
            this.useValue = useValue;
        }

        public void SetVariable(T variable)
        {
            useValue = false;
            this.variable = variable;
        }
    }
}