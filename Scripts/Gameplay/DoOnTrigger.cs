using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using Unity.Linq;
using System.Linq;
using Satisfy.Attributes;

namespace Satisfy.Entities
{
    public class DoOnTrigger : MonoBehaviour
    {
        [SerializeField, Tweakable, MinValue(0)] int take = 1;
        [SerializeField, Tweakable] bool waitForAll;
        [SerializeField, Tweakable] UnityEvent onTriggered;
        [SerializeField, Editor_R] List<Triggerable> triggerables;

        public List<Triggerable> Triggerables => triggerables;

        void Start()
        {
            if (triggerables.Count == 0)
                return;

            Activate();
        }

        public void Activate()
        {
            if (waitForAll)
            {
                this.ObserveEveryValueChanged(x => x.triggerables.Count)
                    .Where(x => x == 0)
                    .Subscribe(x => { onTriggered?.Invoke(); }).AddTo(this);

                triggerables.ForEach(t =>
                   t.Triggered.Take(1).Subscribe(x => { triggerables.Remove(t); }).AddTo(this)
                );

                return;
            }

            if (!waitForAll)
            {
                var takes = 0;

                triggerables.ForEach(t =>
                    t.Triggered.TakeWhile(x => takes <= take)
                        .Subscribe(x =>
                        {
                            if (take != 0)
                                takes++;

                            onTriggered?.Invoke();
                        }).AddTo(this)
                );
            }
        }

        public void Add(Triggerable triggerable)
        {
            if (triggerable == null)
            {
                Debug.LogError("Triggerable is null");
                return;
            }

            triggerables.Add(triggerable);
        }

        [Button, BoxGroup("Refs", false)]
        public void GetFromChilds()
        {
            triggerables.Clear();
            triggerables = gameObject.Children().OfComponent<Triggerable>().ToList();
        }
    }
}