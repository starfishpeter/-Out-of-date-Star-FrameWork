using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum E_UI_Layer
{
    Bottom,
    Middle,
    Top,
    System
}

/// <summary>
/// UI管理器
/// 管理所有显示的面板
/// 提供给外部显示和隐藏等接口
/// </summary>
public class UIManager : BaseManager<UIManager>
{
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform bottom;
    private Transform middle;
    private Transform top;
    private Transform system;

    //记录UI canvas父对象 方便以后外部使用
    public RectTransform canvas;

    public UIManager()
    {
        //动态加载Canvas 确保过场景不被移除
        GameObject obj = ResourceManager.GetInstance().Load<GameObject>("UI/Canvas");
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);

        //找到各层
        bottom = canvas.Find("Bottom");
        middle = canvas.Find("Middle");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        //动态加载EventSystem 确保过场景不被移除
        GameObject eventSystem = ResourceManager.GetInstance().Load<GameObject>("UI/EventSystem");
        GameObject.DontDestroyOnLoad(eventSystem);
    }

    /// <summary>
    /// 得到对应层级的父对象
    /// </summary>
    /// <param name="layer">层级</param>
    /// <returns></returns>
    public Transform GetLayerRoot(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bottom: 
                return bottom;
            case E_UI_Layer.Middle:
                return middle;
            case E_UI_Layer.Top:
                return top;
            case E_UI_Layer.System:
                return system;
        }
        return null;
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板本身</typeparam>
    /// <param name="name">面板名</param>
    /// <param name="layer">显示层级</param>
    /// <param name="callBack">面板创建后处理的逻辑 可置空</param>
    public void ShowPanel<T>(string name, E_UI_Layer layer = E_UI_Layer.Middle, UnityAction<T> callBack = null) where T : BasePanel
    {
        ResourceManager.GetInstance().LoadAsync<GameObject>("UI/" + name, (panel) =>
         {
             if (panelDic.ContainsKey(name)) 
             {
                 panelDic[name].UIComponentOn();

                 //重复加载直接跳过异步加载 来执行回调函数
                 if (callBack != null)
                 {
                     callBack(panelDic[name] as T);
                 }

                 return;
             }

             //作为Canvas四个层级中某一层的子对象 并设置相对位置
             Transform root = bottom;
             switch (layer)
             {
                case E_UI_Layer.Middle:
                     root = middle;
                     break;
                case E_UI_Layer.Top:
                     root = top;
                     break;
                case E_UI_Layer.System:
                     root = system;
                     break;
             }

             //初始化位置和大小
             panel.name = name;
             panel.transform.SetParent(root);
             panel.transform.localPosition = Vector3.zero;
             panel.transform.localScale = Vector3.one;
             (panel.transform as RectTransform).offsetMax = Vector2.zero;
             (panel.transform as RectTransform).offsetMin = Vector2.zero;

             //得到预设体身上的面板脚本
             T panelScript = panel.GetComponent<T>();
             //处理面板创建完成后的逻辑 然后存起来
             if(callBack != null)
             {
                 callBack(panelScript);
             }

             panelDic.Add(name, panelScript);

             //面板显示时处理的逻辑
             panelDic[name].UIComponentOn();
         });
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="name">面板名</param>
    public void HidePanel(string name)
    {
        if(panelDic.ContainsKey(name))
        {
            //面板隐藏时处理的逻辑
            panelDic[name].UIComponentOff();
            GameObject.Destroy(panelDic[name].gameObject);
            panelDic.Remove(name);
        }
    }

    /// <summary>
    /// 得到显示的面板
    /// </summary>
    /// <typeparam name="T">面板的类</typeparam>
    /// <param name="name">面板名</param>
    /// <returns></returns>
    public T GetPanel<T>(string name) where T : BasePanel
    {
        if(panelDic.ContainsKey(name))
        {
            return panelDic[name] as T;
        }
        return null;
    }

    /// <summary>
    /// 添加自定义监听
    /// </summary>
    /// <param name="uiComponent">UI控件</param>
    /// <param name="type">监听类型</param>
    /// <param name="action">回调函数</param>
    public static void AddCustomEventListener(UIBehaviour uiComponent, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = uiComponent.GetComponent<EventTrigger>();
        if(trigger == null)
        {
            trigger = uiComponent.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);

        trigger.triggers.Add(entry);
    }
}
