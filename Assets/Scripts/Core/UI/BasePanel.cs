using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 面板基类 能找到自己面板下的所有控件对象
/// 提供显示或者隐藏的接口
/// </summary>
public class BasePanel : MonoBehaviour
{
    //里氏替换原则 存放UI组件
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

    protected virtual void Awake()
    {
        FindChildrenUIComponents<Image>();
        FindChildrenUIComponents<Text>();
        FindChildrenUIComponents<Button>();
        FindChildrenUIComponents<Slider>();
        FindChildrenUIComponents<Toggle>();
        FindChildrenUIComponents<ScrollRect>();
        FindChildrenUIComponents<InputField>(); 
        FindChildrenUIComponents<ToggleGroup>();
    }

    /// <summary>
    /// 显示面板时处理的逻辑 可以在子类重写
    /// </summary>
    public virtual void UIComponentOn()
    {

    }

    /// <summary>
    /// 隐藏面板时处理的逻辑 可以在子类重写
    /// </summary>
    public virtual void UIComponentOff()
    {

    }    

    /// <summary>
    /// 按钮点击事件 可以在子类检测
    /// </summary>
    /// <param name="btnName">按钮的名字</param>
    protected virtual void OnClick(string btnName)
    {

    }

    /// <summary>
    /// 单选和多选的点击事件 可以在子类检测并重写
    /// </summary>
    /// <param name="toggleName">单选框的名字</param>
    /// <param name="value">单选框的状态</param>
    protected virtual void OnValueChange(string toggleName, bool value)
    {

    }    

    /// <summary>
    /// 得到对应名字的UI控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T GetUIComponent<T>(string name) where T : UIBehaviour
    {
        if(controlDic.ContainsKey(name))
        {
            for (int i = 0; i < controlDic[name].Count; i++)
            {
                if(controlDic[name][i] is T)
                {
                    return controlDic[name][i] as T;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// 找到所有子物体下的该类型的组件放入字典
    /// </summary>
    /// <typeparam name="T">UI类型</typeparam>
    private void FindChildrenUIComponents<T>() where T : UIBehaviour
    {
        T[] components = this.GetComponentsInChildren<T>();
        
        for(int i = 0; i < components.Length; i++)
        {
            string objName = components[i].name;//小心闭包

            if (controlDic.ContainsKey(objName))
            {
                controlDic[objName].Add(components[i]);
            }
            else
            {
                controlDic.Add(objName, new List<UIBehaviour> { components[i] });
            }

            //按钮处理逻辑
            if(components[i] is Button)
            {
                (components[i] as Button).onClick.AddListener(() =>
                {
                    OnClick(objName);
                });
            }
            //单选或多选框处理逻辑
            else if (components[i] is Toggle)
            {
                (components[i] as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnValueChange(objName, value);
                });
            }
        }
    }
}
