using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Satisfy.Utility
{
    public static class Ext
    {
        public static float GetDistanceTo(this Vector3 pos1, Vector3 pos2)
        {
            Vector3 heading;

            heading.x = pos1.x - pos2.x;
            heading.y = pos1.y - pos2.y;
            heading.z = pos1.z - pos2.z;

            var distanceSquared = heading.x * heading.x + heading.y * heading.y + heading.z * heading.z;

            return Mathf.Sqrt(distanceSquared);
        }

        public static float GetDistanceTo(this Transform t1, Transform t2) => t1.position.GetDistanceTo(t2.position);
        public static float GetDistanceTo(this Transform t, Vector3 v) => t.position.GetDistanceTo(v);
        public static float GetDistanceTo(this Vector3 v, Transform t) => t.position.GetDistanceTo(v);
        public static float GetDistanceTo(this GameObject go1, GameObject go2) => go1.transform.position.GetDistanceTo(go2.transform.position);
        public static float GetDistanceTo(this GameObject go1, Transform t) => go1.transform.position.GetDistanceTo(t.position);

        public static float GetRandom(this Vector2 vector2) => UnityEngine.Random.Range(vector2.x, vector2.y);

        public static T GetRandomItem<T>(this IEnumerable<T> list)
        {
            if (list == null || list.Count() == 0)
            {
                Debug.LogWarning("can't get random item: list is null or count == 0");
                return default(T);
            }

            return list.ElementAt(StaticRandom.Instance.Next(0, list.Count()));
        }

        public static T GetRandomItemU<T>(this IEnumerable<T> list)
        {
            if (list == null || list.Count() == 0)
            {
                Debug.LogWarning("can't get random item: list is null or count == 0");
                return default(T);
            }

            return list.ElementAt(UnityEngine.Random.Range(0, list.Count() - 1));
        }

        public static T GetRandomItem<T>(this List<T> list, out int index)
        {
            if (list == null || list.Count == 0)
            {
                Debug.LogWarning("can't get random item: list is null or count == 0");
                index = 0;
                return default(T);
            }

            index = StaticRandom.Instance.Next(0, list.Count);
            return list[index];
        }

        public static int GetRandomIndex<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                Debug.LogWarning("can't get random item: list is null or count == 0");
                return -1;
            }

            return StaticRandom.Instance.Next(0, list.Count);
        }

        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }

        public static int GetNextIndex<T>(this List<T> list, int curIndex)
        {
            if (list == null || list.Count == 0)
            {
                Debug.LogWarning("Can't get next index: list is null or count is 0");
                return -1;
            }

            if (list.Count == 1)
                return 0;

            var nextIndex = curIndex + 1;
            return nextIndex > list.Count - 1 ? 0 : nextIndex;
        }

        public static int GetPreviousIndex<T>(this List<T> list, int curIndex)
        {
            if (list == null || list.Count == 0)
            {
                Debug.LogWarning("Can't get previous index: list is null or count is 0");
                return -1;
            }

            var nextIndex = curIndex - 1;
            return nextIndex < 0 ? list.Count - 1 : nextIndex;
        }

        public static T GetRandomItem<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
            {
                Debug.LogWarning("can't get random item: list is null or count == 0");
                return default(T);
            }

            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        public static T GetRandomItem<T>(this ReactiveCollection<T> list)
        {
            if (list == null || list.Count == 0)
            {
                Debug.LogWarning("can't get random item: list is null or count == 0");
                return default(T);
            }

            return list[StaticRandom.Instance.Next(0, list.Count)];
        }

        public static T GetRandomItem<T>(this ReactiveCollection<T> list, out int index)
        {
            if (list == null || list.Count == 0)
            {
                Debug.LogWarning("can't get random item: list is null or count == 0");
                index = 0;
                return default;
            }

            index = StaticRandom.Instance.Next(0, list.Count);
            return list[index];
        }

        public static int GetRandomIndex<T>(this ReactiveCollection<T> list)
        {
            if (list == null || list.Count == 0)
            {
                Debug.LogWarning("can't get random item: list is null or count == 0");
                return -1;
            }

            return StaticRandom.Instance.Next(0, list.Count);
        }

        public static int GetNextIndex<T>(this ReactiveCollection<T> list, int curIndex)
        {
            if (list == null || list.Count == 0)
            {
                Debug.LogWarning("Can't get next index: list is null or count is 0");
                return -1;
            }

            if (list.Count == 1)
            {
                return 0;
            }

            var nextIndex = curIndex + 1;
            return nextIndex > list.Count - 1 ? 0 : nextIndex;
        }

        public static int GetPreviousIndex<T>(this ReactiveCollection<T> list, int curIndex)
        {
            if (list == null || list.Count == 0)
            {
                Debug.LogWarning("Can't get previous index: list is null or count is 0");
                return -1;
            }

            var nextIndex = curIndex - 1;
            return nextIndex < 0 ? list.Count - 1 : nextIndex;
        }

        public static T GetNextItem<T>(this List<T> list, T curItem)
        {
            if (curItem == null)
            {
                Debug.LogWarning("Can't get next item: current item is null");
                return default;
            }

            if (list == null || list.Count == 0)
            {
                Debug.LogWarning("Can't get next item: list is null or count is 0");
                return default;
            }

            return list.Count == 1 ?
                list[0] :
                list[list.GetNextIndex(list.IndexOf(curItem))];
        }

        public static T GetNextItem<T>(this List<T> list, T curItem, out int index)
        {
            index = -1;

            if (curItem == null)
            {
                Debug.LogWarning("Can't get next item: current item is null");
                return default;
            }

            if (list == null || list.Count == 0)
            {
                Debug.LogWarning("Can't get next item: list is null or count is 0");
                return default;
            }

            index = list.GetNextIndex(list.IndexOf(curItem));

            return list.Count == 1 ? list[0] : list[index];
        }

        public static T GetPreviousItem<T>(this List<T> list, T curItem)
        {
            if (curItem == null)
            {
                Debug.LogWarning("Can't get next item: current item is null");
                return default;
            }

            if (list == null || list.Count == 0)
            {
                Debug.LogWarning("Can't get next item: list is null or count is 0");
                return default;
            }

            return list.Count == 1 ?
                list[0] :
                list[list.GetPreviousIndex(list.IndexOf(curItem))];
        }


        public static bool IsNullOrEmpty<T>(this List<T> list) => list == null || list.Count == 0;

        public static T GetRandomEnum<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new System.Random().Next(v.Length));
        }

        public static T GetRandomEnum<T>(int excludeNum)
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new System.Random().Next(v.Length - excludeNum));
        }

