using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ObjectPool : MonoBehaviour
{
    [Serializable]
    public struct Pool
    {
        public Queue<GameObject> pooledObjects;
        public GameObject objectPrefab;
        public int poolSize;
    }
    [SerializeField] private Pool[] pools = null;
    private void Awake()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i].pooledObjects = new Queue<GameObject>();

            for (int j = 0; j < pools[i].poolSize; j++)
            {
                GameObject obj = Instantiate(pools[i].objectPrefab);
                obj.transform.parent = this.gameObject.transform;
                obj.SetActive(false);

                pools[i].pooledObjects.Enqueue(obj);
            }
        }
    }
    public GameObject GetPooledObject(int objetType)
    {
        if (objetType >= pools.Length)
        {
            return null;
        }
        GameObject obj = pools[objetType].pooledObjects.Dequeue();
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        obj.SetActive(true);

        pools[objetType].pooledObjects.Enqueue(obj);

        return obj;
    }
}
