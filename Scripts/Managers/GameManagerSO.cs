using System;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
namespace Satisfy.Managers
{
    [CreateAssetMenu(fileName = "Game Manager SO", menuName = "Managers/Game")]
    [Serializable]
    public class GameManagerSO : ScriptableObjectSystem
    {
        [Range(0f, 1f)]
        [SerializeField, Tweakable] float timeScale = 1;
        [SerializeField, Tweakable] int targetFPS = 60;

        public override void Initialize()
        {
#if UNITY_EDITOR
            QualitySettings.vSyncCount = 0;
#endif
            Application.targetFrameRate = targetFPS;

            var update = Observable.EveryUpdate();

            update.Where(_ => Application.targetFrameRate != targetFPS)
                .Subscribe(_ => { Application.targetFrameRate = targetFPS; });
        }
    }
}