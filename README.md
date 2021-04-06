# Unity Object Pool
An object pool provides an efficient way to reuse objects, and thus keep the memory foot print of all dynamically created objects within fixed bounds. This is crucial for maintaining consistent frame rates in real time games (especially on mobile), as frequent garbage collection spikes would likely lead to inconsistent performance.

![](Doc/ObjecPool2.gif)
# Cube
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour, IPooledObject
{
    public float upForce = 1F;
    public float sideForce = 0.1f;
    public float lifeTime = 2.0f;
    private float spwanTime;
    public void OnObjectSpwan()
    {
     
        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(upForce/2f, upForce);
        float zForce = Random.Range(-sideForce, sideForce);

        Vector2 force = new Vector3(xForce, yForce, zForce);

        GetComponent<Rigidbody>().velocity = force;
    }
    private void Start()
    {
        spwanTime = 0;
    }
    private void Update()
    {
        spwanTime += Time.deltaTime;
        if(spwanTime > lifeTime)
        {
            gameObject.SetActive(false);
            spwanTime = 0;
        }
    }
}
```

# IPooledObject
```csharp
using UnityEngine;

public interface IPooledObject  
{
    void OnObjectSpwan();
}
```

# ObjectPooler
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
   
    [System.Serializable]
   public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    #region Singletoon
    public static ObjectPooler instance;
    private void Awake()
    {

        instance = this;
    }
    #endregion
    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);

        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag: {tag} does not exist");
            return null;
        }
         GameObject objectToSpawn =  poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject objectIsPooled = objectToSpawn.GetComponent<IPooledObject>();
        if(objectIsPooled != null)
        {
            objectIsPooled.OnObjectSpwan();
        }

        poolDictionary[tag].Enqueue(objectToSpawn);// enter again
        return objectToSpawn;

    }
}
```


# CubeSpawner
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    int i = 0;
    private void FixedUpdate()
    {
        i++;
        if (i % 10 == 0)
        {
            ObjectPooler.instance.SpawnFromPool("Cube", transform.position, Quaternion.identity);
            
            i = 0;
        }
    }
}
```
