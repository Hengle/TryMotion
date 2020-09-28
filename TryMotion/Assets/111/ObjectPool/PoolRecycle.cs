using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L_Pool 
{
    /// <summary>
    /// 对象池回收
    /// </summary>
    public class PoolRecycle : MonoBehaviour
    {
        public GameObject CurrentEntityGo;

        public delegate void Recycle(GameObject entityGo);//string recyclePrefName
        public Recycle RecycleEvent;

        ////对地形无效.所有注释掉这种方法!
        //private void OnDisable()
        //{
        //    //Invoke("DelayDisable", 0.1f);
        //    RecycleEvent?.Invoke(this.gameObject);
        //}

        //void DelayDisable()
        //{
        //    //Debug.LogError("禁用! + name =" + this.gameObject.name);
        //    RecycleEvent?.Invoke(this.gameObject);
        //}
    }
}