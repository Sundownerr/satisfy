using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Satisfy.Variables
{
    public class OnVariableIncreased : MonoBehaviour
    {

        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true, DraggableItems = false), LabelText(" ")]
        [GUIColor(1, 1, 0.95f), SerializeField] VariableAction[] list;

        void Start()
        {
            // foreach (var valueActions in list)
            // {
            //     foreach (var variable in valueActions.Variables)
            //     {
            //         (variable as IntVariable).Increased
            //             .Subscribe(_ =>
            //             {
            //                 valueActions.Action?.Invoke();
            //             }).AddTo(this);
            //     }
            // }
        }
    }
}