using System.Collections.Generic;
using System.Linq;
using Satisfy.Attributes;
using Satisfy.Data;
using Satisfy.Utility;
using Satisfy.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Satisfy.Managers
{
    [HideMonoScript]
    public class VariableLoader<T, V> : MonoBehaviour where T : Variable<V>
    {
        [ListDrawerSettings(DraggableItems = false, ShowPaging = true, ShowIndexLabels = false)]
        [SerializeField]
        [Variable_R]
        private List<VariableWithGuid<T, V>> variables;

        private DataHandler dataHandler;

        public void SetDataHandler(DataHandler dataHandler)
        {
            this.dataHandler = dataHandler;
        }

        public void Save()
        {
            foreach (var variable in variables)
                dataHandler.SaveData(variable.Guid, variable.Value.Value);
        }

        public void Load()
        {
            foreach (var variable in variables)
                if (dataHandler.TryLoad<V>(variable.Guid, out var savedValue))
                    variable.Value.SetValue(savedValue);
        }

        public void HardResetVariables()
        {
            foreach (var variableWithGuid in variables)
                variableWithGuid.Value.HardReset();
        }


#if UNITY_EDITOR
        [HideInPrefabInstances]
        [Button]
        public void Fill()
        {
            FillDataFromVariables(Ext.GetAllInstances<T>(), variables);
        }

        private static void FillDataFromVariables(IEnumerable<T> variables, List<VariableWithGuid<T, V>> list)
        {
            if (list == null)
                list = new List<VariableWithGuid<T, V>>();

            list.Clear();
            list.AddRange(variables.Select(v => new VariableWithGuid<T, V>(v, v.GetGuid())));
        }
#endif
    }
}