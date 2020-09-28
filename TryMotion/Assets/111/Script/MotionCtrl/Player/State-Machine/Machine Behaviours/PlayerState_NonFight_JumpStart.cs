/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-24 14:50:15
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerState_NonFight_JumpStart" , menuName = "FSM/Player/PlayerState_NonFight_JumpStart" , order = -200)]
public class PlayerState_NonFight_JumpStart : PlayerStateInfo
{
    [Range(0 , 1f)][SerializeField] float jumpStart;
    [SerializeField] AnimationCurve curve;

    //private
    private bool isGet = false;
    private PlayerMovementInputController inputController;
    private CharacterController collider;

    private float def_height;
    private float def_centerY;
    private float tar_height;
    private float tar_centerY;

    public override void OnStateEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        if (inputController == null) { inputController = player.GetComponent<PlayerMovementInputController>();}
        if (collider == null) { collider = player.GetComponent<CharacterController>(); }
        def_height = inputController.parameter.defaultJumpHeight;
        def_centerY = inputController.parameter.defaultJumpCenterY;
        tar_centerY = inputController.parameter.jumpCenterY;
        tar_height = inputController.parameter.jumpHeight;
    }

    public override void OnStateUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        if (stateInfo.normalizedTime >= jumpStart)
        {
            inputController.Jump();
        }
        float height = Mathf.Lerp(def_height, tar_height, curve.Evaluate(stateInfo.normalizedTime));
        float centerY = Mathf.Lerp(def_centerY , tar_centerY , curve.Evaluate(stateInfo.normalizedTime));
        collider.center = new Vector3(0, centerY , 0);
        collider.height = height;

        player.GetComponent<PlayerAnimationController>().Jump = false;
    }

    public override void OnStateExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        collider.center = new Vector3(0, tar_centerY , 0);
        collider.height = tar_height;
    }

    public override void OnStateIK(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {

    }

}
