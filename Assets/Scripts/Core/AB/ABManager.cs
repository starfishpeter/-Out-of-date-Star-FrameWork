using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABManager : SingletonAutoMono<ABManager>
{
    //AB包不能重复加载 所以这里用字典来存储加载过的AB包
    private Dictionary<string, AssetBundle> ABDic = new Dictionary<string, AssetBundle>();

    //主包
    private AssetBundle mainAB = null;
    private AssetBundleManifest manifest = null;

    /// <summary>
    /// AB包存放路径 方便外界修改
    /// </summary>
    private string Path_Url
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }

    /// <summary>
    /// 主包名 会根据平台发生变化
    /// </summary>
    private string MainAB_Name
    {
        get
        {
            #if UNITY_IOS
                return "IOS";
            #elif UNITY_ANDROID
                return "Android"
            #else
                return "PC";
            #endif
        }
    }

    /// <summary>
    /// 加载依赖
    /// </summary>
    /// <param name="ABName">目标包名</param>
    public void LoadDependencies(string ABName)
    {
        AssetBundle ab = null;

        //加载主包
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(Path_Url + MainAB_Name);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        //加载依赖包
        string[] strs = manifest.GetAllDependencies(ABName);
        for (int i = 0; i < strs.Length; i++)
        {
            if (!ABDic.ContainsKey(strs[i]))
            {
                AssetBundle abDepend = AssetBundle.LoadFromFile(Path_Url + strs[i]);
                ABDic.Add(strs[i], abDepend);
            }
        }

        //加载目标资源包
        if (!ABDic.ContainsKey(ABName))
        {
            ab = AssetBundle.LoadFromFile(Path_Url + ABName);
            ABDic.Add(ABName, ab);
        }
    }

    /// <summary>
    /// 同步加载AB包资源
    /// </summary>
    /// <param name="ABName">AB包名</param>
    /// <param name="resName">资源名</param>
    public object LoadRes(string ABName, string resName)
    {
        LoadDependencies(ABName);
        //加载资源
        Object obj = ABDic[ABName].LoadAsset(resName);
        if(obj == gameObject)
        {
            return Instantiate(obj);
        }
        else
        {
            return obj;
        }
    }

    /// <summary>
    /// 同步加载 并且指定类型
    /// </summary>
    /// <param name="ABName"></param>
    /// <param name="resName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public object LoadRes(string ABName, string resName, System.Type type)
    {
        LoadDependencies(ABName);
        //加载资源
        Object obj = ABDic[ABName].LoadAsset(resName, type);
        if (obj == gameObject)
        {
            return Instantiate(obj);
        }
        else
        {
            return obj;
        }
    }

    public T LoadRes<T>(string ABName, string resName)
    {
        LoadDependencies(ABName);
        //加载资源
        return null;
    }

    /// <summary>
    /// 单个卸载
    /// </summary>
    /// <param name="ABName">要卸载的包名</param>
    public void UnLoad(string ABName)
    {

    }

    /// <summary>
    /// 清空所有加载的AB包
    /// </summary>
    public void ClearAB()
    {

    }
}
