using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Satisfy.Attributes;

namespace Satisfy.Entities
{
    public class TrailFader : MonoBehaviour
    {
        [SerializeField, Editor_R] TrailRenderer trail;
        [SerializeField, Tweakable] float fadeTime = 1f;
        [SerializeField, Tweakable] bool doAtStart;

        void Start()
        {
            if (doAtStart)
                Fade();
        }

        public void Fade()
        {
            var mult = trail.widthMultiplier;
            DOTween.To(() => trail.widthMultiplier, x => trail.widthMultiplier = x, 0, fadeTime);
        }
    }
}