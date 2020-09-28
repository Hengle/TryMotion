/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-08-23 15:18:21
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonkeyKing_PreparationFightingState"  , menuName = "FSM/MonkeyKing/MonkeyKing_PreparationFightingState " , order = -500)]
public class MonkeyKing_PreparationFightingState : StateInfo_MonkeyKing
{
    //动画状态Enter
    public override void OnEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnEnter(animator , stateInfo , layerIndex);
    }

    //动画状态Update
    public override void  OnUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnUpdate(animator , stateInfo , layerIndex);

        if (AiStateMachine.IsPreparationFighting && stateInfo.normalizedTime >= 1)
        {
            AiStateMachine.IsPreparationFighting = false;//退出准备战斗!
        }
    }

    //动画状态Exit
    public override void OnExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnExit(animator , stateInfo , layerIndex);


    }
}
