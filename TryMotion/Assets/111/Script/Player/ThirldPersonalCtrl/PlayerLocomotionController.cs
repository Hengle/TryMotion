/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-24 09:40:37
*      修改记录：
*      版 本：1.0
 ========================================================*/
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 玩家混合控制器
/// </summary>
public class PlayerLocomotionController : MonoBehaviour
{
    [Header("MainCamera")]
    [SerializeField] Camera _mainCam;

    [Header("cineMachine")]
    [SerializeField] GameObject _shotStart;
    [SerializeField] GameObject _shotAim;

    private void Awake()
    {

    }

    /// <summary>
    /// 利用CineMachine去实现,拉弓的视角缩放.
    /// </summary>
    public void BlendCineMachine_ShootView(bool isPull)
    {
        if (isPull)
        {
            _shotStart.SetActive(false);
            _shotAim.SetActive(true);
        }
        else
        {
            _shotStart.SetActive(true);
            _shotAim.SetActive(false);
        }
    }
}
