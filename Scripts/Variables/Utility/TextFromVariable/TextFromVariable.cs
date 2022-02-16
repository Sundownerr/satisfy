using Satisfy.Attributes;
using Satisfy.Utility;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;

namespace Satisfy.Variables.Utility
{
    [HideMonoScript]
    public class TextFromVariable<T, V> : MonoBehaviour where T : Variable<V>
    {
        [SerializeField] [Variable_R] protected T variable;
        [SerializeField] [Editor_R] protected TMP_Text text;
        [SerializeField] [Tweakable] protected string before;
        [SerializeField] [Tweakable] protected string after;

        protected virtual void Start()
        {
            SetText();

            variable.Changed.Select(x => x.Current)
                .Subscribe(x => SetText()).AddTo(this);
        }

        protected virtual void OnEnable()
        {
            SetText();
        }

        protected virtual void SetText()
        {
            if (variable == null)
                return;

            text.text = $"{before}{variable.Value}{after}";
        }
    }
}