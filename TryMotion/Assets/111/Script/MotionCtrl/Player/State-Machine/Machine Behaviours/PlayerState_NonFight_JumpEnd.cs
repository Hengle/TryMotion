/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-24 15:59:28
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerState_NonFight_JumpEnd" , menuName = "FSM/Player/PlayerState_NonFight_JumpEnd" , order = -200)]
public class PlayerState_NonFight_JumpEnd : PlayerStateInfo
{
    [SerializeField] AnimationCurve curve;

    //private
    private PlayerMovementInputController inputController;
    private CharacterController collider;

    private float def_height;
    private float def_centerY;
    private float tar_height;
    private float tar_centerY;


    public override void OnStateEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        if (inputController == null) { inputController = player.GetComponent<PlayerMovementInputController>(); }
        if (collider == null) { collider = player.GetComponent<CharacterController>(); }
        def_height = inputController.parameter.defaultJumpHeight;
        def_centerY = inputController.parameter.defaultJumpCenterY;
        tar_centerY = inputController.parameter.jumpCenterY;
        tar_height = inputController.parameter.jumpHeight;
    }

    public override void OnStateUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        player.GetComponent<PlayerAnimationController>().Jump = false;

        float height = Mathf.Lerp(tar_height , def_height , curve.Evaluate(stateInfo.normalizedTime));
        float centerY = Mathf.Lerp(tar_centerY , def_centerY , curve.Evaluate(stateInfo.normalizedTime));
        collider.center = new Vector3(0 , centerY , 0);
        collider.height = height;
    }

    public override void OnStateExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        collider.center = new Vector3(0 , def_centerY , 0);
        collider.height = def_height;
    }

    public override void OnStateIK(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {

    }
}
