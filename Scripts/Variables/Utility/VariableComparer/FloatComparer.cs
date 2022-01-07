using UnityEngine;

namespace Satisfy.Variables.Utility
{
    public class FloatComparer : VariableComparer<FloatVariable, float>
    {
        public override bool Compare()
        {
            return Mathf.Approximately(variable.Value, value);
        }
    }
}