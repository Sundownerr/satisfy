using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using UniRx;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Satisfy.Attributes;

namespace Satisfy.Entities
{
    public class TimeScaleChanger : MonoBehaviour
    {

        [SerializeField, Tweakable, MinValue(0.001f)] float targetScale = 0.5f;
        [SerializeField, Tweakable, MinValue(0.001f), HideIf("@setPermanent == true")] float setFor = 0.5f;
        [SerializeField, Tweakable, MinValue(0.001f)] float setTime = 0.2f;
        [SerializeField, Tweakable, MinValue(0.001f)] float revertTime = 0.2f;
        [SerializeField, Tweakable] bool setPermanent;
        [SerializeField, Tweakable] bool debug;
        TweenerCore<float, float, FloatOptions> tween;

        public void ChangeTimeScale()
        {
            if (!enabled || !gameObject.activeSelf)
                return;

            DOTween.Kill(Time.timeScale);
            tween = DOTween.To(() => Time.timeScale, sttr => { Time.timeScale = sttr; }, targetScale, setTime);

            if (!setPermanent)
                Get.DelayRealtime(setTime + setFor + 0.001f).Subscribe(_ =>
                {

                    tween = DOTween.To(() => Time.timeScale, sttr => { Time.timeScale = sttr; }, 1, revertTime)
                        .OnComplete(() => { if (debug) Debug.Log($"Timescale = 1 --- {gameObject.name}"); });

                    if (debug) Debug.Log($"Reverting timeScale --- {gameObject.name}");
                }).AddTo(this);

            if (debug) Debug.Log($"TimeScale = {targetScale} --- {gameObject.name}");
        }

        public void Revert()
        {
            tween.Kill();
            DOTween.Kill(Time.timeScale);
            DOTween.To(() => Time.timeScale, sttr => { Time.timeScale = sttr; }, 1, revertTime);
        }
    }
}