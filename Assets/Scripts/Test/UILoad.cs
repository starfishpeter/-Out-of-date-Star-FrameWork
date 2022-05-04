using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.GetInstance().ShowPanel<UITest>("TestUI", E_UI_Layer.Middle, (panel) =>
        {
            panel.InitPanel();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
