/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-28 13:04:58
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Bullet : MonoBehaviour
{
    public bool isMotion = false;
    private Arrow_DmgSource dmgSource;
    private Rigidbody rig;
    public float moveSpeed = 4;
    public float rotateSpeed_Z = 60;

    //private 
    float G = -9.821f;

    void Awake()
    {
        dmgSource = GetComponent<Arrow_DmgSource>();
        rig = GetComponentInChildren<Rigidbody>(true);
    }

    void Update()
    {
        if (isMotion)
        {
            rig.velocity = Vector3.down * Time.deltaTime * G;
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime , Space.World);
            transform.Rotate(new Vector3(0 , 0 , rotateSpeed_Z * Time.deltaTime),Space.Self);
        }
    }

    public void OpenMotion() 
    {
        transform.parent = null;
        isMotion = true;
    }

    public void CloseMotion() 
    {
        isMotion = false;
    }
}
