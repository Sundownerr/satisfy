using System.Collections;
using System.Collections.Generic;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Satisfy.Utility
{
    public class SkyboxChanger : MonoBehaviour
    {
        [SerializeField, Editor_R] bool changeAtStart = true;
        [SerializeField, Editor_R] Material skybox;

        void Start()
        {
            if (changeAtStart)
                Change();
        }

        public void Change()
        {
            if (skybox == null)
                return;

            RenderSettings.skybox = skybox;
        }
    }
}