#if UNITY_EDITOR
        public static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            var guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
            var assets = new T[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return assets;
        }

        public static T[] GetAllInstances<T>(T obj) where T : ScriptableObject
        {
            var guids = UnityEditor.AssetDatabase.FindAssets("t:" + obj.GetType().Name);
            var assets = new T[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return assets;
        }
#endif

        public static IObservable<bool> ObserveEnabled(this MonoBehaviour monoBehaviour)
        {
            return monoBehaviour.ObserveEveryValueChanged(x => x.enabled).Where(x => x == true);
        }

        public static IObservable<bool> ObserveDisabled(this MonoBehaviour monoBehaviour)
        {
            return monoBehaviour.ObserveEveryValueChanged(x => x.enabled).Where(x => x == true);
        }

        public static IObservable<Unit> ToObservable(this Action action)
        {
            return Observable.FromEvent(h => action += h, h => action -= h);
        }

        public static IObservable<T> ToObservable<T>(this Action<T> action)
        {
            return Observable.FromEvent<T>(h => action += h, h => action -= h);
        }

        ///<summary>
        /// Return percernt of value
        ///</summary>
        public static float GetPercent(this float value, float desiredPercent) =>
            value / 100f * desiredPercent;

        public static float GetPercent2(this float value, float desiredPercent) =>
            value * 100f / desiredPercent;

        ///<summary>
        /// Return percernt of value
        ///</summary>
        public static float GetPercent(this int value, int desiredPercent) =>
            (float)value / 100f * (float)desiredPercent;

        public static int Replace<T>(this IList<T> source, T oldValue, T newValue)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var index = source.IndexOf(oldValue);

            if (index != -1) source[index] = newValue;

            return index;
        }


        public static void TimesDo(this int times, Action<int> action)
        {
            for (int i = 0; i < times; i++)
                action?.Invoke(i);
        }

        public static Vector3 GetBezierPoint(Vector3 startP, Vector3 midP, Vector3 endP, float t)
        {
            return Vector3.Lerp(
                Vector3.Lerp(startP, midP, t),
                Vector3.Lerp(midP, endP, t),
                t
            );
        }

        public static bool CompareWithTags(this GameObject go, params string[] tags)
        {
            for (int i = 0; i < tags.Length; i++)
                if (go.CompareTag(tags[i]))
                    return true;
            return false;
        }

        public static bool CompareWithTags(this Transform tr, params string[] tags)
        {
            for (int i = 0; i < tags.Length; i++)
                if (tr.CompareTag(tags[i]))
                    return true;
            return false;
        }

        ///<summary>
        /// Return random element from given probabilites
        ///</summary>
        public static int RollDice(this List<int> probabilities)
        {
            var total = 0f;

            probabilities.ForEach(probability => total += probability);

            var randomProbability = StaticRandom.Instance.Next(0, (int)total);

            for (int i = 0; i < probabilities.Count; i++)
                if (randomProbability < probabilities[i])
                    return i;
                else
                    randomProbability -= probabilities[i];
            return -1;
        }

        public static int RollDice(this List<float> probabilities)
        {
            var total = 0f;

            probabilities.ForEach(probability => total += probability);

            var randomProbability = (float)StaticRandom.Instance.Next(0, (int)total);

            for (int i = 0; i < probabilities.Count; i++)
                if (randomProbability < probabilities[i])
                    return i;
                else
                    randomProbability -= probabilities[i];
            return -1;
        }

        public static GameObject CreateTestCube(Color color)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(cube.GetComponent<BoxCollider>());

            cube.GetComponent<MeshRenderer>().material.color = color;
            return cube;
        }

        public static GameObject CreateTestCube(Color color, Vector3 scale)
        {
            var cube = CreateTestCube(color);

            cube.transform.localScale = scale;

            return cube;
        }

        public static List<GameObject> GetAllChilds(this GameObject go)
        {
            var transform = go.transform;
            var list = new List<GameObject>(transform.childCount);

            for (int i = 0; i < transform.childCount; i++)
            {
                list.Add(transform.GetChild(i).gameObject);
            }

            return list;
        }

        public static Type GetUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException
                    (
                     "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                    );
            }
        }
    }

    public static class IndexGiver
    {
        static int index;

        public static int GetIndex()
        {
            var prevIndex = index;
            index++;

            return prevIndex;
        }
    }

    public static class NumberExtensions
    {
        public static string ToStringShort(this int num)
        {
            if (num < 1000)
            {
                return num.ToString();
            }

            if (num < 10000)
            {
                num /= 10;
                return (num / 100f).ToString("#.00'K'", CultureInfo.CurrentCulture);
            }

            if (num < 1000000)
            {
                num /= 100;
                return (num / 10f).ToString("#.0'K'", CultureInfo.CurrentCulture);
            }

            if (num < 10000000)
            {
                num /= 10000;
                return (num / 100f).ToString("#.00'M'", CultureInfo.CurrentCulture);
            }

            num /= 100000;
            return (num / 10f).ToString("#,0.0'M'", CultureInfo.CurrentCulture);
        }
    }
}

