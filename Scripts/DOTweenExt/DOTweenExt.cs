using System;
using DG.Tweening;
using Satisfy.Variables;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Satisfy.Utility
{
    [HideMonoScript]
    [Serializable]
    public abstract class DOTweenExt : MonoBehaviour, ITargetInjectable
    {
        [BoxGroup("Settings/2", false)]
        [SerializeField, HorizontalGroup("Settings/2/2"), MinValue(0.0001f), LabelText("Duration"), LabelWidth(60)] protected float duration = 0.5f;
        [SerializeField, HorizontalGroup("Settings/2/2"), HideLabel, LabelWidth(60)] protected Ease ease = Ease.Linear;
        [SerializeField, HorizontalGroup("Settings/2/3"), LabelText("Loops"), MinValue(-1), LabelWidth(60)] protected int loops = 0;
        [SerializeField, HorizontalGroup("Settings/2/3"), HideLabel, ShowIf(nameof(haveLoops)), LabelWidth(60)] protected LoopType loopType = LoopType.Restart;
        [SerializeField, HorizontalGroup("Settings/2/4"), LabelText("Delay"), LabelWidth(60)] protected float delayy;

        [HorizontalGroup("Settings")]
        [SerializeField, BoxGroup("Settings/1", false), LabelText("Play On Start"), ToggleLeft, LabelWidth(210)] protected bool playOnStart = true;
        [SerializeField, BoxGroup("Settings/1", false), LabelText("Play On Enable"), ToggleLeft, LabelWidth(210)] protected bool playOnEveryActivation = false;
        [SerializeField, BoxGroup("Settings/1", false), LabelText("Use Cached Tween"), ToggleLeft, LabelWidth(210)] protected bool useCachedTween = true;
        [SerializeField, BoxGroup("Settings/1", false), LabelText("Target"), HideIf("@variableTarget != null"), LabelWidth(50)] protected Transform target;
        [SerializeField, BoxGroup("Settings/1", false), LabelText("Target"), HideIf("@target != null"), LabelWidth(50)] protected Variables.Variable variableTarget;

        [SerializeField, FoldoutGroup("Events", false), LabelText("Start")] protected UnityEvent onStart;
        [SerializeField, FoldoutGroup("Events", false), LabelText("Complete")] protected UnityEvent onComplete;
        public Subject<int> Started { get; private set; } = new Subject<int>();
        public Subject<int> Completed { get; private set; } = new Subject<int>();
        public bool PlayOnStart { get => playOnStart; set => playOnStart = value; }
        public bool PlayOnEveryActivation { get => playOnEveryActivation; set => playOnEveryActivation = value; }

        public bool haveLoops() => loops != 0;

        public abstract void Play();
        public abstract void Stop();

        protected virtual void Start()
        {
            if (playOnStart)
                Play();
        }

        protected virtual void OnEnable()
        {
            if (playOnEveryActivation)
                Play();
        }

        protected virtual void HandleStart()
        {
            onStart?.Invoke();
            Started.OnNext(1);
        }

        protected virtual void HandleComplete()
        {
            onComplete?.Invoke();
            Completed.OnNext(1);
        }

        public void SetTarget(GameObject target)
        {
            if (target == null)
            {
                this.target = null;
                return;
            }

            this.target = target.transform;
        }

        protected virtual void OnValidate()
        {
            if (target == null)
                target = transform;
        }
    }
}