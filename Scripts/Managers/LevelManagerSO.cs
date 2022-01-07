using System;
using Satisfy.Variables;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using System.Linq;
using Satisfy.Attributes;
using Satisfy.Utility;
using System.Collections;
using System.Collections.Generic;

namespace Satisfy.Managers
{
    [CreateAssetMenu(fileName = "LevelManagerSO", menuName = "Managers/Level")]
    [Serializable]
    public class LevelManagerSO : ScriptableObjectSystem
    {
        [SerializeField, Debugging, LabelWidth(40)] bool debug;
        [SerializeField, Tweakable] KeyCode restartKey = KeyCode.R;
        [PropertyOrder(-2)]
        [SerializeField, Variable_R] Variables.Variable finishButtonPressed;
        [SerializeField, Variable_R] Variables.Variable levelCompleted;
        [SerializeField, Variable_R] Variables.Variable levelDecreased;
        [SerializeField, Variable_R] IntVariable levelIndex;
        [SerializeField, Variable_R] GameObjectVariable levelInstance;
        [SerializeField, Variable_R] GameObjectVariable levelPrefab;
        [SerializeField, Variable_R] Variables.Variable[] loadLevelEvents;
        [HideLabel]
        [SerializeField, GUIColor(0.95f, 0.95f, 1)] GameObjectListVariable levelGroup;
        [HideLabel, InlineEditor(Expanded = true)]
        [SerializeField, GUIColor(0.95f, 0.95f, 1)] GameObjectList usedLevels;

        // [HideLabel]
        // [SerializeField, GUIColor(0.95f, 0.95f, 1)] VariableVariable levelsRefs;
        // [HideLabel, InlineEditor(Expanded = true)]
        // [SerializeField, GUIColor(0.95f, 0.95f, 1)] AssetReferenceList listLevelsRefs;

        void OnValidate()
        {
            if (levelGroup.Value != usedLevels)
                levelGroup.SetValue(usedLevels);
        }

        List<GameObject> levels => levelGroup.Value.List;
        GameObject nextLevel => levels[levelIndex.Value];

        public override void Initialize()
        {
            levelInstance.SetValue(null as GameObject);

            // загрузка уровня после загрузки индекса из сейв файла игрока (только на старте 1 раз)
            levelIndex.Changed.Take(1).Subscribe(_ =>
            {
                var validLevelIndex = Mathf.Clamp(levelIndex.Value, 0, levels.Count - 1);
                levelIndex.SetValue(validLevelIndex);

                if (nextLevel == null)
                {
#if UNITY_EDITOR
                    Debug.LogError($"LevelManager|    Null level at index {levelIndex.Value}");
                    Debug.Break();
#endif
                    var validLevel = levels.Where(x => x != null).First();
                    levelIndex.SetValue(levels.IndexOf(validLevel));
                }

                levelPrefab.SetValue(nextLevel);
#if UNITY_EDITOR
                if (debug)
                    Debug.Log($"LevelManager|    Set level {levelPrefab.Value.name}", levelPrefab.Value);
#endif
            });

            levelPrefab.Changed.Select(x => x.Current)
                .Where(x => x != null)
                .Subscribe(newLevelPrefab =>
                {
                    if (levelInstance.Value != null)
                    {
#if UNITY_EDITOR
                        if (debug)
                            Debug.Log($"LevelManager|    Destroy old level {levelInstance.Value.name}");
#endif
                        GameObject.Destroy(levelInstance.Value);
                    }

                    GameObject.Instantiate(newLevelPrefab);
#if UNITY_EDITOR
                    if (debug)
                        Debug.Log($"LevelManager|    Loaded {newLevelPrefab.name}", levelInstance.Value);
#endif
                });

            foreach (var x in loadLevelEvents)
            {
                x.Published.Subscribe(_ =>
                {
                    levelPrefab.SetValue(nextLevel);
                });
            }

#if UNITY_EDITOR
            Observable.EveryGameObjectUpdate()
                .Where(_ => Input.GetKeyDown(restartKey))
                .Subscribe(_ =>
                {
                    // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    levelIndex.DecreaseBy(1);
                    levelCompleted.Publish();
                    finishButtonPressed.Publish();
                    // levelDecreased.Publish();
                });
#endif
        }

#if UNITY_EDITOR
        [Button("< Предыдущий уровень", ButtonSizes.Large), GUIColor(0.85f, 1, 0.85f), PropertyOrder(-3), HideInEditorMode]
        [HorizontalGroup("Settings/1")]
        void DecreaseLevel()
        {
            levelDecreased.Publish();
            levelPrefab.SetValue(levels[levels.GetPreviousIndex(levelIndex.Value)]);
        }

        [Button("Следующий уровень >", ButtonSizes.Large), GUIColor(0.85f, 1, 0.85f), PropertyOrder(-3), HideInEditorMode]
        [HorizontalGroup("Settings/1")]
        void IncreaseLevel()
        {
            levelCompleted.Publish();
            finishButtonPressed.Publish();
        }
#endif

        // #if UNITY_EDITOR
        //         [Button("Создать новый список")]
        //         void CreateLevelList()
        //         {
        //             var asset = ScriptableObject.CreateInstance<GameObjectList>();

        //             if (!UnityEditor.AssetDatabase.IsValidFolder(StrKey.PrefabsPath))
        //                 UnityEditor.AssetDatabase.CreateFolder(StrKey.AssetsPath, StrKey.Prefabs);

        //             if (!UnityEditor.AssetDatabase.IsValidFolder(StrKey.SOPath))
        //                 UnityEditor.AssetDatabase.CreateFolder(StrKey.PrefabsPath, StrKey.SO);

        //             if (!UnityEditor.AssetDatabase.IsValidFolder(StrKey.LevelsPath))
        //                 UnityEditor.AssetDatabase.CreateFolder(StrKey.SOPath, StrKey.Levels);

        //             UnityEditor.AssetDatabase.CreateAsset(asset, $"{StrKey.LevelsPath}/Levels.asset");
        //             UnityEditor.AssetDatabase.SaveAssets();

        //             levels.Set(asset;
        //         }
        // #endif


    }
}
