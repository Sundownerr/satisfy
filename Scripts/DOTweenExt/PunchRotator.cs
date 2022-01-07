
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Satisfy.Utility;

namespace Satisfy.Entities
{
    public class PunchRotator : DOTweenExt
    {
        [SerializeField, BoxGroup("settings2", false)] Vector3 offset = Vector3.up * 35;
        [SerializeField, BoxGroup("settings2", false)] float elasticity = 0.5f;
        [SerializeField, BoxGroup("settings2", false)] int vibrato = 3;

        Tweener tween;

        void Awake()
        {
            tween = target.DOPunchRotation(offset, duration, vibrato, elasticity).SetEase(ease).SetLoops(loops, loopType)
                .SetAutoKill(false).Pause()
                .SetDelay(delayy, true)
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
