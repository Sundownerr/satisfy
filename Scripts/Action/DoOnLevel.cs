using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;
using System;
using Satisfy.Variables;
using Satisfy.Attributes;

namespace Satisfy.Entities
{
    [Serializable]
    public class LevelAction : UnityEventWithDelay
    {
        [SerializeField, Tweakable] int level;

        public int Level { get => level; set => level = value; }
    }
    public class DoOnLevel : MonoBehaviour
    {
        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false)]
        [SerializeField, Tweakable] List<LevelAction> list;
        [SerializeField, Variable_R] FloatVariable completedLevels;

        void Start()
        {
            list.ForEach(act =>
            {
                if (completedLevels.Value == act.Level)
                    act.Perform();
                else
                {
                    completedLevels.ObserveEveryValueChanged(x => x.Value)
                        .Where(x => enabled)
                        .Where(x => gameObject.activeSelf)
                        .Where(x => x == act.Level)
                        .Subscribe(x => { act.Perform(); }).AddTo(this);
                }
            });
        }
    }
}