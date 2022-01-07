using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Satisfy.Variables
{
    [Serializable]
    public class VariableAction
    {
        [HorizontalGroup("1")]
        [HorizontalGroup("1/1")]
        [LabelText("Do")]
        [SerializeField, DrawWithUnity] UnityEvent action;

        [HorizontalGroup("1/2")]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false)]
        [PropertyOrder(-1), HideLabel, LabelText(" ")]
        [SerializeField] Variable[] message;

        public IEnumerable<Variable> Variables => message;
        public UnityEvent Action => action;
    }

    [Serializable]
    public class VariableWithValueAction
    {
        [HorizontalGroup("1")]
        [HorizontalGroup("1/1")]
        [LabelText("Do")]
        [SerializeField, DrawWithUnity] UnityEvent action;

        [HorizontalGroup("1/2")]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false)]
        [PropertyOrder(-1), HideLabel, LabelText(" ")]
        [SerializeField] VariableWithValue[] variables;

        public IEnumerable<VariableWithValue> Variables => variables;
        public UnityEvent Action => action;
    }

    [HideMonoScript]
    public class VariableListener : MonoBehaviour
    {
        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true, DraggableItems = true), LabelText(" ")]
        [GUIColor(1, 1, 1f), SerializeField] VariableAction[] list;

        void Start()
        {
            foreach (var variableAction in list)
            {
                var goodMessages = variableAction.Variables.Where(x => x != null);

                foreach (var message in goodMessages)
                {
                    message.Published.Where(_ => enabled && gameObject.activeSelf)
                        .Subscribe(_ =>
                        {
                            variableAction.Action?.Invoke();
                        }).AddTo(this);
                }
            }
        }
    }

    [HideMonoScript]
    [Serializable]
    public class VariableListenerModule
    {
        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true, DraggableItems = true), LabelText(" ")]
        [GUIColor(1, 1, 1f), SerializeField] VariableAction[] list;

        private readonly Subject<int> stopped = new Subject<int>();

        public void Initialize()
        {
            var validMessages = list.Where(x => x.Variables != null);

            foreach (var messageAction in validMessages)
            {
                HandleMessage(messageAction);
            }
        }

        private void HandleMessage(VariableAction variableAction)
        {
            foreach (var variable in variableAction.Variables)
            {
                variable.Published.TakeUntil(stopped)
                    .Subscribe(_ =>
                    {
                        variableAction.Action?.Invoke();
                    });
            }
        }

        public void Stop()
        {
            stopped.OnNext(1);
        }
    }
}