using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Satisfy.Data
{
    [Serializable]
    [InlineProperty]
    public class ScriptableObjectWithGuid<T> where T : ScriptableObject
    {
        [HorizontalGroup("1")] [HideLabel] [SerializeField]
        protected T data;

        [HorizontalGroup("1/1")] [HideLabel] [SerializeField] [ReadOnly]
        protected string guid;

        public ScriptableObjectWithGuid(T data, string guid)
        {
            this.data = data;
            this.guid = guid;
        }

        public string Guid
        {
            get => guid;
            set => guid = value;
        }

        public T Value
        {
            get => data;
            set => data = value;
        }
    }
}