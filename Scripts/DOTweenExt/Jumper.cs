using UnityEngine;
using DG.Tweening;
using Satisfy.Utility;
using Sirenix.OdinInspector;
using System;
using Satisfy.Variables.VariableReferences;

namespace Satisfy.Entities
{
    [Serializable]
    public class Jumper : DOTweenExt
    {

        [InlineButton("SetEndPos", "Set")]
        [InlineButton("MoveToEndPos", "Move")]
        [SerializeField, BoxGroup("set2", false), LabelText("End position")] Vector3 endPos;
        // [SerializeField, BoxGroup("set2", false), LabelText("End target")] Transform jumpTo;
        [SerializeField, BoxGroup("set2", false), LabelText("End target Variable")] TransformVariableRef jumpTo;
        [SerializeField, BoxGroup("set2", false)] float power = 3;
        [SerializeField, BoxGroup("set2", false), Min(0)] int jumpCount = 1;

        int loopsCount = 0;

        bool isPaused;
        bool isFirstTime = true;
        private Sequence tween2;

        void MoveToEndPos() { target.localPosition = endPos; }

        void SetEndPos() { endPos = target.localPosition; }

        void Awake()
        {
            if (jumpTo.Value != null)
                tween2 = target.DOJump(jumpTo.Value.position, power, jumpCount, duration);
            else
                tween2 = target.DOLocalJump(endPos, power, jumpCount, duration);

            tween2.SetEase(ease).SetAutoKill(false).Pause()
                .OnComplete(() => { HandleComplete(); })
                .OnRewind(() => StartLoop());
        }

        public void StartLoop()
        {
            if (isPaused) return;

            base.HandleStart();

            if (useCachedTween)
            {
                tween2.Restart();
            }
            else
            {
                if (jumpTo.Value != null)
                    tween2 = target.DOJump(jumpTo.Value.position, power, jumpCount, duration);
                else
                    tween2 = target.DOLocalJump(endPos, power, jumpCount, duration);

                tween2.SetEase(ease).OnComplete(() => { HandleComplete(); });
            }
        }

        protected override void HandleComplete()
        {
            base.HandleComplete();

            if (isPaused)
                return;

            loopsCount++;

            if (loopsCount < loops || loops < 0)
                tween2.SmoothRewind();
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (target == null)
                return;


            Gizmos.color = Color.green;

            var pos1 = target.position;
            var pos2 = endPos;

            pos2 = jumpTo != null && jumpTo.Value != null ? jumpTo.Value.position : transform.TransformPoint(endPos);

            Gizmos.DrawLine(pos2, pos1);
            Gizmos.DrawSphere(pos2, 0.5f);
            Gizmos.DrawSphere(pos1, 0.9f);
        }
#endif

        public override void Play()
        {
            isPaused = false;
            StartLoop();
            isFirstTime = false;
        }

        public override void Stop()
        {
            isPaused = true;
            isFirstTime = true;
            tween2.Pause();
        }

        public void Complete()
        {
            isPaused = false;
            isFirstTime = true;
            tween2.Complete();
        }

        public void SetPower(float power)
        {
            this.power = power;
        }

        public void SetTime(float time)
        {
            this.duration = time;
        }
    }
}