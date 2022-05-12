using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRecycle : MonoBehaviour
{

    public IEnumerator Recycle()
    {
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        PoolManager.GetInstance().PushObj(this.name, this.gameObject);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

}
