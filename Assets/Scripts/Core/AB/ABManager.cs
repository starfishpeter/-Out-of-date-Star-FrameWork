using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    /// <summary>
    /// 同步加载 根据泛型指定类型
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="ABName">包名</param>
    /// <param name="resName">资源名</param>
    /// <returns></returns>
    public T LoadRes<T>(string ABName, string resName) where T: Object
    {
        LoadDependencies(ABName);
        //加载资源
        T obj = ABDic[ABName].LoadAsset<T>(resName);
        if (obj == gameObject)
        {
            return Instantiate(obj);
        }
        else
        {
            return obj;
        }
    }

    /// <summary>
    /// 异步加载AB包资源
    /// </summary>
    /// <param name="ABName">包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="callBack">回调函数</param>
    public void LoadResAsync(string ABName, string resName, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(ABName, resName, callBack));
    }


    public void LoadResAsync(string ABName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(ABName, resName, type, callBack));
    }

    public void LoadResAsync<T>(string ABName, string resName, UnityAction<T> callBack) where T : Object
    {
        StartCoroutine(ReallyLoadResAsync<T>(ABName, resName, callBack));
    }

    private IEnumerator ReallyLoadResAsync(string ABName, string resName, UnityAction<Object> callBack)
    {
        LoadDependencies(ABName);
  
        AssetBundleRequest abRequest = ABDic[ABName].LoadAssetAsync(resName);
        yield return abRequest;

        if (abRequest.asset == gameObject)
        {
            callBack(Instantiate(abRequest.asset));
        }
        else
        {
            callBack(abRequest.asset);
        }
    }

    private IEnumerator ReallyLoadResAsync(string ABName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        LoadDependencies(ABName);

        AssetBundleRequest abRequest = ABDic[ABName].LoadAssetAsync(resName, type);
        yield return abRequest;

        if (abRequest.asset == gameObject)
        {
            callBack(Instantiate(abRequest.asset));
        }
        else
        {
            callBack(abRequest.asset);
        }
    }

    private IEnumerator ReallyLoadResAsync<T>(string ABName, string resName, UnityAction<T> callBack) where T: Object
    {
        LoadDependencies(ABName);

        AssetBundleRequest abRequest = ABDic[ABName].LoadAssetAsync<T>(resName);
        yield return abRequest;

        if (abRequest.asset == gameObject)
        {
            callBack(Instantiate(abRequest.asset) as T);
        }
        else
        {
            callBack(abRequest.asset as T);
        }
    }

    /// <summary>
    /// 单个卸载
    /// </summary>
    /// <param name="ABName">要卸载的包名</param>
    public void UnLoad(string ABName)
    {
        if(ABDic.ContainsKey(ABName))
        {
            ABDic[ABName].Unload(false);
            ABDic.Remove(ABName);
        }
    }

    /// <summary>
    /// 清空所有加载的AB包
    /// </summary>
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        ABDic.Clear();
        mainAB = null;
        manifest = null;
    }
}
