using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L_Pool 
{
    /// <summary>
    /// 所有对象池管理
    /// </summary>
    public class AllPoolMgr : MonoBehaviour
    {
        #region 单例

        private static AllPoolMgr _instance;
        public static AllPoolMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AllPoolMgr>();
                }
                return _instance;
            }
        }

        #endregion

        [Header("对象池")]
        public string poolContent;
        public Pool bulletPool;


        void Awake()
        {
            bulletPool.ClearAllAssetObject();
        }

    }
}