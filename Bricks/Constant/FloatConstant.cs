using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Satisfy.Bricks
{
    [HideMonoScript]
    [Serializable, CreateAssetMenu(fileName = "Float Constant", menuName = "Bricks/Constant/Float")]
    public class FloatConstant : Constant<float> { }
}