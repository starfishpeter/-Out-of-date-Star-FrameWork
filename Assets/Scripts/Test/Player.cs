using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        EventCenter.GetInstance().AddEventListener<Monster>("MonsterDie", GetBonus);        
    }

    void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener<Monster>("MonsterDie", GetBonus);
    }

    public void GetBonus(Monster info)
    {
        Debug.Log(info.gameObject.name +"死了，增加金币");
    }    
}
