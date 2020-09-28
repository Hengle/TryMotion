/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-25 16:54:41
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickMgr : MonoBehaviour
{
    #region 单例

    private static JoyStickMgr _instance;
    public static JoyStickMgr Instance 
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<JoyStickMgr>();
            }
            return _instance;
        }
    }

    #endregion


    [Header("按钮")]
    public Transform joystick_MoveCtrl;
    public Transform joystick_AttackCtrl;
    public Transform joystick_JumpCtrl;

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
