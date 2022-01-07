using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Lean.Touch;

namespace Satisfy.Managers
{
    // public static class InputManager
    // {
    //     public static event Action Tap;
    //     public static float ScreenHalfHeigh => Screen.height / 2;
    //     public static float FingerDeltaX { get; private set; }
    //     public static float FingerDeltaY { get; private set; }
    //     public static float AbsFingerDelta => Math.Abs(FingerDeltaX);
    //     public static Vector2 ScreenPos;
    //     public static bool IsHolding => isHeld && FingerDeltaX != 0;
    //     public static bool IsOnUI { get; private set; }
    //     static bool isHeld;

    //     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    //     static void Init()
    //     {
    //         LeanTouch.OnFingerUpdate += FingerHold;
    //         void FingerHold(LeanFinger obj)
    //         {
    //             FingerDeltaX = obj.ScaledDelta.x / 2;
    //             FingerDeltaY = obj.ScaledDelta.y / 2;
    //             // ScreenPos = obj.;
    //             IsOnUI = obj.IsOverGui;
    //         }

    //         LeanTouch.OnFingerDown += FingerDown;
    //         void FingerDown(LeanFinger obj)
    //         {
    //             isHeld = true;
    //             Tap?.Invoke();
    //         }

    //         LeanTouch.OnFingerExpired += FingerExpired;
    //         void FingerExpired(LeanFinger obj)
    //         {
    //             isHeld = false;
    //         }

    //         LeanTouch.OnFingerUp += FingerUp;
    //         void FingerUp(LeanFinger obj)
    //         {
    //             isHeld = false;
    //             IsOnUI = obj.IsOverGui;
    //         }
    //     }
    // }
}