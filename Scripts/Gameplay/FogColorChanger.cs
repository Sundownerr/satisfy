using System.Collections;
using System.Collections.Generic;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Satisfy.Utility
{
    public class FogColorChanger : MonoBehaviour
    {
        [SerializeField, Tweakable] bool changeAtStart = true;
        [SerializeField, Tweakable] Color fogColor = Color.gray;

        void Start()
        {
            if (changeAtStart)
                ChangeColor();
        }

        public void ChangeColor()
        {
            RenderSettings.fogColor = fogColor;
        }
    }
}