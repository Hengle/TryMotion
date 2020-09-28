/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-24 14:43:42
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色状态类型
/// </summary>
public enum PlayerStateType 
{
    /// <summary>
    /// 强制过渡
    /// </summary>
    ForceTransition,
}

public abstract class PlayerStateInfo : ScriptableObject
{
    [HideInInspector] public Transform player;

    public abstract void OnStateEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex);
    public abstract void OnStateUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex);
    public abstract void OnStateIK(Animator animator , AnimatorStateInfo stateInfo , int layerIndex);
    public abstract void OnStateExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex);
}
