/* ========================================================
*      作 者：Lin 
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-08-19 13:31:46
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonkeyKing_NormalAttack2State"  , menuName = "FSM/MonkeyKing/MonkeyKing_NormalAttack2State " , order = -500)]
public class MonkeyKing_NormalAttack2State : StateInfo_MonkeyKing
{
    Vector3 atkerDir = Vector3.zero;


    [SerializeField, Header("触发区间")] Vector2 atkInterval = Vector2.zero;
     MonkeyKingSkill_norAtk2 skillTri;

    //动画状态Enter
    public override void OnEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnEnter(animator , stateInfo , layerIndex);
        AiStateMachine.IsPlayingMotion = true;
        AiStateMachine.Agent.updateRotation = false;

        //技能触发
        skillTri = (MonkeyKingSkill_norAtk2)AiStateMachine.GetSkillTriggerById(SkillTriggerId.NormalAttack2);

        //非空判断?
        if (AiStateMachine.attackerGo != null)
        {
            Vector3 atkerPos = new Vector3(AiStateMachine.attackerGo.transform.position.x, AiStateMachine.transform.position.y, AiStateMachine.attackerGo.transform.position.z);
            atkerDir = atkerPos - AiStateMachine.transform.position;
        }
    }

    //动画状态Update
    public override void  OnUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnUpdate(animator , stateInfo , layerIndex);

        if (stateInfo.normalizedTime >= atkInterval.x && stateInfo.normalizedTime <= atkInterval.y)
        {
            skillTri.OpenNorAttack2Tri();
        }
        else
        {
            skillTri.CloseNorAttack2Tri();
            AiStateMachine.NormalAtk = 0;
        }

        //朝向atkerGo.
        AiStateMachine.transform.rotation = Quaternion.Slerp(AiStateMachine.transform.rotation, Quaternion.LookRotation(atkerDir, Vector3.up), Time.deltaTime * 6);
    }

    //动画状态Exit
    public override void OnExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnExit(animator , stateInfo , layerIndex);
        AiStateMachine.Agent.updateRotation = true;
        AiStateMachine.NormalAtk = 0;
        AiStateMachine.IsSkillCold = true;//进入技能cd
        AiStateMachine.IsPlayingMotion = false;
    }
}
