using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//把对象池本身和对象池里的分类分开来写
//对象池里的分类操作 
//对象池的操作

/// <summary>
/// 对象池里的某一分类
/// </summary>
public class PoolData
{
    //单个分类挂载的父节点 这样在unity数据显示中会体现层级
    public GameObject rootObj;
    //对象的分类容器
    public List<GameObject> poolList;

    /// <summary>
    /// 构造函数  
    /// </summary>
    /// <param name="obj">传入分类的名称</param>
    /// <param name="poolObj">分类的父节点（最外层的pool）</param>
    public PoolData(GameObject obj, GameObject poolObj)
    {
        //衣柜 塞 抽屉
        //pool下放入分类的文件夹
        rootObj = new GameObject(obj.name);
        rootObj.transform.parent = poolObj.transform;
        poolList = new List<GameObject>();
        PushObj(obj);
    }

    /// <summary>
    /// 往分类里放入对象
    /// </summary>
    /// <param name="obj">放入分类的对象</param>
    public void PushObj(GameObject obj)
    {
        obj.SetActive(false);//隐藏
        poolList.Add(obj);
        obj.transform.parent = rootObj.transform;
    }

    /// <summary>
    /// 从分类中取出对象
    /// </summary>
    /// <returns></returns>
    public GameObject GetObj()
    {
        GameObject obj = null;

        obj = poolList[0];
        poolList.RemoveAt(0);


        obj.SetActive(true);//记得一定要激活
        obj.transform.parent = null;//不显示在pool里的分类下 而是显示在外面

        return obj;
    }
}


public class PoolManager : BaseManager<PoolManager>
{
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    private GameObject poolObj;

    /// <summary>
    /// 从分类里异步加载对象
    /// </summary>
    /// <param name="name">分类名</param>
    /// <param name="action">action委托</param>
    public void GetObj(string name, UnityAction<GameObject> action)
    {
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0) 
        {
            action(poolDic[name].GetObj());
        }
        else
        {
            ResourceManager.GetInstance().LoadAsync<GameObject>(name, (obj) =>
            {
                obj.name = name;
                action(obj);
            });
        }
    }

    /// <summary>
    /// 放入对象到对应分类里
    /// </summary>
    /// <param name="name">分类名</param>
    /// <param name="obj">放入的对象</param>
    public void PushObj(string name, GameObject obj)
    {
        if(poolObj == null)
        {
            poolObj = new GameObject("Pool");
        }

        if(poolDic.ContainsKey(name))
        {
            poolDic[name].PushObj(obj);
        }
        else
        {
            //没有对应的分类
            poolDic.Add(name, new PoolData(obj,poolObj));
        }
    }

    /// <summary>
    /// 清空缓存池 防止在场景切换的时候溢出
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
