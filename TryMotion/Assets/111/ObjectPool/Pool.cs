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
                ClearAllAssetObject();
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
        public float autoDestroy;//递减销毁时间
        [SerializeField] int freeObjCount = 0;
        int natNumber = 0;//自然递增数值


        //对象池-根
        public Transform BasePoolParent { get; set; }
        Transform currentPoolParent;

        public Queue<GameObject> poolQueue;

        public void Clear()
        {
            if (poolQueue != null)
            {
                poolQueue.Clear();
                freeObjCount = 0;
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
            if (poolQueue == null || poolQueue.Count <= 0)
            {
                poolQueue = new Queue<GameObject>();

                for (int i = 0; i < InitPreAmount; i++)
                {
                    GameObject go = GameObject.Instantiate(pref);
                    string oriName = go.name;
                    natNumber++;
                    oriName += "#" + natNumber;
                    go.name = oriName;
                    //todo 注册回收事件
                    Register_PoolRecycleEvent(go);
                    poolQueue.Enqueue(go);
                    go.transform.SetParent(currentPoolParent , false);
                    go.SetActive(false);
                }

                freeObjCount = InitPreAmount;
            }
            return this;
        }

        /// <summary>
        /// 复用
        /// </summary>
        public GameObject Recycle()
        {
            GameObject go = null;
            go = poolQueue.Dequeue();
            while (freeObjCount > 0)
            {
                if (!go.activeInHierarchy)
                {
                    go.SetActive(true);
                    freeObjCount--;
                    //Debug.LogError(go.name);
                    poolQueue.Enqueue(go);
                    return go;
                }
                else
                {
                    poolQueue.Enqueue(go);
                    go = poolQueue.Dequeue();
                    while (!go.activeInHierarchy)
                    {
                        poolQueue.Enqueue(go);
                        go = poolQueue.Dequeue();
                    }
                    if (!go.activeInHierarchy) 
                    {
                        go.SetActive(true);
                        freeObjCount--;
                        Debug.LogError(go.name);
                        poolQueue.Enqueue(go);
                    }
                    return go;
                }
            }
            if (freeObjCount <= 0)
            {
                go = null;
                //自动增加
                if (isAutoIncrease)
                {
                    go = GameObject.Instantiate(pref);
                    string oriName = go.name;
                    natNumber++;
                    oriName += "#"+natNumber;
                    go.name = oriName;
                    //todo 注册回收事件
                    Register_PoolRecycleEvent(go);
                    poolQueue.Enqueue(go);
                    go.SetActive(true);
                    go.transform.SetParent(currentPoolParent , false);
                }
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
                    //Debug.LogError("<color=#00ff00>entityGo.name = </color> " + entityGo.name);
                    string entityName = entityGo.name.Split('#')[0];
                    if (entityName.Equals(prefName + "(Clone)" , System.StringComparison.InvariantCultureIgnoreCase))//这里大小写都不需要区分.
                    {
                        freeObjCount++;
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

