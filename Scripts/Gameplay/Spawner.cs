using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Satisfy.Entities
{
    [HideMonoScript]
    public class Spawner : MonoBehaviour
    {
        public Subject<IEnumerable<GameObject>> Spawned { get; set; } = new Subject<IEnumerable<GameObject>>();
        [SerializeField, Editor_R] GameObject[] objects;
        [SerializeField, Editor_R] Transform parent;
        [SerializeField, Tweakable] bool useSpawnerTransform = true;

        [SerializeField, Tweakable]
        [Button]
        public void Spawn()
        {
            if (objects.Length == 0)
                return;

            var spawnedPrefabs = new List<GameObject>(objects.Length);

            var goodObjects = objects.Where(x => x != null);

            foreach (var x in goodObjects)
            {
                var spawned = Instantiate(x,
                                          useSpawnerTransform ? transform.position : x.transform.position,
                                          useSpawnerTransform ? transform.rotation : x.transform.rotation,
                                          parent);

                spawnedPrefabs.Add(spawned);
            }

            Spawned.OnNext(spawnedPrefabs);
        }

        public void Spawn(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Spawn();
            }
        }

        public void OnValidate()
        {
            if (parent == null)
                parent = transform;
        }
    }
}