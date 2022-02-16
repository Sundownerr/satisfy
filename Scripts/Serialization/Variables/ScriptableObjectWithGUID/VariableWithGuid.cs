using System;
using Satisfy.Variables;

namespace Satisfy.Data
{
    [Serializable]
    public class VariableWithGuid<T, V> : ScriptableObjectWithGuid<T> where T : Variable<V>
    {
        public VariableWithGuid(T data, string guid) : base(data, guid)
        {
        }
    }
}