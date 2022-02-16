using System;
using Satisfy.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Satisfy.Data
{
    [InlineProperty]
    [Serializable]
    public class SerializedData<T> : IResettable
    {
        [BoxGroup("b", false)] [HorizontalGroup("b/1")] [SerializeField] [LabelText("Default")] [LabelWidth(50)]
        protected T defaultValue;

        [HorizontalGroup("b/2")] [SerializeField] [HideInEditorMode] [LabelWidth(50)]
        protected T value;

#if UNITY_EDITOR
        [DisableIf(nameof(disableCondition))]
        [CustomContextMenu("Regenerate Guid", nameof(RegenerateGuid))]
#endif
        [HideLabel]
        [SerializeField]
        [HorizontalGroup("b/1/2")]
        [LabelWidth(30)]
        protected string guid;

        public T Value
        {
            get => value;
            set => this.value = value;
        }

        public string Guid
        {
            get => guid;
            set => guid = value;
        }

        public void ResetToDefault()
        {
            value = defaultValue;
        }

        public string GetSerializedValue()
        {
            return DataHandler.Serialize(value);
        }

#if UNITY_EDITOR
        private bool disableCondition => !string.IsNullOrEmpty(guid);

        protected void RegenerateGuid()
        {
            guid = System.Guid.NewGuid().ToString();
        }
#endif
    }
}