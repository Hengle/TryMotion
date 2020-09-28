using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterAttack : MonoBehaviour
{

    #region 属性

    private Transform atkPoint;
    public Transform AtkPoint
    {
        get
        {
            if (atkPoint == null)
            {
                atkPoint = transform.Find("atkPoint");
            }
            return atkPoint;
        }
    }

    #endregion


    public GameObject bulletPref;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //攻击
        if (InputHandleMgr.Instsance.GetFireInputDown())
        {
            GameObject go = Instantiate(bulletPref, AtkPoint.position, AtkPoint.rotation);
            go.GetComponent<Bullet>().atkerGo = this.gameObject;
        }
    }
}
