using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Satisfy.Utility
{
    public class Rotator : DOTweenExt
    {

        [BoxGroup("settings2", false)]
        [HorizontalGroup("settings2/1")]
        [SerializeField, VerticalGroup("settings2/1/1"), HideLabel] Vector3 rotation;
        [SerializeField, VerticalGroup("settings2/1/2"), HideLabel] RotateMode mode;

        Vector3 defaultRotation;
        TweenerCore<Quaternion, Vector3, QuaternionOptions> tween;

        void Awake()
        {
            defaultRotation = target.localRotation.eulerAngles;
            var targetRotation = mode == RotateMode.LocalAxisAdd ? rotation : defaultRotation + rotation;

            tween = target.DOLocalRotate(targetRotation, duration, mode)
                .SetEase(ease).SetLoops(loops, loopType)
                 // .OnComplete(() => { HandleComplete(); })
                 .OnStepComplete(() => { HandleComplete(); })
                .SetAutoKill(false).Pause();
        }

        public override void Play()
        {
            if (!enabled || !gameObject.activeSelf)
                return;

            tween.Restart();
            HandleStart();
        }

        public override void Stop()
        {
            if (!enabled || !gameObject.activeSelf)
                return;

            tween.Pause();
        }
    }
}