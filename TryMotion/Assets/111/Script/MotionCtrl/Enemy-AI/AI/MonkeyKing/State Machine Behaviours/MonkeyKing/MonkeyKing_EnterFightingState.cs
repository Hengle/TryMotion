/* ========================================================
*      作 者：Lin 
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-08-18 23:25:14
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonkeyKing_EnterFightingState"  , menuName = "FSM/MonkeyKing/MonkeyKing_EnterFightingState " , order = -500)]
public class MonkeyKing_EnterFightingState : StateInfo_MonkeyKing
{
    [Header("非战斗状态")]
    [SerializeField] float originalRadius = 0.6f;
    [SerializeField] Vector3 originalCetner = Vector3.zero;

    [Header("战斗状态")]
    [SerializeField] float fightingRadius = 0.8f;
    [SerializeField] Vector3 fightingCetner = Vector3.zero;

    //动画状态Enter
    public override void OnEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnEnter(animator , stateInfo , layerIndex);

        AiStateMachine.CC.radius = originalRadius;
        AiStateMachine.CC.center = fightingCetner;
    }

    //动画状态Enter
    public override void  OnUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnUpdate(animator , stateInfo , layerIndex);
    }

    public override void OnExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnExit(animator , stateInfo , layerIndex);
        AiStateMachine.CC.radius = fightingRadius;
        AiStateMachine.CC.center = originalCetner;
        AiStateMachine.EnterFighting = false;
    }
}
