using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestSceneChange : MonoBehaviour
{

    void Start()
    {
        //ScenesManager.GetInstance().LoadScene("Test2");

        //ScenesManager.GetInstance().LoadSceneAsync("Test2", () =>
        //{
        //    Debug.Log("³¡¾°ÇÐ»»");
        //});

        ScenesManager.GetInstance().LoadSceneAsync("Test2");
    }

    void Update()
    {
        
    }
}
