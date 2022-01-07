using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Satisfy.Utility
{
    public class PunchScaler : DOTweenExt
    {
        [SerializeField, BoxGroup("settings2", false)] Vector3 additionalScale = Vector3.one * 0.25f;
        [SerializeField, BoxGroup("settings2", false)] float elasticity = 0.5f;
        [SerializeField, BoxGroup("settings2", false)] int vibrato = 3;

        Tweener tween;

        void Awake()
        {
            tween = target.DOPunchScale(additionalScale, duration, vibrato, elasticity).SetEase(ease).SetLoops(loops, loopType)
                .SetDelay(delayy)
                .SetAutoKill(false).Pause()
                .OnComplete(() => HandleComplete());
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
    }
}