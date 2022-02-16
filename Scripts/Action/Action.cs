using System.Collections;
using System.Collections.Generic;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Satisfy.Entities
{
    [HideMonoScript]
    public class Action : MonoBehaviour
    {
        [BoxGroup("toggle", false)]
        [SerializeField, LabelText("Play on start"), LabelWidth(80)]
        private bool doOnStart;

        [BoxGroup("action", false)]
        [SerializeField, InlineProperty, HideLabel]
        private UnityEventWithDelay action;
        
        private void Start()
        {
            if (doOnStart)
            {
                action.Perform();
            }
        }

        [Button("Perform action", ButtonSizes.Large), PropertyOrder(-1), GUIColor(0.85f, 1, 0.85f), HideInEditorMode]
        public void Perform()
        {
            if (!gameObject.activeSelf || !enabled)
                return;
            
            action.Perform();
        }
    }
}