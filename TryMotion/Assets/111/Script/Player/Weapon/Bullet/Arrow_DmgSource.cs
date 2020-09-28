/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-28 13:07:15
*      修改记录：
*      版 本：1.0
 ========================================================*/
using Cinemachine;
using L_Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_DmgSource : MonoBehaviour
{
    private CinemachineImpulseSource source;

    private void Awake()
    {
        source = GetComponent<CinemachineImpulseSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            GetComponentInParent<PoolRecycle>().RecycleEvent(transform.parent.gameObject);
            this.transform.parent.gameObject.SetActive(false);
            source.GenerateImpulse(Camera.main.transform.forward);
        }
    }

}
