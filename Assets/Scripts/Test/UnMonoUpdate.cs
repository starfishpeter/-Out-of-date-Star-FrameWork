using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test
{ 
    public Test()
    {
        MonoManager.GetInstance().StartCoroutine(TestCroutine());
        //MonoManager.GetInstance().StopCoroutine("TestCroutine");
        //不可用 怀疑是必须管理类下的协程才可通过传入函数名开启
    }

    public void Update()
    {
        Debug.Log("非继承Mono的Update");
    }

    IEnumerator TestCroutine()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("非Mono的协程测试");
    }

}

public class UnMonoUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Test t = new Test();
        MonoManager.GetInstance().AddUpdateListener(t.Update);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
