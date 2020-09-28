/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-28 11:25:49
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L_Pool 
{
    public class PoolSetActice : MonoBehaviour
    {
        public float DestoryTimer = 1;


        void OnEnable()
        {
            Invoke("Destory" , DestoryTimer);
        }

        void Destory()
        {
            this.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            CancelInvoke("Destory");
        }
    }

}