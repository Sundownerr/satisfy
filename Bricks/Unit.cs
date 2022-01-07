using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Satisfy.Bricks
{
    [HideMonoScript]
    [Serializable, CreateAssetMenu(fileName = "Unit", menuName = "Bricks/Unit")]
    public class Unit : ScriptableObject
    {
        [SerializeField] private string description;
    }
}