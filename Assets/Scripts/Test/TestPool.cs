using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPool : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PoolManager.GetInstance().GetObj("Test/Cube", (obj) => { 
                obj.transform.position = new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f),Random.Range(0f,10f));
            });
        }
        else if(Input.GetMouseButtonDown(1))
        {
            PoolManager.GetInstance().GetObj("Test/Sphere", (obj) => {
                obj.transform.position = new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f));
            });
        }
    }
}
