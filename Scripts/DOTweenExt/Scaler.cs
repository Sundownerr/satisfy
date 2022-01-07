using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Satisfy.Utility
{
    public class Scaler : DOTweenExt
    {
        [SerializeField, BoxGroup("settings2", false)] Vector3 targetScale;
        [SerializeField, BoxGroup("settings2", false)] bool fromZero;

        TweenerCore<Vector3, Vector3, VectorOptions> tween;

        void Awake()
        {
            tween = target.DOScale(targetScale, duration).SetAutoKill(false).Pause()
                .SetEase(ease).SetLoops(loops, loopType)
                .OnComplete(() => HandleComplete())
                 .OnStepComplete(() => HandleComplete());

            if (fromZero)
                tween.From(Vector3.zero);
        }

        public override void Play()
        {
            tween.Restart();
            HandleStart();
        }

        public override void Stop()
        {
            tween.Pause();
        }

        [Button, BoxGroup("settings2", false)]
        void SetCurrentScale()
        {
            targetScale = target.localScale;
        }
    }
}