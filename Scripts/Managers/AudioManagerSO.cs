using System;
using System.Collections;
using System.Collections.Generic;
using Satisfy.Variables;
using Satisfy.Utility;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using System.Linq;
using Satisfy.Attributes;

namespace Satisfy.Managers
{
    [Serializable]
    public class TweakableAudioClip
    {
        [InlineEditor(InlineEditorModes.SmallPreview)]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false)]
        [LabelText("  ")]
        [HorizontalGroup("ses")]
        [SerializeField, GUIColor(1, 1, 1f), VerticalGroup("ses/1")]
        AudioClip[] clips;

        [SerializeField, GUIColor(1, 1, 0.95f), VerticalGroup("ses/2"), HideLabel, MinMaxSlider(0, 1, true)]
        Vector2 volume = new Vector2(0.5f, 1f);

        [SerializeField, GUIColor(1, 1, 0.95f), VerticalGroup("ses/2"), HideLabel, MinMaxSlider(0, 5, true)]
        Vector2 pitch = new Vector2(0.85f, 1.2f);

        public Vector2 Pitch => pitch;
        public Vector2 Volume => volume;
        public IEnumerable<AudioClip> Clips => clips;

        [HideInInspector] public AudioSource source;
    }

    [Serializable]
    public class MessageAudio
    {
        [HorizontalGroup("mes")]
        [HorizontalGroup("mes/1")]
        [GUIColor(0.95f, 1, 1), SerializeField]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false), HideLabel, LabelText(" ")]
        Variables.Variable[] message;

        [HorizontalGroup("mes/2")]
        [ShowIf("@shouldStop == true")]
        [GUIColor(1, 0.95f, 0.95f), SerializeField]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false), HideLabel, LabelText(" ")]
        Variables.Variable[] messageStop;

        [SerializeField, VerticalGroup("mes/3"), ToggleLeft]
        bool shouldStop;

        [SerializeField, VerticalGroup("mes/3"), ToggleLeft]
        bool loop;

        [GUIColor(1, 1, 0.95f), SerializeField, HideLabel]
        TweakableAudioClip audio;

        ValueDropdownList<bool> loopingValues = new ValueDropdownList<bool>()
        {
            {"No Repeat", false},
            {"Repeat", true}
        };

        public IEnumerable<Variables.Variable> Message => message;
        public IEnumerable<Variables.Variable> MessageStop => messageStop;
        public TweakableAudioClip Audio => audio;
        public bool ShouldStop => shouldStop;
        public bool Loop => loop;
    }

    [Serializable, CreateAssetMenu(fileName = "Audio", menuName = "Managers/Audio")]
    public class AudioManagerSO : ScriptableObjectSystem
    {
        [SerializeField, Variable_R] FloatVariable volume;
        [SerializeField, Variable_R] BoolVariable muted;
        [SerializeField, Tweakable] bool overrideDisableSounds;

        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true), LabelText(" ")]
        [SerializeField, GUIColor(1, 1, 0.95f)]
        MessageAudio[] list;

        public override void Initialize()
        {
            foreach (var messageAudio in list)
            {
                messageAudio.Audio.source = new GameObject().AddComponent<AudioSource>();
                messageAudio.Audio.source.transform.SetParent(Camera.main.transform);
                messageAudio.Audio.source.transform.localPosition = Vector3.zero;

                foreach (var message in messageAudio.Message)
                {
                    var receivedMessage = message.Published.Where(_ => !muted)
                                                           .Where(_ => !overrideDisableSounds);

                    receivedMessage.Subscribe(_ =>
                    {
                        messageAudio.Audio.source.clip = messageAudio.Audio.Clips.GetRandomItem();
                        messageAudio.Audio.source.volume = UnityEngine.Random.Range(messageAudio.Audio.Volume.x, messageAudio.Audio.Volume.y) * volume.Value;
                        messageAudio.Audio.source.pitch = UnityEngine.Random.Range(messageAudio.Audio.Pitch.x, messageAudio.Audio.Pitch.y);
                        messageAudio.Audio.source.loop = messageAudio.Loop;
                        messageAudio.Audio.source.Play();
                    });

                    receivedMessage.Where(_ => messageAudio.ShouldStop)
                        .Subscribe(_ =>
                        {
                            messageAudio.Audio.source.Stop();
                        });
                }
            }

            var volumeChanged = volume.ObserveEveryValueChanged(x => x.Value);

            volumeChanged.Where(x => x <= 0.01f)
                .Subscribe(_ =>
                {
                    muted.SetValue(true);

                    volumeChanged.Where(x => x > 0.01f).Take(1)
                        .Subscribe(m =>
                        {
                            muted.SetValue(false);
                        });
                });
        }

        public void ChangeMuted()
        {
            muted.SetValue(!muted.Value);
        }
    }
}