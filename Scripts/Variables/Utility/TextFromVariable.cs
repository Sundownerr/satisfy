using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using Sirenix.OdinInspector;
using Satisfy.Utility;
using Satisfy.Attributes;

namespace Satisfy.Variables.Utility
{
    public class TextFromVariable : MonoBehaviour
    {
        [SerializeField, Editor_R, HideIf("@text3D != null")] TextMeshProUGUI text;
        [SerializeField, Editor_R, HideIf("@text != null")] TextMeshPro text3D;
        [SerializeField, Variable_R] FloatVariable floatVariable;
        [SerializeField, Variable_R] IntVariable intVariable;
        [SerializeField, Tweakable, ShowIf("@floatVariable != null")] bool round;
        [SerializeField, Tweakable] string before;
        [SerializeField, Tweakable] string after;

        void OnEnable()
        {
            SetText(intVariable != null ? intVariable.Value : (int)floatVariable.Value);
        }

        void Start()
        {
            if (floatVariable != null)
            {
                SetText((int)floatVariable.Value);

                floatVariable.Changed.Select(x =>
                {
                    var val = floatVariable.Value;
                    return round ? Mathf.Round(val) : val;
                })
                .Subscribe(x => { SetText((int)x); }).AddTo(this);
            }

            if (intVariable != null)
            {
                SetText((int)intVariable.Value);

                intVariable.Changed.Subscribe(x => { SetText((int)intVariable.Value); }).AddTo(this);
            }
        }

        void SetText(int value)
        {
            if (text != null) text.text = $"{before}{value.ToStringShort()}{after}";
            if (text3D != null) text3D.text = $"{before}{value.ToStringShort()}{after}";
        }
    }
}