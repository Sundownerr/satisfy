using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Satisfy.Bricks
{
    [HideMonoScript]
    [Serializable, CreateAssetMenu(fileName = "Float", menuName = "Bricks/Event/Float")]
    public class FloatEvent : Event<float> { }
}