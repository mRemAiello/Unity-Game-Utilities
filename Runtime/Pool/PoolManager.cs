using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class PoolManager : Singleton<PoolManager>
    {
        [SerializeField, ReadOnly] private Dictionary<string, Queue<GameObject>> _poolDict;
        [SerializeField] private List<Pool> _pools;

        protected override void OnPostAwake()
        {
            // Inizializzazione del dizionario dei pool
            _poolDict = new Dictionary<string, Queue<GameObject>>();

            // Creazione e popolamento dei pool
            foreach (Pool pool in _pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.Size; i++)
                {
                    GameObject obj = Instantiate(pool.Prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                _poolDict.Add(pool.Tag, objectPool);
            }
        }

        public GameObject GetObjectFromPool(string tag)
        {
            if (!_poolDict.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
                return null;
            }

            GameObject objectToSpawn = _poolDict[tag].Dequeue();
            objectToSpawn.SetActive(true);
            _poolDict[tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }

        public void AddObjectsToPool(string tag, int count)
        {
            if (!_poolDict.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
                return;
            }

            Pool pool = _pools.Find(p => p.Tag == tag);
            if (pool == null)
            {
                Debug.LogWarning("Pool with tag " + tag + " not found in pools list.");
                return;
            }

            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(pool.Prefab);
                obj.SetActive(false);
                _poolDict[tag].Enqueue(obj);
            }

            Debug.Log(count + " objects added to pool with tag " + tag);
        }
    }
}
