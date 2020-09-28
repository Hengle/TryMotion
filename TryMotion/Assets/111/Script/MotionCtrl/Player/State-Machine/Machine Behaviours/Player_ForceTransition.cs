/* ========================================================
*      作 者：Lin
*      主 题：Player 状态过渡
*      主要功能：

*      详细描述：

*      创建时间：2020-09-24 15:05:33
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ForceTransition" , menuName = "FSM/Player/强制状态过渡" , order = -1000)]
public class Player_ForceTransition : PlayerStateInfo
{
    [Range(0f , 1f)] public float transitionTime = 0.7f;//过渡时间

    public override void OnStateEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        
    }

    public override void OnStateUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        if (stateInfo.normalizedTime >= transitionTime)
        {
            animator.SetBool("ForceTransition" , true);
        }
    }

    public override void OnStateExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        if (stateInfo.normalizedTime >= transitionTime)
        {
            animator.SetBool("ForceTransition" , false);
        }
    }

    public override void OnStateIK(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        
    }

}
