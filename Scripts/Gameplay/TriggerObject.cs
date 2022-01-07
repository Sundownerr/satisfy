using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UniRx;
using Sirenix.OdinInspector;
using UniRx.Triggers;
using Satisfy.Variables;
using Satisfy.Attributes;
using Satisfy.Utility;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Satisfy.Entities
{
    public class Triggerable : MonoBehaviour
    {
        public Subject<int> Triggered { get; protected set; } = new Subject<int>();
    }

    [HideMonoScript]
    public class TriggerObject : Triggerable
    {
        public Subject<Collider> Entered { get; private set; } = new Subject<Collider>();
        public Subject<Collider> Exit { get; private set; } = new Subject<Collider>();
        public IReadOnlyCollection<GameObject> EnteredEntities => enteredEntities;

        public Collider Collider => col;

        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false)]
        [SerializeField, Tweakable, LabelText("Tags")]
        private List<string> filterTag = new List<string>() { "Player" };

        [SerializeField, Tweakable] private bool destroyEnteredObject;
        [SerializeField, Tweakable] private bool deactivateEnteredObject;

        [SerializeField, Tweakable, LabelText("Enter")]
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false)]
        private UnityEvent<GameObject>[] enterActions;

        [SerializeField, Tweakable, LabelText("Exit")]
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false)]
        private UnityEvent<GameObject>[] exitActions;

        [SerializeField, Debugging] private bool debug;
        [SerializeField, Debugging] private Color debugColor = new Color(0.3f, 1f, 0.3f, 0.15f);
        [SerializeField, Debugging] private Color debugOutlineColor = new Color(0.1f, 0.1f, 0.1f, 0.05f);

        [SerializeField, ShowIf("@debug == true"), ReadOnly]
        [ListDrawerSettings(Expanded = true)]
        private List<GameObject> enteredEntities = new List<GameObject>();

        private Collider col;

        private void OnValidate()
        {
            col = GetComponent<Collider>();

            if (col == null)
                gameObject.AddComponent<BoxCollider>().isTrigger = true;
            // else
            //     col.isTrigger = true;
        }


        public void AddEntity(GameObject entity)
        {
            if (enteredEntities.Contains(entity))
                return;

            enteredEntities.Add(entity);
        }

        private void Start()
        {
            if (col == null)
            {
                col = GetComponent<Collider>();

                if (col == null)
                    gameObject.AddComponent<BoxCollider>().isTrigger = true;
                // else
                //     col.isTrigger = true;
            }

            Observable.EveryUpdate().Where(_ => enabled && gameObject.activeSelf)
                .Where(_ => enteredEntities.Count > 0)
                .Subscribe(_ =>
                {
                    for (var i = 0; i < enteredEntities.Count; i++)
                    {
                        if (enteredEntities[i] != null) continue;
                        
                        Exit.OnNext(null);
                        PerformExitActions();
                        enteredEntities.RemoveAt(i);
                    }
                }).AddTo(this);

            var entered = this.OnTriggerEnterAsObservable()
                .Where(x => x != null)
                .Where(col => filterTag.Find(tag => col.CompareTag(tag)) != null)
                .Where(col => !enteredEntities.Contains(col.gameObject));

            var exit = this.OnTriggerExitAsObservable()
                .Where(x => x != null)
                .Where(col => filterTag.Find(tag => col.CompareTag(tag)) != null)
                .Where(col => enteredEntities.Contains(col.gameObject));

            entered.Subscribe(x =>
            {
                Entered.OnNext(x);
                enteredEntities.Add(x.gameObject);

                PerformEnterActions();

                if (destroyEnteredObject)
                {
                    Destroy(x.gameObject);
                }

                if (deactivateEnteredObject)
                {
                    x.gameObject.SetActive(false);
                }

                Triggered.OnNext(1);
#if UNITY_EDITOR
                if (debug)
                {
                    Debug.Log($"{x.gameObject.name} entered {name}");
                }
#endif
            }).AddTo(this);

            exit.Subscribe(x =>
            {
                Exit.OnNext(x);
                enteredEntities.Remove(x.gameObject);

                PerformExitActions();
#if UNITY_EDITOR
                if (debug)
                {
                    Debug.Log($"{x.gameObject.name} exit {name}");
                }
#endif
            }).AddTo(this);
        }

        [Button("Enter Action", ButtonSizes.Large), PropertyOrder(-1), GUIColor(0.85f, 1, 0.85f), HideInEditorMode]
        public void PerformEnterActions()
        {
            foreach (var action in enterActions)
            {
                action?.Invoke(enteredEntities.Last());
            }
        }

        [Button("Exit Action", ButtonSizes.Large), PropertyOrder(-1), GUIColor(0.85f, 1, 0.85f), HideInEditorMode]
        public void PerformExitActions()
        {
            foreach (var action in exitActions)
            {
                action?.Invoke(enteredEntities.Last());
            }
        }

        public void ClearEntities()
        {
            enteredEntities.Clear();
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/3D Object/Trigger")]
        private static void Create()
        {
            if (Application.isPlaying) return;

            var trigger = new GameObject();
            trigger.AddComponent<TriggerObject>();
            trigger.gameObject.name = "Trigger";
            trigger.transform.SetParent(Selection.activeGameObject.transform);
            trigger.transform.localPosition = Vector3.zero;
            Selection.activeGameObject = trigger.gameObject;
        }
        
        private void OnDrawGizmos()
        {
            if (col == null)
                return;

            if (col is BoxCollider box)
            {
                Gizmos.color = debugColor;
                Gizmos.matrix = Matrix4x4.TRS(transform.TransformPoint(box.center), transform.rotation,
                    Vector3.Scale(transform.lossyScale, box.size));

                Gizmos.DrawCube(Vector3.zero, Vector3.one);

                Gizmos.color = debugOutlineColor;
                Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            }
        }
#endif
    }
}