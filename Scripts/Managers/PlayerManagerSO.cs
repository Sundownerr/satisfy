using System;
using Satisfy.Variables;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using System.Linq;
using Satisfy.Attributes;
using Satisfy.Utility;
using Satisfy.Data;
using Satisfy.Variables.Custom;
using System.Collections.Generic;
using Event = Satisfy.Bricks.Event;

namespace Satisfy.Managers
{
    [Serializable, CreateAssetMenu(fileName = "Player Manager SO", menuName = "Managers/Player")]
    public class PlayerManagerSO : ScriptableObjectSystem
    {
        [SerializeField, Debugging, LabelWidth(40)] bool debug;
        [SerializeField, Tweakable] int levelProgressLimit = 4;
        [SerializeField, Tweakable] KeyCode selectPlayerKey = KeyCode.P;
        [SerializeField, Variable_R] GameObjectVariable playerGO;
        [SerializeField, Variable_R] IntVariable completedLevels;
        [SerializeField, Variable_R] IntVariable progressBarIndex;
        [SerializeField, Variable_R] IntVariable loopFromLevel;
        [SerializeField, Variable_R] IntVariable levelIndex;
        [SerializeField, Variable_R] Event levelCompleted;
        [SerializeField, Variable_R] Event levelDecreased;
        [SerializeField, Variable_R] GameObjectListVariable levelGroup;
        [SerializeField, Variable_R] SkinDataList unlockedSkins;
        [SerializeField, Variable_R] SkinDataList selectedSkins;

        List<GameObject> levels => levelGroup.DefaultValue.List;

        public override void Initialize()
        {
            if (loopFromLevel.Value > levels.Count - 1)
            {
                Debug.LogWarning("Loop from level value is greater then level count");
                loopFromLevel.SetValue(levels.Count - 1);
            }

#if UNITY_EDITOR
            Observable.EveryUpdate()
                .Where(_ => playerGO.Value != null && Input.GetKeyDown(selectPlayerKey))
                .Subscribe(_ =>
                {
                    UnityEditor.Selection.activeGameObject = playerGO.Value;
                });
#endif
            levelCompleted.Raised
                .Subscribe(_ =>
                {
                    var nextLevelIndex = levels.GetNextIndex(levelIndex.Value);

                    if (nextLevelIndex == 0)
                        nextLevelIndex = loopFromLevel.Value;
#if UNITY_EDITOR
                    if (debug) Debug.Log($"PlayerManager|    Next level index = {nextLevelIndex}");
#endif
                    levelIndex.SetValue(nextLevelIndex);
                    completedLevels.IncreaseBy(1);

                    if (completedLevels.Value > (loopFromLevel.Value + 1))
                        progressBarIndex.IncreaseBy(1);

                    if (progressBarIndex.Value > levelProgressLimit)
                        progressBarIndex.SetValue(0);
#if UNITY_EDITOR
                    if (debug)
                    {
                        Debug.Log($"PlayerManager|    Progress bar index = {progressBarIndex.Value}");
                        Debug.Log($"PlayerManager|    Completed Levels = {completedLevels.Value}");
                    }
#endif
                });

            levelDecreased.Raised.Subscribe(_ =>
            {
                var nextLevelIndex = levels.GetPreviousIndex(levelIndex.Value);

                levelIndex.SetValue(nextLevelIndex);
                completedLevels.DecreaseBy(1);
                progressBarIndex.IncreaseBy(1);

                if (progressBarIndex.Value < 0)
                    progressBarIndex.SetValue(5);
#if UNITY_EDITOR
                if (debug) Debug.Log($"PlayerManager|    Level decreased");
#endif
            });
        }
    }
}