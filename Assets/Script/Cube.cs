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
