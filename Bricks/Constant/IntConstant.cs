using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Satisfy.Bricks
{
    [HideMonoScript]
    [Serializable, CreateAssetMenu(fileName = "Int Constant", menuName = "Bricks/Constant/Int")]
    public class IntConstant : Constant<int> { }
}