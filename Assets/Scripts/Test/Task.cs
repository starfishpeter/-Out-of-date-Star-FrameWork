using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    void Awake()
    {
        EventCenter.GetInstance().AddEventListener<Monster>("MonsterDie", TaskDone);       
    }

    void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener<Monster>("MonsterDie", TaskDone);
    }

    public void TaskDone(Monster info)
    {
        Debug.Log("任务完成");
    }
}
