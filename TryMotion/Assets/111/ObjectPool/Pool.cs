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
    /// <summary>
    /// 池基类
    /// <para>注意,每次用Pool的时候.都先 调用一下方法.ClearAllAssetObject()</para>
    /// </summary>
    [CreateAssetMenu(fileName = "defaultPool" , menuName = "Create/objectPool" , order = -500)]
    public class Pool : ScriptableObject
    {
        [TextArea()]
        public string poolContent;

        public ObjectPool[] objectPool;

        GameObject rootPoolParent;
        GameObject dontDestory;


        /// <summary>
        /// 必须一开始就清除  asset资源一下
        /// </summary>
        public void ClearAllAssetObject()
        {
            if (objectPool != null)
            {
                for (int i = 0; i < objectPool.Length; i++)
                {
                    objectPool[i].Clear();
                }
            }
        }

        /// <summary>
        /// 通过[预制体名称]找到对应的对象池[类].
        /// </summary>
        /// <param name="prefName">预制体名称</param>
        /// <returns></returns>
        public ObjectPool FindCorrespondingPoolByPrefName(string prefName)
        {
            if (rootPoolParent == null)
            {
                rootPoolParent = new GameObject(poolContent + "--根池～～～～rootPoolParent");
                rootPoolParent.transform.SetParent(GameObject.FindGameObjectWithTag("DontDestroyOnLoad").transform);
            }

            for (int i = 0; i < objectPool.Length; i++)
            {
                if (objectPool[i].prefName.Equals(prefName))
                {
                    ObjectPool temp = null;
                    temp = objectPool[i].Init(rootPoolParent.transform);
                    return temp;
                }
            }
            return null;
        }

    }


    /// <summary>
    /// 对象池
    /// </summary>
    [System.Serializable]
    public class ObjectPool
    {
        [Space(10)]
        public string prefName;
        [TextArea()]
        public string prefConent;
        public GameObject pref;     //克隆预制体
        public int InitPreAmount = 5;      //预先加载对象池容量
        public bool isAutoIncrease = true; //是否自动增长


        //对象池-根
        public Transform BasePoolParent { get; set; }
        Transform currentPoolParent;

        public List<GameObject> pooledList;

        public void Clear()
        {
            if (pooledList != null)
            {
                pooledList.Clear();
            }
        }


        /// <summary>
        /// 初始化
        /// </summary>
        public ObjectPool Init(Transform basePoolParent)
        {
            this.BasePoolParent = basePoolParent;
            //设置 对象池 挂载父类
            if (currentPoolParent == null)
            {
                currentPoolParent = new GameObject(prefConent).transform;
                currentPoolParent.SetParent(BasePoolParent , false);
            }

            if (InitPreAmount <= 0) { Debug.LogError("注意,InitPreAmount 不能小于0!"); return this; }
            if (pref == null || string.IsNullOrEmpty(prefName)) { Debug.LogError(string.Format("对象池,prefName={0}" , prefName) + "对象池为空!!!"); return this; }

            //初始化
            if (pooledList == null || pooledList.Count <= 0)
            {
                pooledList = new List<GameObject>();

                for (int i = 0; i < InitPreAmount; i++)
                {
                    GameObject go = GameObject.Instantiate(pref);
                    //todo 注册回收事件
                    Register_PoolRecycleEvent(go);
                    pooledList.Add(go);
                    go.transform.SetParent(currentPoolParent , false);
                    go.SetActive(false);
                }
            }
            return this;
        }

        /// <summary>
        /// 复用
        /// </summary>
        public GameObject Recycle()
        {
            GameObject go = null;
            for (int i = 0; i < pooledList.Count; i++)
            {
                if (pooledList[i] == null) { pooledList.RemoveAt(i); continue; }
                if (!pooledList[i].activeInHierarchy) //不在场景中显示,就可以复用!
                {
                    go = pooledList[i];
                    go.SetActive(true);
                    return go;
                }
            }

            //自动增加
            if (isAutoIncrease)
            {
                go = GameObject.Instantiate(pref);
                //todo 注册回收事件
                Register_PoolRecycleEvent(go);
                pooledList.Add(go);
                go.SetActive(true);
                go.transform.SetParent(currentPoolParent , false);
                return go;
            }

            return go;
        }

        /// <summary>
        /// 针对粒子-复用
        /// </summary>
        public GameObject Recycle2Particle(GameObject TargetGO , float scaleFactor = 1)
        {
            GameObject go = null;
            for (int i = 0; i < pooledList.Count; i++)
            {
                if (pooledList[i] == null) { pooledList.RemoveAt(i); continue; }
                if (!pooledList[i].activeInHierarchy) //不在场景中显示,就可以复用!
                {
                    go = pooledList[i];
                    go.SetActive(true);
                    if (go.GetComponent<ParticleScaleFactor>() == null) { go.AddComponent<ParticleScaleFactor>(); }
                    go.GetComponent<ParticleScaleFactor>().Scale2Amp(scaleFactor);
                    go.GetComponent<ParticleScaleFactor>()._Target = TargetGO;

                    return go;
                }
            }

            //自动增加
            if (isAutoIncrease)
            {
                go = GameObject.Instantiate(pref);
                //todo 注册回收事件
                Register_PoolRecycleEvent(go);
                pooledList.Add(go);
                go.SetActive(true);
                if (go.GetComponent<ParticleScaleFactor>() == null) { go.AddComponent<ParticleScaleFactor>(); }
                go.GetComponent<ParticleScaleFactor>().Scale2Amp(scaleFactor);
                go.GetComponent<ParticleScaleFactor>()._Target = TargetGO;
                go.transform.SetParent(currentPoolParent , false);
                return go;
            }

            return go;
        }

        /// <summary>
        /// 注册 回收事件
        /// </summary>
        void Register_PoolRecycleEvent(GameObject go)
        {
            if (go.GetComponent<PoolRecycle>() == null) { go.AddComponent<PoolRecycle>(); }

            //注册 回收事件.
            if (go.GetComponent<PoolRecycle>())
            {
                go.GetComponent<PoolRecycle>().RecycleEvent = (GameObject entityGo) =>
                {
                    //Debug.LogError("entityGo.name = " + entityGo.name);
                    if (entityGo.name == prefName + "(Clone)")
                    {
                        entityGo.transform.SetParent(currentPoolParent , false);
                        entityGo.transform.localPosition = Vector3.zero;
                        entityGo.transform.localEulerAngles = Vector3.zero;
                        entityGo.SetActive(false);
                    }
                };
            }
            //测试用的(方便直接获得当前对象)
            go.GetComponent<PoolRecycle>().CurrentEntityGo = go;
        }
    }
}

