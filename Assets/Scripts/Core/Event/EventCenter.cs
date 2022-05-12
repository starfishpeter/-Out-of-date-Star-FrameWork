using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface I_EventGeneric
{ 
    
}

public class EventGeneric<T> : I_EventGeneric
{
    public UnityAction<T> actions;

    public EventGeneric(UnityAction<T> action)
    {
        actions += action;
    }
}

public class EventGeneric : I_EventGeneric
{
    public UnityAction actions;

    public EventGeneric(UnityAction action)
    {
        actions += action;
    }
}

/// <summary>
/// 事件中心 单例
/// 事件触发要晚于事件监听
/// </summary>
public class EventCenter : BaseManager<EventCenter>
{
    //存放事件的容器 Key是事件名 value是监听事件对应的委托函数们
    private Dictionary<string, I_EventGeneric> eventDic = new Dictionary<string, I_EventGeneric>();

    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="action">处理事件的委托函数</param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        //有没有对应的事件监听
        if(eventDic.ContainsKey(name))
        {
            //有就直接添加
            (eventDic[name] as EventGeneric<T>).actions += action;
        }
        else
        {
            //没有就要创建
            eventDic.Add(name, new EventGeneric<T>(action));
        }
    }

    /// <summary>
    /// 移除事件监听
    /// </summary>
    /// <param name="name">移除的事件名</param>
    /// <param name="action">事件添加的委托函数</param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventGeneric<T>).actions -= action;
        }
        //销毁时调用 OnDestroy()
    }

    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="name">触发的事件名</param>
    public void EventTrigger<T>(string name, T info)
    {
        if (eventDic.ContainsKey(name) && (eventDic[name] as EventGeneric<T>).actions != null) 
        {
            (eventDic[name] as EventGeneric<T>).actions.Invoke(info);
        }
    }

    /// <summary>
    /// 清空事件中心 防止场景切换时溢出
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
