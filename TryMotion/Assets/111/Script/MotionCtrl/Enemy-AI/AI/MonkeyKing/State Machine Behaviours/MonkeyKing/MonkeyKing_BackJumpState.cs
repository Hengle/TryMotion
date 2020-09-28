using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "MonkeyKing_BackJumpState" , menuName = "FSM/MonkeyKing/MonkeyKing_BackJumpState" , order = -500)]
public class MonkeyKing_BackJumpState : StateInfo_MonkeyKing
{
    [SerializeField] AnimationCurve curve_z;
    [SerializeField] AnimationCurve curve_y;

    Vector3 targetDir = Vector3.zero;

    ////rig
    //[SerializeField] Vector3 velocity = Vector3.up;

    public override void OnEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnEnter(animator, stateInfo, layerIndex);
    }

    public override void OnUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnUpdate(animator, stateInfo, layerIndex);

        //方式1
        targetDir.x = 0;
        targetDir.y = curve_y.Evaluate(stateInfo.normalizedTime);//stateInfo.normalizedTime
        targetDir.z = curve_z.Evaluate(stateInfo.normalizedTime);//Time.time
        targetDir = AiStateMachine.transform.TransformDirection(targetDir);

        AiStateMachine.CC.Move(targetDir * Time.deltaTime);

    }

    public override void OnExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnExit(animator, stateInfo, layerIndex);


        AiStateMachine.BackJump = false;
        AiStateMachine.IsPreparationFighting = false;//退出战斗预备
    }

}
