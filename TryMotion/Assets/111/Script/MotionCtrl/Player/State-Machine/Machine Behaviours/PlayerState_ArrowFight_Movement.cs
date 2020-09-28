/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-25 17:24:50
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerState_ArrowFight_Movement" , menuName = "FSM/Player/PlayerState_ArrowFight_Movement" , order = -200)]
public class PlayerState_ArrowFight_Movement : PlayerStateInfo
{

    //private
    private PlayerMovementInputController _inputController;
    private PlayerAnimationController _playerAnimCtrl;
    private InputManager _inputManager;

    public override void OnStateEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        if (_inputController == null) { _inputController = player.GetComponent<PlayerMovementInputController>(); }
        if (_playerAnimCtrl == null) { _playerAnimCtrl = player.GetComponent<PlayerAnimationController>(); }
        if (_inputManager == null) { _inputManager = InputManager.Instance; }
    }

    public override void OnStateUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        //处于弓箭
        if (_playerAnimCtrl.InArrowFight)
        {
            if (_inputManager.IsAim)
            {
                _playerAnimCtrl.ArrowAttack = 1;
            }
            else
            {
                _playerAnimCtrl.ArrowAttack = 0;
            }
        }
    }

    public override void OnStateExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {

    }

    public override void OnStateIK(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {

    }
}
