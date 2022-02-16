using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Satisfy.Variables
{
    [HideMonoScript]
    public class VariableChangeListener<T, V> : MonoBehaviour where V : Variable<T>
    {
        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true, DraggableItems = false)]
        [LabelText(" ")]
        [SerializeField]
        private VariableWithAction<T, V>[] list;

        private void Start()
        {
            foreach (var variableAction in list)
            foreach (var variable in variableAction.Variables)
                variable.Changed
                    .Subscribe(_ => { variableAction.Action?.Invoke(variable.Value); }).AddTo(this);
        }
    }
}