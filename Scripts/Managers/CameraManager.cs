using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Satisfy.Variables;
using System;
using Sirenix.OdinInspector;
using DG.Tweening;
using System.Linq;
using UnityEngine.Events;
using Satisfy.Attributes;
using Unity.Linq;

namespace Satisfy.Managers
{
    [Serializable]
    public class MessageZoom
    {
        [HorizontalGroup("g1")]
        [BoxGroup("g1/Settings1", false), GUIColor(1, 1, 0.95f), SerializeField, LabelWidth(40), HideLabel, HideIf("@zoomVariable != null")]
        float zoom = 5f;

        [BoxGroup("g1/Settings1", false), GUIColor(1, 1, 0.95f), SerializeField, LabelWidth(40), HideLabel, HideIf("@zoom != 0")]
        FloatVariable zoomVariable;

        [BoxGroup("g1/Settings1", false), GUIColor(1, 1, 0.95f), SerializeField, LabelWidth(40), HideLabel]
        Ease ease = Ease.InOutSine;

        [BoxGroup("g1/Settings2", false), GUIColor(1, 1, 0.95f), SerializeField, LabelWidth(40), LabelText("In"), Min(0)]
        float time = 0.3f;

        [BoxGroup("g1/Settings2", false), GUIColor(1, 1, 0.95f), SerializeField, LabelWidth(40), LabelText("Out"), Min(-1f)]
        float revertTime = 0.2f;

