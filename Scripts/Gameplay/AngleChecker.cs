using System.Collections;
using System.Collections.Generic;
using Satisfy.Utility;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Events;
using Satisfy.Attributes;

namespace Satisfy.Entities
{
    [Serializable]
    public class AngleCondition
    {
        [Serializable] public enum Compare { GreaterThan, GreaterOrEqual, LessThan, LessOrEqual }

        [HorizontalGroup("ang")]
        [HideLabel] public Compare type;
        [HorizontalGroup("ang")]
        [HideLabel] public int angle;

        public bool IsValidAngle(float angle)
        {
            var result = angle > this.angle;

            if (type == Compare.GreaterThan)
                return result;
            else if (type == Compare.GreaterOrEqual)
                result = angle >= this.angle;
            else if (type == Compare.LessThan)
                result = angle < this.angle;
            else if (type == Compare.LessOrEqual)
                result = angle <= this.angle;

            return result;
        }
    }

    [RequireComponent(typeof(TriggerObject)), RequireComponent(typeof(TargetInjector))]
    public class AngleChecker : MonoBehaviour, ITargetInjectable
    {
        [SerializeField, Editor_R, ReadOnly] Transform target;
        [SerializeField, Editor_R] Collider col;
        [SerializeField, Tweakable, ValueDropdown(nameof(directions))] Vector3 dir;
        [SerializeField, Tweakable, InlineProperty] AngleCondition goodAngle;
        [SerializeField, Tweakable, InlineProperty] AngleCondition badAngle;
        // [SerializeField, Tweakable, InlineProperty] AngleCondition missAngle;
        [SerializeField, Tweakable] UnityEvent onGood;
        [SerializeField, Tweakable] UnityEvent onBad;
        [SerializeField] bool debug;

        ValueDropdownList<Vector3> directions = new ValueDropdownList<Vector3>()
        {
            {"Up", Vector3.up},
            {"Down", Vector3.down},
            {"Right", Vector3.right},
            {"Left", Vector3.left},
            {"Forward", Vector3.forward},
            {"Back", Vector3.back},
        };

        public void SetTarget(GameObject target)
        {
            if (target == null)
                return;

            this.target = target.transform;
        }

        void Start()
        {
            col = GetComponent<Collider>();

            if (col == null)
            {
                Debug.LogError($"Missing collider reference in angle checker {name}");
                return;
            }

            var enterAngle = this.ObserveEveryValueChanged(x => x.target).Where(x => x != null)
                .Select(x =>
                {
                    var posOnCollider = col.ClosestPoint(x.position);

                    return Vector3.Angle(x.transform.InverseTransformDirection(dir), posOnCollider - x.transform.position);
                });

            var onGoodAngle = enterAngle.Where(x => goodAngle.IsValidAngle(x));
            var onBadAngle = enterAngle.Where(x => badAngle.IsValidAngle(x));
            // var onMissAngle = enterAngle.Where(x => x > 61f && x < 150f);

            enterAngle.Subscribe(x =>
            {
                if (debug)
                {
                    Debug.Log($"{name} enter angle = {x}");
                    // Debug.DrawRay(target.transform.position, target.transform.InverseTransformDirection(dir) * 15, Color.red, 5f);
                    // Debug.DrawRay(target.transform.position, transform.position - target.transform.position * 15, Color.yellow, 5f);
                }
            }).AddTo(this);

            onGoodAngle.Subscribe(_ =>
            {
                if (debug) Debug.Log($"{name} good angle");
                onGood?.Invoke();
            }).AddTo(this);

            onBadAngle.Subscribe(_ =>
            {
                if (debug) Debug.Log($"{name} bad angle");
                onBad?.Invoke();
            }).AddTo(this);
        }
    }
}