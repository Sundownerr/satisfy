using System.Collections;
using System.Collections.Generic;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Satisfy.Entities
{
    public class InterpolateSetter : MonoBehaviour
    {
        [SerializeField, Editor_R] List<Rigidbody> rigidbodies;
        [SerializeField, Tweakable] RigidbodyInterpolation interpolation;

        public void Set()
        {
            rigidbodies.ForEach(x =>
            {
                if (x != null)
                    x.interpolation = interpolation;
            });
        }
    }
}
