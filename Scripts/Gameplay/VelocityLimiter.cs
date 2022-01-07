using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;
using Satisfy.Attributes;

namespace Satisfy.Entities
{
    public class VelocityLimiter : MonoBehaviour
    {
        [SerializeField, Editor_R] Rigidbody target;
        [SerializeField, Tweakable] float min;
        [SerializeField, Tweakable] float max;

        void Start()
        {
            var vel = this.ObserveEveryValueChanged(x => x.target.velocity.magnitude).Where(x => enabled);

            vel.Where(x => x < min)
                .Subscribe(x => { target.AddForce(target.velocity * (Mathf.Abs(min) - Mathf.Abs(x))); }).AddTo(this);

            vel.Where(x => x > max)
                .Subscribe(y => { target.AddForce(-target.velocity * (Mathf.Abs(y) - Mathf.Abs(max))); }).AddTo(this);
        }
    }
}