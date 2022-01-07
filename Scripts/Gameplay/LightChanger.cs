using Satisfy.Attributes;
using UnityEngine;

namespace Satisfy.Utility
{
    public class LightChanger : MonoBehaviour
    {
        [SerializeField, Tweakable] bool changeAtStart = true;
        [SerializeField, Tweakable] Color lightColor = Color.white;

        void Start()
        {
            if (changeAtStart)
                ChangeColor();
        }

        public void ChangeColor()
        {
            // if (ReferenceHolder.Instance.DirectionalLight != null)
            //     ReferenceHolder.Instance.DirectionalLight.color = lightColor;
        }
    }
}