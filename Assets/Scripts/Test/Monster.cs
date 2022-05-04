using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    void Start()
    {
        Die();
    }

    public void Die()
    {
        Debug.Log("怪物死亡");
        //触发事件
        EventCenter.GetInstance().EventTrigger("MonsterDie", this);
    }

}
