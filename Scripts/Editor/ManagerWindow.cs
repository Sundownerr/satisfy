
using System;
using System.Collections.Generic;
using Satisfy.Managers;
using Satisfy.Utility;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Satisfy.Editor
{
    public class ManagerWindow : OdinEditorWindow
    {
        [InfoBox("Select manager", InfoMessageType.Info, "showInfo")]
        [OnValueChanged(nameof(ChangeName))]
        [SerializeField, InlineEditor(Expanded = true)] ScriptableObjectSystem manager;

        bool showInfo => manager == null;

        [MenuItem("Window/New Manager Window")]
        private static void OpenWindow()
        {
            CreateWindow<ManagerWindow>().Show();
        }

        void ChangeName()
        {
            if (manager == null)
                return;

            this.titleContent.text = manager.name;
        }
    }
}