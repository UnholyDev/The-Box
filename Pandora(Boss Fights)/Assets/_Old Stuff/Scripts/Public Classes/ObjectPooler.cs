using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    Dictionary<string, Queue<GameObject>> poolDictionary;

    #region Singleton

    public static ObjectPooler Instance;

    void Awake()
    {
        Instance = this;
    }

    #endregion

    // Use this for initialization
    void Start () {

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++)
            {
                //Instantiate all the objects and set their parents to this transfom
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                poolQueue.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, poolQueue);
        }

	}

    public GameObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("No object with tag " + tag + " within poolDictionary.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    public void ReturnObjectToQueue(GameObject obj, string tag)
    {
        poolDictionary[tag].Enqueue(obj);
        obj.SetActive(false);
    }
}
