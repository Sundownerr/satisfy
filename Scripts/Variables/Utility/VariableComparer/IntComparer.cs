namespace Satisfy.Variables.Utility
{
    public class IntComparer : VariableComparer<IntVariable, int>
    {
        public override bool Compare()
        {
            return variable.Value == value;
        }
    }
}