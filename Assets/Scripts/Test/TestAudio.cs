using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudio : MonoBehaviour
{
    private float volume = 1f;

    void Start()
    {
        //AudioManager.GetInstance().ChangeBGMVolume(0.5f);
        //AudioManager.GetInstance().BGMOn("110515");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.GetInstance().SoundPlay("true", false, null);
        }

        if(volume > 0f)
        {
            AudioManager.GetInstance().ChangeSoundVolume(volume -= Time.deltaTime/50);
        }
        
    }
}