        [HorizontalGroup("g1/Messages"), GUIColor(0.95f, 1, 1), SerializeField]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false), PropertyOrder(-1), HideLabel, LabelText(" ")]
        List<Variables.Variable> message;

        public List<Variables.Variable> Message { get => message; set => message = value; }
        public float Zoom { get => zoom; set => zoom = value; }
        public float Time { get => time; set => time = value; }
        public float RevertTime { get => revertTime; set => revertTime = value; }
        public Ease Ease { get => ease; set => ease = value; }
        public FloatVariable ZoomVariable { get => zoomVariable; set => zoomVariable = value; }
    }

    [Serializable]
    public class MessageSetFOV
    {
        [HorizontalGroup("g1")]
        [BoxGroup("g1/Settings1", false), GUIColor(1, 1, 0.95f), SerializeField, LabelWidth(40), HideLabel, HideIf("@zoomVariable != null")]
        float zoom = 5f;

        [BoxGroup("g1/Settings1", false), GUIColor(1, 1, 0.95f), SerializeField, LabelWidth(40), HideLabel, HideIf("@zoom != 0")]
        FloatVariable zoomVariable;

        [BoxGroup("g1/Settings1", false), GUIColor(1, 1, 0.95f), SerializeField, LabelWidth(40), HideLabel]
        Ease ease = Ease.InOutSine;

        [BoxGroup("g1/Settings2", false), GUIColor(1, 1, 0.95f), SerializeField, LabelWidth(40), LabelText("In"), Min(0)]
        float time = 0.3f;

        [HorizontalGroup("g1/Messages"), GUIColor(0.95f, 1, 1), SerializeField]
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowItemCount = false), PropertyOrder(-1), HideLabel, LabelText(" ")]
        Variables.Variable[] message;

        public IEnumerable<Variables.Variable> Message => message;
        public float Zoom => zoom;
        public float Time => time;
        public Ease Ease => ease;
        public FloatVariable ZoomVariable => zoomVariable;
    }

    [HideMonoScript]
    public abstract class CameraManager : SerializedMonoBehaviour
    {
        [SerializeField, Debugging, LabelWidth(40)] bool debug;

        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true, DraggableItems = false), LabelText("Zoom")]
        [SerializeField, Tweakable]
        MessageZoom[] zoomList;

        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = true, DraggableItems = false), LabelText("Set FOV")]
        [SerializeField, Tweakable]
        MessageSetFOV[] fovList;

        [SerializeField, Editor_R] TransformVariable lookAt;
        [SerializeField, Editor_R] TransformVariable follow;
        [SerializeField, Tweakable] UnityEvent onActiveCameraChanged;

        [SerializeField, HideIf("@debug == false"), Debugging]
        CinemachineVirtualCamera activeCamera;

        [ListDrawerSettings(ShowIndexLabels = false, ShowItemCount = false, Expanded = false, DraggableItems = false), LabelText("Cameras")]
        [SerializeField, HideIf("@debug == false"), Debugging]
        CinemachineVirtualCamera[] cameras;

        [SerializeField, HideIf("@debug == false"), Debugging]
        Dictionary<CinemachineVirtualCamera, float> cameraFov = new Dictionary<CinemachineVirtualCamera, float>();

        void Start()
        {
            cameras = gameObject.Children().OfComponent<CinemachineVirtualCamera>().ToArray();

            foreach (var cam in cameras)
            {
                cameraFov.Add(cam, cam.m_Lens.FieldOfView);
            }

            lookAt.ObserveEveryValueChanged(x => x.Value)
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    foreach (var cam in cameras)
                    {
                        cam.LookAt = x;
                    }
                }).AddTo(this);

            follow.ObserveEveryValueChanged(x => x.Value)
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    foreach (var cam in cameras)
                    {
                        cam.Follow = x;
                        cam.transform.position = x.position;
                    }
                }).AddTo(this);

            foreach (var zoomData in zoomList)
            {
                foreach (var message in zoomData.Message)
                {
                    message.Published.Subscribe(_ => { ChangeFOV(zoomData); }).AddTo(this);
                }
            }

            foreach (var fovData in fovList)
            {
                foreach (var message in fovData.Message)
                {
                    message.Published.Subscribe(_ => { SetFOV(fovData); }).AddTo(this);
                }
            }
        }

        public void SetFOV(MessageSetFOV fovData)
        {
            var activeCam = activeCamera;
            var zoom = fovData.Zoom != 0 ? fovData.Zoom : fovData.ZoomVariable.Value;

            DOTween.Complete(activeCam.m_Lens.FieldOfView, true);
            DOTween.Complete(activeCam.m_Lens, true);
            DOTween.Complete(activeCamera.m_Lens.FieldOfView, true);
            DOTween.Complete(activeCamera.m_Lens, true);

            if (fovData.Time == 0)
                activeCam.m_Lens.FieldOfView = zoom;
            else
                DOTween.To(() => activeCam.m_Lens.FieldOfView, sttr => { activeCam.m_Lens.FieldOfView = sttr; }, zoom, fovData.Time);
        }

        public void ChangeFOV(MessageZoom zoomData)
        {
            var activeCam = activeCamera;
            var defaultFov = activeCam.m_Lens.FieldOfView;
            var zoom = zoomData.Zoom != 0 ? zoomData.Zoom : zoomData.ZoomVariable.Value;

            DOTween.Complete(activeCam.m_Lens.FieldOfView, true);
            DOTween.Complete(activeCam.m_Lens, true);
            DOTween.Complete(activeCamera.m_Lens.FieldOfView, true);
            DOTween.Complete(activeCamera.m_Lens, true);

            if (zoomData.Time == 0)
            {
                activeCam.m_Lens.FieldOfView = defaultFov - zoom;
                HandleRevert();
            }
            else
            {
                DOTween.To(() => activeCam.m_Lens.FieldOfView, sttr => { activeCam.m_Lens.FieldOfView = sttr; }, defaultFov - zoom, zoomData.Time)
                    .OnComplete(() => { HandleRevert(); });
            }

            void HandleRevert()
            {
                if (zoomData.RevertTime >= 0)
                {
                    DOTween.To(() => activeCam.m_Lens.FieldOfView, sttr => { activeCam.m_Lens.FieldOfView = sttr; }, defaultFov, zoomData.RevertTime);
#if UNITY_EDITOR
                    if (debug) Debug.Log($"Camera|    <color=blue>{activeCam.name}</color> revert FoV {defaultFov} in {zoomData.Time}", activeCam.gameObject);
#endif
                }
            }

#if UNITY_EDITOR
            if (debug) Debug.Log($"Camera|    <color=blue>{activeCam.name}</color> set FoV {defaultFov - zoom} in {zoomData.Time}", activeCam.gameObject);
#endif
        }

        public void SetActiveCamera(CinemachineVirtualCamera camera)
        {
            foreach (var cam in cameras)
            {
                cam.gameObject.SetActive(false);
            }

            camera.gameObject.SetActive(true);
            activeCamera = camera;

            onActiveCameraChanged?.Invoke();

#if UNITY_EDITOR
            if (debug) Debug.Log($"Camera|    <color=blue>{activeCamera.name}</color> is Acitve", activeCamera.gameObject);
#endif
        }

        public void ResetZoom()
        {
            var time = 1f;

            foreach (var cam in cameras)
            {
                if (cam.m_Lens.FieldOfView != cameraFov[cam])
                {
                    DOTween.To(() => cam.m_Lens.FieldOfView, val => { cam.m_Lens.FieldOfView = val; }, cameraFov[cam], time);
#if UNITY_EDITOR
                    if (debug) Debug.Log($"Camera|    Reset <color=blue>{cam}</color> zoom to {cameraFov[cam]} in {time}", cam.gameObject);
#endif
                }
            }
        }
    }
}