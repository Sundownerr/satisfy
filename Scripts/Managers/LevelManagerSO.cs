using System;
using System.Collections.Generic;
using System.Linq;
using Satisfy.Attributes;
using Satisfy.Utility;
using Satisfy.Variables;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Event = Satisfy.Bricks.Event;

namespace Satisfy.Managers
{
    [CreateAssetMenu(fileName = "LevelManagerSO", menuName = "Managers/Level")]
    [Serializable]
    public class LevelManagerSO : ScriptableObjectSystem
    {
        [SerializeField, Debugging, LabelWidth(40)]
        private bool debug;
        [SerializeField, Tweakable] private KeyCode restartKey = KeyCode.R;
        [PropertyOrder(-2)]
        [SerializeField, Variable_R]
        private Event finishButtonPressed;
        [SerializeField, Variable_R] private Event levelCompleted;
        [SerializeField, Variable_R] private Event levelDecreased;
        [SerializeField, Variable_R] private IntVariable levelIndex;
        [SerializeField, Variable_R] private GameObjectVariable levelInstance;
        [SerializeField, Variable_R] private GameObjectVariable levelPrefab;
        [SerializeField, Variable_R] private Event[] loadLevelEvents;
        
        [HideLabel]
        [SerializeField, GUIColor(0.95f, 0.95f, 1)]
        private GameObjectListVariable levelGroup;
        
        [HideLabel, InlineEditor(Expanded = true)]
        [SerializeField, GUIColor(0.95f, 0.95f, 1)]
        private GameObjectList usedLevels;

        private void OnValidate()
        {
            if (levelGroup.Value != usedLevels)
                levelGroup.SetValue(usedLevels);
        }

        private List<GameObject> levels => levelGroup.DefaultValue.List;
        private GameObject GetNextLevel() => levels[levelIndex.Value];

        public override void Initialize()
        {
            var validLevelIndex = Mathf.Clamp(levelIndex.Value, 0, levels.Count - 1);
            levelIndex.SetValue(validLevelIndex);

            if (GetNextLevel() == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"LevelManager|    Null level at index {levelIndex.Value}");
                Debug.Break();
#endif
                var validLevel = levels.First(x => x != null);
                levelIndex.SetValue(levels.IndexOf(validLevel));
            }

            levelPrefab.SetValue(GetNextLevel());
            LoadLevel(GetNextLevel());
            
#if UNITY_EDITOR
            if (debug)
                Debug.Log($"LevelManager|    Set level {levelPrefab.Value.name}", levelPrefab.Value);
#endif
            foreach (var x in loadLevelEvents)
            {
                x.Raised.Subscribe(_ =>
                {
                    LoadLevel(GetNextLevel());
                });
            }

#if UNITY_EDITOR
            Observable.EveryGameObjectUpdate()
                .Where(_ => Input.GetKeyDown(restartKey))
                .Subscribe(_ =>
                {
                    // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    levelIndex.DecreaseBy(1);
                    levelCompleted.Raise();
                    finishButtonPressed.Raise();
                    // levelDecreased.Publish();
                });
#endif
        }

        private void LoadLevel(GameObject level)
        {
            if (levelInstance.Value != null)
            {
#if UNITY_EDITOR
                if (debug)
                    Debug.Log($"LevelManager|    Destroy old level {levelInstance.Value.name}");
#endif
                Destroy(levelInstance.Value);
            }

            Instantiate(level);
#if UNITY_EDITOR
            if (debug)
                Debug.Log($"LevelManager|    Loaded {level.name}", levelInstance.Value);
#endif
        }

#if UNITY_EDITOR
        [Button("< Предыдущий уровень", ButtonSizes.Large), GUIColor(0.85f, 1, 0.85f), PropertyOrder(-3), HideInEditorMode]
        [HorizontalGroup("Settings/1")]
        private void DecreaseLevel()
        {
            levelDecreased.Raise();
            levelPrefab.SetValue(levels[levels.GetPreviousIndex(levelIndex.Value)]);
        }

        [Button("Следующий уровень >", ButtonSizes.Large), GUIColor(0.85f, 1, 0.85f), PropertyOrder(-3), HideInEditorMode]
        [HorizontalGroup("Settings/1")]
        private void IncreaseLevel()
        {
            levelCompleted.Raise();
            finishButtonPressed.Raise();
        }
#endif
    }
}
