using System.Collections;
using System.Collections.Generic;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Satisfy.Entities
{
    [HideMonoScript]
    public class DoAction : MonoBehaviour
    {
        [BoxGroup("toggle", false)]
        [PropertySpace(10)]
        [SerializeField, LabelText("Play on start"), LabelWidth(80)] bool doOnStart;

        [BoxGroup("action", false)]
        [SerializeField, InlineProperty, HideLabel] UnityActionWithDelay action;

        public UnityActionWithDelay Action => action;

        void Start()
        {
            if (doOnStart)
                action.Perform();
        }

        [Button("Perform action", ButtonSizes.Large), PropertyOrder(-1), GUIColor(0.85f, 1, 0.85f), HideInEditorMode]
        public void Perform()
        {
            action.Perform();
        }
    }
}