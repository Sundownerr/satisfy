using System.Collections;
using System.Collections.Generic;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Satisfy.Entities
{
    public class ExplosionForce : MonoBehaviour
    {
        [SerializeField, Tweakable] float force = 5;
        [SerializeField, Tweakable] float radius = 5;
        [SerializeField, Tweakable] float upwardModifier = 1;
        [SerializeField, Tweakable] ForceMode mode;
        [SerializeField, Editor_R] List<Rigidbody> targets;

        public void Push()
        {
            if (targets.Count == 0)
                return;

            targets.ForEach(x =>
            {
                if (x != null)
                    x.AddExplosionForce(force, transform.position, radius, upwardModifier, mode);
            });
        }

        void OnDrawGizmos()
        {
            var col = Color.yellow;
            col.a = 0.3f;

            Gizmos.color = col;


            Gizmos.DrawSphere(transform.position, radius);
        }

    }
}