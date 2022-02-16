using System;
using System.Collections.Generic;
using Satisfy.Variables;
using MoreMountains.NiceVibrations;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Satisfy.Attributes;

namespace Satisfy.Managers
{
    [Serializable]
    public class MessageTaptic
    {
        [HorizontalGroup("ses")]
        [HorizontalGroup("ses/Settings"), SerializeField, HideLabel]
        private HapticTypes type;

        [HorizontalGroup("ses/Messages"), SerializeField]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false), PropertyOrder(-1), HideLabel, LabelText(" ")]
        private Bricks.Event[] tapticEvent;

        public IEnumerable<Bricks.Event> TapticEvent => tapticEvent;
        public HapticTypes Type => type;
    }

    [CreateAssetMenu(fileName = "TapticManagerSO", menuName = "Managers/Taptic")]
    [Serializable]
    public class TapticManagerSO : ScriptableObjectSystem
    {
        [SerializeField, Variable_R] private BoolVariable tapticActive;
        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true, DraggableItems = false), LabelText(" ")]
        [SerializeField, Tweakable]
        private MessageTaptic[] list;

        public override void Initialize()
        {
            MMNViOS.iOSInitializeHaptics();

            foreach (var taptic in list)
            {
                foreach (var message in taptic.TapticEvent)
                {
                    message.Raised.Subscribe(_ =>
                    {
                        MMVibrationManager.Haptic(taptic.Type);
                    });
                }
            }
        }

        // public enum HapticTypes
        // {
        //     Selection 0,
        //     Success 1,
        //     Warning 2,
        //     Failure 3,
        //     LightImpact 4,
        //     MediumImpact 5,
        //     HeavyImpact 6,
        //     RigidImpact 7,
        //     SoftImpact 8,
        //     None 9
        // }

        public void PlayHaptic(int type)
        {
            MMVibrationManager.Haptic((HapticTypes)type );
        }
    }
}
