using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Satisfy.Utility
{
    public class RotatorStartEnd : DOTweenExt
    {
        [Space]
        [Space]
        [SerializeField, BoxGroup("Settings")] Vector3 startRotation;
        [SerializeField, BoxGroup("Settings")] Vector3 endRotation;
        [SerializeField, BoxGroup("Settings")] float firstTimeDuration;
        int loopsCount = 0;
        bool isFirstTime = true;
        bool isPaused;
        private TweenerCore<Quaternion, Vector3, QuaternionOptions> tween;
        private TweenerCore<Quaternion, Vector3, QuaternionOptions> tween2;

        [Button]
        void SetStartRotation()
        {
            startRotation = target.localRotation.eulerAngles;
        }

        [Button]
        void SetEndRotation()
        {
            endRotation = target.localRotation.eulerAngles;
        }

        [Button]
        void MoveToStartRotation()
        {
            target.localRotation = Quaternion.Euler(startRotation);
        }

        public void StartLoop()
        {
            if (isPaused)
                return;

            var dur = isFirstTime ? firstTimeDuration : duration;

            if (tween.IsActive())
            {
                tween.Play();
            }
            else
                tween = target.DOLocalRotate(endRotation, duration).SetEase(ease)
                    .OnComplete(() =>
                    {
                        if (tween2.IsActive())
                        {

                            tween2.Play();
                        }
                        else
                            tween2 = target.DOLocalRotate(startRotation, duration).SetEase(ease)
                                .OnComplete(() =>
                                {
                                    onComplete?.Invoke();
                                    IncrementLoops();
                                });
                    });
        }

        void IncrementLoops()
        {
            if (isPaused)
                return;

            loopsCount++;

            if (loops < 0)
            {
                StartLoop();
                return;
            }

            if (loopsCount < loops)
                StartLoop();
        }

        public override void Play()
        {
            isPaused = false;
            StartLoop();
            isFirstTime = false;
            onStart?.Invoke();
        }

        public override void Stop()
        {
            isPaused = true;
            isFirstTime = true;
            tween.Kill();
            tween2.Kill();
        }
    }
}