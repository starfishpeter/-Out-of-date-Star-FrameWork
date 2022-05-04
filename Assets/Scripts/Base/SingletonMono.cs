using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        //重点：不能自己new 得通过u3d来实例化 也就是生命周期函数
        return instance;
    }

    protected virtual void Awake()
    {
        instance = this as T;
        //问题 重写Awake会有问题
        //解决 变成保护类型 虚函数
        //子类重写 必须要保留base.Awake();
    }

    //缺点 挂载多个 单例模式就被破坏了
    //只会关联最后一个 
}
