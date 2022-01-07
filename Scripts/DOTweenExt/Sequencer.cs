using System.Collections;
using System.Collections.Generic;
using Satisfy.Utility;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UniRx;
using DG.Tweening;
using Satisfy.Attributes;

namespace Satisfy.Entities
{
    [Serializable]
    public class SequencePart
    {
        [InlineEditor(InlineEditorModes.GUIOnly, InlineEditorObjectFieldModes.Boxed)]
        [HideLabel]
        public DOTweenExt part;

        [Min(0)] public float delayToNext;

        public SequencePart(DOTweenExt part, float delayToNext)
        {
            this.part = part;
            this.delayToNext = delayToNext;
        }
    }

    [HideMonoScript]
    public class Sequencer : SerializedMonoBehaviour
    {
        [SerializeField, Tweakable] bool playOnStart;
        [SerializeField, Tweakable] Transform target;

        [SerializeField, Tweakable, ValueDropdown(nameof(addPart)), OnValueChanged(nameof(AddNewPart))]
        Type addPartType;

        [SerializeField, LabelText(" ")]
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false)]
        List<SequencePart> list;

        ValueDropdownList<Type> addPart = new ValueDropdownList<Type>()
        {
            {nameof(Jumper), typeof(Jumper)},
            {nameof(Mover), typeof(Mover)},
            {nameof(Scaler), typeof(Scaler)},
            {nameof(Rotator), typeof(Rotator)},
            {nameof(PunchMover), typeof(PunchMover)},
            {nameof(PunchScaler), typeof(PunchScaler)},
            {nameof(PunchRotator), typeof(PunchRotator)},
        };

        public void Play()
        {
            PlayPart(0);

            void PlayPart(int index)
            {
                var part = list[index].part;

                part.Completed.Where(x => index <= list.Count - 1)
                    .Delay(TimeSpan.FromSeconds(list[index].delayToNext))
                    .Subscribe(_ =>
                    {
                        PlayPart(index);
                    }).AddTo(this);

                index++;

                part.Play();
            }
        }

        void Start()
        {
            if (playOnStart)
                Play();
        }

        void AddNewPart()
        {
            if (addPartType == null)
                return;

            var newPartGO = new GameObject(addPartType.Name);
            newPartGO.transform.SetParent(transform);

            var part = newPartGO.AddComponent(addPartType) as DOTweenExt;
            part.SetTarget(target.gameObject);
            part.PlayOnStart = false;
            part.PlayOnEveryActivation = false;

            list.Add(new SequencePart(part, 0));
        }
    }
}