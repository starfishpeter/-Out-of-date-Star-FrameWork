using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载
/// </summary>
public class ResourceManager : BaseManager<ResourceManager>
{
    /// <summary>
    /// 同步加载
    /// </summary>
    /// <param name="name">资源路径</param>
    public T Load<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);

        //如果是GameObject先实例化再返回
        if(res is GameObject)
        {
            return GameObject.Instantiate(res);
        }

        return res;
    }

    /// <summary>
    /// 异步加载
    /// </summary>
    /// <typeparam name="T">加载的资源类型</typeparam>
    /// <param name="name">资源路径</param>
    /// <param name="action">返回的资源</param>
    public void LoadAsync<T>(string name, UnityAction<T> action) where T : Object
    {
        MonoManager.GetInstance().StartCoroutine(IELoadAsync<T>(name,action));
    }

    /// <summary>
    /// 协程异步加载
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator IELoadAsync<T>(string name, UnityAction<T> action) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;

        if(r.asset is GameObject)
        {
            action(GameObject.Instantiate(r.asset) as T);
        }

        action(r.asset as T);
    }
}
