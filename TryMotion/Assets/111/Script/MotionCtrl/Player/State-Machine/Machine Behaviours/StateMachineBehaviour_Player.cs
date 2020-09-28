/* ========================================================
*      作 者：Lin
*      主 题：角色状态机
*      主要功能：

*      详细描述：

*      创建时间：2020-09-24 14:39:45
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBehaviour_Player : StateMachineBehaviour
{
    public PlayerStateInfo[] playerStateInfos;

    public override void OnStateEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnStateEnter(animator , stateInfo , layerIndex);

        if (playerStateInfos == null || playerStateInfos.Length <= 0) { Debug.LogError("请先对当前动画添加<PlayerStateInfo.cs>"); return; }
        for (int i = 0; i < playerStateInfos.Length; i++)
        {
            playerStateInfos[i].player = animator.transform;
            playerStateInfos[i].OnStateEnter(animator, stateInfo, layerIndex);
        }
    }

    public override void OnStateUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        for (int i = 0; i < playerStateInfos.Length; i++)
        {
            playerStateInfos[i].OnStateUpdate(animator , stateInfo , layerIndex);
        }
    }

    public override void OnStateIK(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnStateIK(animator , stateInfo , layerIndex);

        for (int i = 0; i < playerStateInfos.Length; i++)
        {
            playerStateInfos[i].OnStateIK(animator , stateInfo , layerIndex);
        }
    }

    public override void OnStateExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnStateExit(animator , stateInfo , layerIndex);

        for (int i = 0; i < playerStateInfos.Length; i++)
        {
            playerStateInfos[i].OnStateExit(animator , stateInfo , layerIndex);
        }
    }
}
