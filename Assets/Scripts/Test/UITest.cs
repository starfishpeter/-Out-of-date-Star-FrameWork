using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITest : BasePanel
{
    protected override void Awake()
    {
        base.Awake();//一定要保留这句 才能确保控件监听的存在
    }

    void Start()
    {
        UIManager.AddCustomEventListener(GetUIComponent<Button>("Start"), EventTriggerType.PointerEnter, (data) =>
        {
            Debug.Log("鼠标进入按钮");
        });

        UIManager.AddCustomEventListener(GetUIComponent<Button>("Start"), EventTriggerType.PointerExit, (data) =>
        {
            Debug.Log("鼠标离开按钮");
        });
    }

    public override void UIComponentOn()
    {
        base.UIComponentOn();
        //在这写入面板显示时要执行的逻辑
    }

    public override void UIComponentOff()
    {
        base.UIComponentOff();
        //在这写入面板隐藏时要执行的逻辑
    }

    protected override void OnClick(string btnName)
    {
        switch(btnName)
        {
            case "Start":
                Debug.Log("游戏开始");
                break;
            case "End":
                Debug.Log("游戏结束");
                break;
        }
        
    }

    public void InitPanel()
    {
        Debug.Log("面板已经生成 处理这的逻辑");
        Debug.Log(this.gameObject.name);
        //Invoke("DelayHide", 5f);
    }

    public void DelayHide()
    {
        UIManager.GetInstance().HidePanel(this.name);
    }

}
