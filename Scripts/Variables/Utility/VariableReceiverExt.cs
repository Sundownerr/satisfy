using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Satisfy.Variables
{
    [Serializable]
    public class VariableReceiverExt
    {
        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = false, DraggableItems = false), LabelText("Receiver")]
        [GUIColor(1, 1, 1f), SerializeField]
        VariableAction[] list;

        public void Initialize()
        {
            foreach (var messageAction in list)
            {
                foreach (var message in messageAction.Variables)
                {
                    message.Published.Subscribe(_ =>
                    {
                        messageAction.Action?.Invoke();
                    });
                }
            }
        }
    }
}