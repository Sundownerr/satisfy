
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;
using Satisfy.Attributes;

namespace Satisfy.Entities
{
    public class VelocityLimiterXYZ : MonoBehaviour
    {
        [SerializeField, Editor_R] TriggerObject trigger;
        [SerializeField, Editor_R, HideIf("@trigger != null")] Rigidbody target;
        [SerializeField, Tweakable] Vector3 min;
        [SerializeField, Tweakable] Vector3 max;

        void Start()
        {
            if (trigger == null)
                HandleVelocity(target);
            else
            {
                trigger.Entered.Select(x => x.GetComponent<Rigidbody>())
                    .Subscribe(x => { HandleVelocity(x); }).AddTo(this);

                trigger.Exit.Select(x => x.GetComponent<Rigidbody>())
                    .Where(x => x == target)
                   .Subscribe(x => { target = null; }).AddTo(this);
            }

            void HandleVelocity(Rigidbody rBody)
            {
                if (target != null)
                    target = null;

                target = rBody;

                var velocity = target.ObserveEveryValueChanged(x => x.velocity)
                    .TakeWhile(_ => target != null)
                    .Where(_ => enabled);

                velocity.Where(vel => vel.x < min.x)
                    .Subscribe(vel => { target.AddForce((Vector3.right * target.velocity.x) * (Mathf.Abs(min.x) - Mathf.Abs(vel.x))); }).AddTo(this);

                velocity.Where(vel => vel.y < min.y)
                    .Subscribe(vel => { target.AddForce((Vector3.up * target.velocity.y) * (Mathf.Abs(min.y) - Mathf.Abs(vel.y))); }).AddTo(this);

                velocity.Where(vel => vel.z < min.z)
                    .Subscribe(vel => { target.AddForce((Vector3.forward * target.velocity.z) * (Mathf.Abs(min.z) - Mathf.Abs(vel.z))); }).AddTo(this);

                // vel.Where(v => v > max)
                //     .Subscribe(y => { rBody.AddForce(-rBody.velocity * (Mathf.Abs(y) - Mathf.Abs(max))); }).AddTo(this);

                velocity.Where(vel => vel.x > max.x)
                   .Subscribe(vel => { target.AddForce(-(Vector3.right * target.velocity.x) * (Mathf.Abs(vel.x) - Mathf.Abs(max.x))); }).AddTo(this);

                velocity.Where(vel => vel.y > max.y)
                  .Subscribe(vel => { target.AddForce(-(Vector3.up * target.velocity.y) * (Mathf.Abs(vel.y) - Mathf.Abs(max.y))); }).AddTo(this);

                velocity.Where(vel => vel.z > max.z)
                  .Subscribe(vel => { target.AddForce(-(Vector3.forward * target.velocity.z) * (Mathf.Abs(vel.z) - Mathf.Abs(max.z))); }).AddTo(this);
            }
        }
    }
}

// this.ObserveEveryValueChanged(x => x.target)
//     .Where(x => x != null)
//     .Subscribe(rBody =>
//     {
//         var vel = rBody.ObserveEveryValueChanged(v => v.velocity.magnitude)
//             .TakeWhile(_ => rBody != null)
//             .Where(_ => enabled);

//         vel.Where(x => x < min)
//             .Subscribe(x => { target.AddForce(target.velocity * (Mathf.Abs(min) - Mathf.Abs(x))); }).AddTo(this);

//         vel.Where(x => x > max)
//             .Subscribe(y => { target.AddForce(-target.velocity * (Mathf.Abs(y) - Mathf.Abs(max))); }).AddTo(this);
//     }).AddTo(this);
