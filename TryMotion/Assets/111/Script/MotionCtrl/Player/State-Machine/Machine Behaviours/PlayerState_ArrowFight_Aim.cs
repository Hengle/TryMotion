/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-27 13:43:37
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerState_ArrowFight_Aim" , menuName = "FSM/Player/PlayerState_ArrowFight_Aim" , order = -200)]
public class PlayerState_ArrowFight_Aim : PlayerStateInfo
{
    //private
    private PlayerMovementInputController _inputController;
    private PlayerAnimationController _playerAnimCtrl;
    private PlayerLocomotionController _playerLocomotionCtrl;


    public override void OnStateEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        if (_inputController == null) { _inputController = player.GetComponent<PlayerMovementInputController>(); }
        if (_playerAnimCtrl == null) { _playerAnimCtrl = player.GetComponent<PlayerAnimationController>(); }
        if (_playerLocomotionCtrl == null) { _playerLocomotionCtrl = player.GetComponent<PlayerLocomotionController>(); }
        _playerLocomotionCtrl.BlendCineMachine_ShootView(true);
    }

    public override void OnStateUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {       
        if (InputManager.Instance.IsAim)
        {
            if (InputManager.Instance.IsShot)
            {
                _playerAnimCtrl.ArrowAttack = 2;
            }
        }
        else
        {
            _playerAnimCtrl.ArrowAttack = 3;
        }
    }

    public override void OnStateExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        _playerAnimCtrl.ArrowAttack = 1;
    }

    public override void OnStateIK(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {

    }
}
