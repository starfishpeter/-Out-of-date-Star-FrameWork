using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mono的管理者 不能被直接使用
/// </summary>
public class MonoController : MonoBehaviour
{

    private event UnityAction updateEvent;

    void Start()
    {
        DontDestroyOnLoad(this);
    }


    void Update()
    {
        if(updateEvent != null)
        {
            updateEvent.Invoke();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// 添加帧更新事件
    /// </summary>
    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }

    /// <summary>
    /// 移除帧更新事件
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }
}
