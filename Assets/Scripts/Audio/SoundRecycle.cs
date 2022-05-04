using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRecycle : MonoBehaviour
{
    void Update()
    {
        if (!this.GetComponent<AudioSource>().isPlaying)
        {
            PoolManager.GetInstance().PushObj(this.name, this.gameObject);
        }
    }
}
