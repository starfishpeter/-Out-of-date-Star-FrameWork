using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRecycle : MonoBehaviour
{

    public IEnumerator Recycle()
    {
        Debug.Log("回收开始");
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        Debug.Log("回收结束");
        PoolManager.GetInstance().PushObj(this.name, this.gameObject);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

}
