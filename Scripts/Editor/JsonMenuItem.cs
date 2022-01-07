using System.Collections;
using System.Collections.Generic;
using System.IO;
using Satisfy.Managers;
using UnityEditor;
using UnityEngine;

namespace Satisfy.Editor
{
    public class JsonMenuItem
    {
        [MenuItem("Tools/JSON/Delete")]
        public static void ClearJSON()
        {
            File.Delete(Application.persistentDataPath + "/playerData.json");
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Tools/JSON/Open")]
        public static void OpenJSON()
        {
            EditorUtility.OpenWithDefaultApp(Application.persistentDataPath + "/playerData.json");
        }

        [MenuItem("Tools/JSON/Open folder")]
        public static void OpenJSONFolder()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath + "/playerData.json");
        }
    }
}