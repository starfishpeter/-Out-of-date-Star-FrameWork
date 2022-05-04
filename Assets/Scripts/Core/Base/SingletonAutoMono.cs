using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//继承这种自动创建的单例模式基类
//不需要我们手动去拖或者api加了
//直接GetInstance
public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        if (instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = typeof(T).ToString();
            //自动创建一个空对象 加一个单例模式脚本

            //切换场景 对象会被删除 所以存在问题
            //所以过场景 要确保不被移除
            //单例模式对象往往是存在整个程序生命周期的
            GameObject.DontDestroyOnLoad(obj);

            instance = obj.AddComponent<T>();
        }
        return instance;
    }

}
