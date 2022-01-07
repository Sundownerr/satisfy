using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Satisfy.Bricks
{
    [HideMonoScript]
    [Serializable, CreateAssetMenu(fileName = "GameObject", menuName = "Bricks/Event/GameObject")]
    public class GameObjectEvent : Event<GameObject> { }
}