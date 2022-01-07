using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Satisfy.Bricks
{
    [HideMonoScript]
    [Serializable, CreateAssetMenu(fileName = "GameObject Constant", menuName = "Bricks/Constant/GameObject")]
    public class GameObjectConstant : Constant<GameObject> { }
}