using Satisfy.Utility;

namespace Satisfy.Variables.Utility
{
    public class TextFromIntVariable : TextFromVariable<IntVariable, int>
    {
        protected override void SetText()
        {
            if (variable == null)
                return;

            text.text = $"{before}{variable.Value.ToStringShort()}{after}";
        }
    }
}