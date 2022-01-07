using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Satisfy.Bricks
{
    [HideMonoScript]
    [Serializable, CreateAssetMenu(fileName = "Int", menuName = "Bricks/Event/Int")]
    public class IntEvent : Event<int> { }
}