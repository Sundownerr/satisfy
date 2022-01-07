using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Satisfy
{

    public static class Get
    {
        static Dictionary<float, WaitForSeconds> delays = new Dictionary<float, WaitForSeconds>(100);
        static Dictionary<float, WaitForSecondsRealtime> delaysRealtime = new Dictionary<float, WaitForSecondsRealtime>(100);
        public static WaitForEndOfFrame EndOfFrame { get; } = new WaitForEndOfFrame();
        public static WaitForFixedUpdate FixedUpdate { get; } = new WaitForFixedUpdate();

        public static IObservable<long> Delay(float delay) =>
            Observable.Timer(TimeSpan.FromSeconds(delay));

        public static IObservable<long> DelayRealtime(float delay) =>
            Observable.Timer(TimeSpan.FromSeconds(delay), Scheduler.MainThreadIgnoreTimeScale);

        // public static void DelayRealtime(float delay, MonoBehaviour mb, Action action)
        // {
        //     mb.StartCoroutine(Wait());
        //     IEnumerator Wait()
        //     {
        //         yield return DelayInSecondsRealtime(delay);
        //         action.Invoke();
        //     }
        // }

        public static WaitForSeconds DelayInSeconds(float delay)
        {
            if (delays.TryGetValue(delay, out var value))
                return value;

            var newDelay = new WaitForSeconds(delay);
            delays.Add(delay, newDelay);

            return newDelay;
        }

        public static WaitForSecondsRealtime DelayInSecondsRealtime(float delay)
        {
            if (delaysRealtime.TryGetValue(delay, out var value))
                return value;

            var newDelay = new WaitForSecondsRealtime(delay);
            delaysRealtime.Add(delay, newDelay);

            return newDelay;
        }

        internal static object Delay(object chargeLifetime)
        {
            throw new NotImplementedException();
        }
    }
}