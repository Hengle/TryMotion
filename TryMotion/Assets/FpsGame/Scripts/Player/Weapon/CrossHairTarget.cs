using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    #region 单例

    private static CrossHairTarget _instance;
    public static CrossHairTarget Instance 
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CrossHairTarget>();
            }
            return _instance;
        }
    }

    #endregion

    private Transform oriTransform;
    
    void Awake()
    {
        oriTransform = Camera.main.transform.Find("CrossHairTarget").transform;
    }

    Ray ray;
    RaycastHit hitInfo;
    /// <summary>
    /// 检测十字瞄准目标
    /// <para>返回: 检测目标点</para>
    /// </summary>
    public Vector3 DetectCrossHairTarget() 
    {
        var tarPos = Vector3.zero;
        ray.origin = oriTransform.position;
        ray.direction = Camera.main.transform.forward;

        if (Physics.Raycast(ray,out hitInfo))
        {
            tarPos = hitInfo.point;
        }
        return tarPos;
    }
}
