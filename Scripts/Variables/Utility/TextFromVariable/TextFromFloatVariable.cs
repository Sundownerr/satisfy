using System;
using Satisfy.Attributes;
using UnityEngine;

namespace Satisfy.Variables.Utility
{
    public class TextFromFloatVariable : TextFromVariable<IntVariable, int>
    {
        [SerializeField, Tweakable] private int round;
        
        protected override void SetText()
        {
            if (variable == null)
                return;

            var roundedValue = Math.Round((double) variable.Value, round);

            text.text = $"{before}{roundedValue}{after}";
        }
    }
}