using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayRemove : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("Push", 2);
    }

    void Push()
    {
        PoolManager.GetInstance().PushObj(this.gameObject.name, this.gameObject);
    }    
}
