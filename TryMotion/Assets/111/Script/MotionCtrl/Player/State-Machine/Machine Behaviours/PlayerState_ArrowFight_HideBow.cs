/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-28 13:52:54
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState_ArrowFight_HideBow" , menuName = "FSM/Player/PlayerState_ArrowFight_HideBow" , order = -200)]
public class PlayerState_ArrowFight_HideBow : PlayerStateInfo
{
    //private
    private PlayerLocomotionController _playerLocomotionCtrl;


    public override void OnStateEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        if (_playerLocomotionCtrl == null) { _playerLocomotionCtrl = player.GetComponent<PlayerLocomotionController>(); }


        _playerLocomotionCtrl.BlendCineMachine_ShootView(false);
    }

    public override void OnStateUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {

    }

    public override void OnStateExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {

    }

    public override void OnStateIK(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {

    }
}
