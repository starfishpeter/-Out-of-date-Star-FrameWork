using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{

    void Start()
    {
        //AudioManager.GetInstance().ChangeBGMVolume(0.5f);
        //AudioManager.GetInstance().BGMOn("110515");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.GetInstance().ChangeSoundVolume(0.5f);
            AudioManager.GetInstance().SoundPlay("ReloadAmmo", false, null);
        }
    }

}
