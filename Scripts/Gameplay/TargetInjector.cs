using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UniRx;
using System.Linq;
using Satisfy.Entities;
using Satisfy.Attributes;

namespace Satisfy.Utility
{
    public interface ITargetInjectable
    {
        void SetTarget(GameObject target);
    }

    [Serializable]
    public class TargetInjector : SerializedMonoBehaviour
    {
        [SerializeField, Editor_R] List<ITargetInjectable> injectables = new List<ITargetInjectable>();
        [SerializeField, Editor_R] TriggerObject trigger;
        [SerializeField, Tweakable] bool holdFirstEntered = false;
        [SerializeField, Tweakable] bool releaseTargetOnExit = true;
        [SerializeField, Tweakable] bool gatherInjectables = true;
        [SerializeField, Tweakable] bool debug;

        [SerializeField, ShowIf("@debug == true")] GameObject target;

        void OnValidate()
        {
            if (injectables.Count == 0)
                injectables = GetComponents<ITargetInjectable>().ToList();

            if (trigger == null)
                trigger = GetComponent<TriggerObject>();
        }

        void Start()
        {
            if (gatherInjectables)
                // if (injectables.Count == 0)
                injectables = GetComponents<ITargetInjectable>().ToList();

            trigger.Entered
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    if (holdFirstEntered && target != null)
                        return;

                    injectables.ForEach(i => i.SetTarget(x.gameObject));
                    target = x.gameObject;
                }).AddTo(this);

            trigger.Exit.Where(x => x != null)
                .Where(x => x.gameObject == target)
                .Where(_ => releaseTargetOnExit)
                .Subscribe(x =>
                {
                    injectables.ForEach(i => i.SetTarget(null));
                    target = null;
                }).AddTo(this);
        }

        public void SetNullTarget()
        {
            target = null;
        }
    }
}