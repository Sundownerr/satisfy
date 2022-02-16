using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using UniRx;
using Satisfy.Variables;
using Satisfy.Attributes;
using Satisfy.Utility;

namespace Gongulus.Game
{
    [Serializable]
    public class TweakableAudioClip
    {
        [InlineEditor(InlineEditorModes.SmallPreview)]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false)]
        [LabelText("  ")]
        [HorizontalGroup("ses")]
        [SerializeField, GUIColor(1, 1, 1f), VerticalGroup("ses/1")]
        private AudioClip[] clips;

        [SerializeField, GUIColor(1, 1, 0.95f), VerticalGroup("ses/2"), HideLabel, MinMaxSlider(0, 1, true)]
        private Vector2 volume = new Vector2(0.5f, 1f);

        [SerializeField, GUIColor(1, 1, 0.95f), VerticalGroup("ses/2"), HideLabel, MinMaxSlider(0, 5, true)]
        private Vector2 pitch = new Vector2(0.85f, 1.2f);

        public Vector2 Pitch => pitch;
        public Vector2 Volume => volume;
        public IEnumerable<AudioClip> Clips => clips;
    }

    [HideMonoScript]
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField, Editor_R] private AudioSource source;
        [SerializeField, VariableExpanded_R] private FloatVariable volume;
        [SerializeField, VariableExpanded_R] private BoolVariable muted;
        [HideLabel]
        [SerializeField, Tweakable]
        private TweakableAudioClip clip;

        public void Play()
        {
            if (muted.Value == true)
                return;
            
            source.clip = clip.Clips.GetRandomItem();
            source.volume = UnityEngine.Random.Range(clip.Volume.x, clip.Volume.y) * volume.Value;
            source.pitch = UnityEngine.Random.Range(clip.Pitch.x, clip.Pitch.y);

            source.gameObject.SetActive(true);
            source.enabled = true;
            source.Play();
        }

        public void Stop()
        {
            source.Stop();
        }
    }
}