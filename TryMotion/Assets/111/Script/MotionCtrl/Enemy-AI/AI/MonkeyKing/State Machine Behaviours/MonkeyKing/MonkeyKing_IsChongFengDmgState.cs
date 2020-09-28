/* ========================================================
*      作 者：Lin 
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-08-19 15:49:30
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonkeyKing_IsChongFengDmgState"  , menuName = "FSM/MonkeyKing/MonkeyKing_IsChongFengDmgState " , order = -500)]
public class MonkeyKing_IsChongFengDmgState : StateInfo_MonkeyKing
{

    [SerializeField] bool releaseSummon = false;//释放召唤
    [Range(0f, 1f)] float releaseFactor;

    //动画状态Enter
    public override void OnEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnEnter(animator , stateInfo , layerIndex);
        releaseSummon = false;
    }

    //动画状态Update
    public override void  OnUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnUpdate(animator , stateInfo , layerIndex);

        if (stateInfo.normalizedTime >= releaseFactor && !releaseSummon)
        {
            releaseSummon = true;
            //召唤小怪
           
        }
    }

    //动画状态Exit
    public override void OnExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnExit(animator , stateInfo , layerIndex);
        AiStateMachine.IsChongFengDmg = false;
        AiStateMachine.SendMessage("ResetSkillCold");//AIMonkeyKingState_Fighting1.ResetSkillCold();
    }
}
