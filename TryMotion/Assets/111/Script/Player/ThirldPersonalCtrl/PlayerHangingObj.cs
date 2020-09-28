/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-28 11:56:54
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangingObj : MonoBehaviour
{
    [Header("挂载容器")]
    public List<Transform> hangingContainer = new List<Transform>();

    void Start()
    {
        HangingObj[] objs = GetComponentsInChildren<HangingObj>(true);
        for (int i = 0; i < objs.Length; i++)
        {
            hangingContainer.Add(objs[i].transform);
        }
    }

    void Update()
    {
        
    }
}
