using System.Collections;
using System.Collections.Generic;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Satisfy.Entities
{
    public class ColliderObject : MonoBehaviour
    {
        public Subject<Collision> Entered { get; private set; } = new Subject<Collision>();
        public Subject<Collision> Exit { get; private set; } = new Subject<Collision>();

        [SerializeField] bool debug;

        [SerializeField, Tweakable, LabelText("Теги для активации триггера")] List<string> filterTag;
        [BoxGroup("Settings1", false), GUIColor(1, 1, 0.95f), SerializeField, LabelText("При входе в триггер")] UnityEvent actionOnTriggerEnter;
        [BoxGroup("Settings1", false), GUIColor(1, 1, 0.95f), SerializeField, LabelText("При выходе из триггера")] UnityEvent actionOnTriggerExit;

        List<GameObject> enteredEntities = new List<GameObject>();

        void OnValidate()
        {
            var col = GetComponent<Collider>();

            if (col == null)
                gameObject.AddComponent<BoxCollider>();
        }

        void OnCollisionEnter(Collision other)
        {
            var entity = other.gameObject;
            var isEntityValid = filterTag.Find(x => other.gameObject.CompareTag(x)) != null;

            if (!isEntityValid)
                return;

            isEntityValid = enteredEntities.Find(x => x == entity) == null;

            if (!isEntityValid)
                return;

            enteredEntities.Add(entity);
            Entered.OnNext(other);
            actionOnTriggerEnter?.Invoke();

            // Triggered.OnNext(1);

            if (debug)
                Debug.Log($"{other.gameObject.name} entered {name}");
        }

        void OnCollisionExit(Collision other)
        {
            var entity = other.gameObject;
            var isEntityValid = filterTag.Find(x => other.gameObject.CompareTag(x)) != null;

            if (!isEntityValid)
                return;

            isEntityValid = enteredEntities.Find(x => x == entity) != null;

            if (!isEntityValid)
                return;

            enteredEntities.Remove(entity);
            Exit.OnNext(other);
            actionOnTriggerExit?.Invoke();

            if (debug)
                Debug.Log($"{other.gameObject.name} exit {name}");
        }
    }
}