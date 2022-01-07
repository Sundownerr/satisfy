using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Satisfy.Utility;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using UniRx;

namespace Satisfy.Entities
{
    [HideMonoScript]
    public class Mover : DOTweenExt
    {
        [InlineButton("SetEndPos", "Set")]
        [InlineButton("MoveToEndPos", "Move")]
        [SerializeField, BoxGroup("settings2", false), LabelText("End position"), HideIf("@moveTarget != null")] Vector3 endPos;
        [SerializeField, BoxGroup("settings2", false), LabelText("End target")] Transform moveTarget;

        int loopsCount = 0;
        TweenerCore<Vector3, Vector3, VectorOptions> startPosTween;
        bool isPaused;

        void MoveToEndPos() { target.localPosition = endPos; }
        void SetEndPos() { endPos = target.localPosition; }

        void Awake()
        {
            startPosTween = target.DOLocalMove(endPos, duration).SetEase(ease).SetAutoKill(false).Pause()
                .OnComplete(() => HandleComplete())
                .OnRewind(() => { StartLoop(); });
        }

        public void StartLoop()
        {
            if (isPaused) return;

            startPosTween.Restart();
            HandleStart();
        }

        protected override void HandleComplete()
        {
            base.HandleComplete();

            if (isPaused) return;

            loopsCount++;

            if (loops < 0 || loopsCount < loops)
                startPosTween.SmoothRewind();
        }

        public override void Play()
        {
            isPaused = false;
            StartLoop();
        }

        public override void Stop()
        {
            isPaused = true;

            startPosTween.Pause();
        }

        public Mover SetStartY(float y)
        {
            endPos.y = y;
            return this;
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (target == null)
                return;

            Gizmos.color = Color.green;

            var pos1 = target.position;
            var pos2 = endPos;

            if (moveTarget != null)
                pos2 = moveTarget.position;

            Gizmos.DrawLine(pos2, pos1);
            Gizmos.DrawSphere(pos2, 0.1f);
            Gizmos.DrawSphere(pos1, 0.05f);
        }
#endif
    }
}