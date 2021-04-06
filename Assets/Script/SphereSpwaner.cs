




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpwaner : MonoBehaviour
{
    // public GameObject cubePrefab;
    int i = 0;
    private void FixedUpdate()
    {
        // Instantiate(cubePrefab, transform.position, Quaternion.identity);


        //if(Input.GetKeyDown(KeyCode.Space)){
        i++;
        if (i % 10 == 0)
        {
            ObjectPooler.instance.SpawnFromPool("Sphere", transform.position, Quaternion.identity);

            i = 0;
        }

        // }
    }

}

