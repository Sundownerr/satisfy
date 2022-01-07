using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UniRx;

namespace Satisfy.Entities
{
    public class BlendTreeHandler
    {
        Animator animator;
        string blendTreeName;
        int curID = -1;
        DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> curTween;
        DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> nextTween;

        public BlendTreeHandler(Animator animator, string blendTreeName)
        {
            this.animator = animator;
            this.blendTreeName = blendTreeName;
        }

        public BlendTreeHandler(Animator animator, string blendTreeName, int startID)
        {
            this.animator = animator;
            this.blendTreeName = blendTreeName;
            curID = startID;
        }

        public BlendTreeHandler SetFloat(int ID, float targetValue, float time = 0.1f)
        {
            var curFloat = animator.GetFloat(ID);

            if (time == 0)
                animator.SetFloat(ID, targetValue);
            else
                DOTween.To(() => curFloat, val =>
                {
                    curFloat = val;
                    animator.SetFloat(ID, curFloat);
                }, targetValue, time);

            return this;
        }

        public BlendTreeHandler Reset()
        {
            curID = -1;
            nextTween.Kill();
            curTween.Kill();

            return this;
        }

        public BlendTreeHandler BlendTo(int targetID, float time = 0.2f, bool completePrev = false)
        {
            animator.Play(blendTreeName);

            if (curID != -1)
                RevertCurrentAnimation();

            PlayerTargetAnimation();

            return this;

            void PlayerTargetAnimation()
            {
                curID = targetID;

                var nextFloat = animator.GetFloat(targetID);
                nextTween.Kill(completePrev);

                nextTween = DOTween.To(() => nextFloat, val =>
                {
                    nextFloat = val;
                    animator.SetFloat(targetID, nextFloat);
                }, 1, time);
            }

            void RevertCurrentAnimation()
            {
                var refID = curID;
                var curFloat = animator.GetFloat(curID);

                curTween.Kill(completePrev);

                curTween = DOTween.To(() => curFloat, val =>
                {
                    curFloat = val;
                    animator.SetFloat(refID, curFloat);
                }, 0, time);
            }
        }
    }
}