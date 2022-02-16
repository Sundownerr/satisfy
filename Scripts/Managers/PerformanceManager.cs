using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Gongulus.Game
{
    [HideMonoScript]
    public class PerformanceManager : MonoBehaviour
    {
        [Range(0f, 1f)] [SerializeField] [Tweakable]
        private float timeScale = 1;

        [SerializeField] [Tweakable] private int targetFPS = 60;

        public void Start()
        {
#if UNITY_EDITOR
            // QualitySettings.vSyncCount = 1;
#endif
            Application.targetFrameRate = targetFPS;

            var update = Observable.EveryUpdate();

            update.Where(_ => !Mathf.Approximately(Time.timeScale, timeScale))
                .Subscribe(_ => { Time.timeScale = timeScale; });
        }
    }
}