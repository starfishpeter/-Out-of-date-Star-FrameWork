using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换
/// </summary>
public class ScenesManager : BaseManager<ScenesManager>
{
    
    /// <summary>
    /// 同步加载
    /// </summary>
    /// <param name="name">场景名</param>
    /// <param name="action">action委托</param>
    public void LoadScene(string name, UnityAction action)
    {
        SceneManager.LoadScene(name);
        action();
    }

    /// <summary>
    /// 同步加载无后续逻辑
    /// </summary>
    /// <param name="name">场景名</param>
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// 异步加载
    /// </summary>
    /// <param name="name">场景名</param>
    /// <param name="action">action委托</param>
    public void LoadSceneAsync(string name, UnityAction action)
    {
        MonoManager.GetInstance().StartCoroutine(IELoadSceneAsync(name, action));    
    }

    /// <summary>
    /// 异步加载无后续逻辑
    /// </summary>
    /// <param name="name"></param>
    public void LoadSceneAsync(string name)
    {
        MonoManager.GetInstance().StartCoroutine(IELoadSceneAsync(name));
    }

    /// <summary>
    /// 协程异步加载
    /// </summary>
    /// <param name="name">场景名</param>
    /// <param name="action">action委托</param>
    /// <returns></returns>
    IEnumerator IELoadSceneAsync(string name, UnityAction action)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);

        while(!ao.isDone)
        {
            //在这里更新进度条 事件中心添加触发 外面想用就用
            EventCenter.GetInstance().EventTrigger("Loading", ao.progress);
            yield return ao.progress;
        }

        //加载完成后才会去执行action
        action();
    }

    /// <summary>
    /// 协程异步加载 无后续逻辑
    /// </summary>
    /// <param name="name">场景名</param>
    /// <returns></returns>
    IEnumerator IELoadSceneAsync(string name)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);

        while (!ao.isDone)
        {
            //在这里更新进度条 事件中心添加触发 外面想用就用
            EventCenter.GetInstance().EventTrigger("Loading", ao.progress);
            yield return ao.progress;
        }
    }
}
