using System;
using UnityEngine;

namespace Satisfy.Variables
{
    [Serializable, CreateAssetMenu(fileName = "GameObjectList", menuName = "Lists/GameObjectList")]
    public class GameObjectList : ListSO<GameObject>
    {
        public void InsertUsingChildIndex(GameObject item)
        {
            if (list.Contains(item))
                return;

            list.Insert(item.transform.GetSiblingIndex(), item);
        }
    }
}

