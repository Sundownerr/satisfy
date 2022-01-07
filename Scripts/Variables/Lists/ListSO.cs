using System.Collections;
using System.Collections.Generic;
using Satisfy.Variables;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Satisfy.Variables
{
    public class ListSO<T> : Variable
    {
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = true, ShowPaging = false), LabelText(" ")]
        [GUIColor(0.95f, 0.95f, 1), SerializeField]
        protected List<T> list;

        public List<T> List => list;
        public int Count => list.Count;

        public Subject<T> Added { get; private set; } = new Subject<T>();
        public Subject<T> Removed { get; private set; } = new Subject<T>();

        public void Add(T item)
        {
            if (list.Contains(item))
                return;

            list.Add(item);
            Added.OnNext(item);

            if (debug) Debug.Log($"{this.name} + {item.ToString()}");
        }

        public void AddExisting(T item)
        {
            list.Add(item);
            Added.OnNext(item);

            if (debug) Debug.Log($"{this.name} + {item.ToString()}");
        }

        public void Remove(T item)
        {
            if (!list.Contains(item))
                return;

            list.Remove(item);
            Removed.OnNext(item);

            if (debug) Debug.Log($"{this.name} - {item.ToString()}");
        }

        public void Clear()
        {
            list.Clear();

            if (debug) Debug.Log($"{this.name} cleared");
        }

        public void CreateList(int capacity)
        {
            list = new List<T>();
        }

        public static implicit operator List<T>(ListSO<T> variable)
        {
            if (variable == null)
                return default(List<T>);

            return variable.List;
        }
    }
}