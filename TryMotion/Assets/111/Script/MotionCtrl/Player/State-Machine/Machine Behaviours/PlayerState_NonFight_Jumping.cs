/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-27 11:35:33
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState_NonFight_Jumping" , menuName = "FSM/Player/PlayerState_NonFight_Jumping" , order = -200)]
public class PlayerState_NonFight_Jumping : PlayerStateInfo
{
    //private
    private PlayerMovementInputController inputController;


    public override void OnStateEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        if (inputController == null) { inputController = player.GetComponent<PlayerMovementInputController>(); }
    }

    public override void OnStateUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        //bool isOnGround = inputController.CheckIsGround();
        //animator.SetBool(AnimEnumType.IsOnGround.ToString(), isOnGround);
    }

    public override void OnStateExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {

    }

    public override void OnStateIK(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {

    }
}
