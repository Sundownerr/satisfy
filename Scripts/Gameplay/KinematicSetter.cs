using System.Collections;
using System.Collections.Generic;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Satisfy.Entities
{
    public class KinematicSetter : MonoBehaviour
    {
        [SerializeField, Editor_R] List<Rigidbody> rigidbodies;

        public void SetKinematic(bool set)
        {
            rigidbodies.ForEach(x =>
            {
                if (x != null)
                    x.isKinematic = set;
            });
        }
    }
}
