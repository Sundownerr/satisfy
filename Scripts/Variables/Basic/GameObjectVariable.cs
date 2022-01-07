using System;
using UniRx;
using UnityEngine;

namespace Satisfy.Variables
{
    [CreateAssetMenu(fileName = "GameObject", menuName = "Variables/GameObject")]
    [Serializable]
    public class GameObjectVariable : Variable<GameObject>
    {
        public void SetNullValue()
        {
            SetValue(null);
        }
    }
}
