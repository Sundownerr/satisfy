using UnityEngine;

namespace Satisfy.Bricks
{
    public abstract class Constant<T> : ScriptableObject
    {
        [SerializeField] private string description;
        [SerializeField] private T value;

        public T Value => value;
    }
}