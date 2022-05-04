using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GameObject obj = ResourceManager.GetInstance().Load<GameObject>("Test/Cube");
        ResourceManager.GetInstance().LoadAsync<GameObject>("Test/Cube", (obj) => {
            obj.transform.localScale = Vector3.one * 2;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
