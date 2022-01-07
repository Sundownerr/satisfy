using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Satisfy.Utility
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var instanceInScene = GameObject.FindObjectOfType<T>();
                    instance = instanceInScene;
                    return instance;
                }

                return instance;
            }
            set
            {
                if (instance == null)
                {
                    instance = value;
                }
                else
                {
                    Destroy(value.gameObject);
                    Debug.LogWarning($"Deleting other {typeof(T).Name} singleton instance");
                }
            }
        }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
        }
    }

    public class SingletonDDOL<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }
    }
}