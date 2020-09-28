using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonkeyKing_ChongFengState" , menuName = "FSM/MonkeyKing/MonkeyKing_ChongFengState" , order = -500)]
public class MonkeyKing_ChongFengState : StateInfo_MonkeyKing
{

    [Header("强化版冲锋:")]
    public bool chongFengPlus = false;


    [Header("冲锋参数:")]
    public float Speed = 5;
    private MonkeyKingSkill_ChongFeng chongFeng;

    //private
    Vector3 norChongFengPoint = Vector3.zero;
    Vector3 atkerDir = Vector3.zero;

    public override void OnEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnEnter(animator , stateInfo , layerIndex);

        //开启冲锋检测
        chongFeng = (MonkeyKingSkill_ChongFeng)AiStateMachine.GetSkillTriggerById(SkillTriggerId.ChongFeng);
        chongFeng.OpenChongFengTri();
        //已经进入冲锋动画
        AiStateMachine.IsPlayingMotion = true;

        //目标非空判断
        if (AiStateMachine.attackerGo != null) { norChongFengPoint = AiStateMachine.attackerGo.transform.position; }
    }

    public override void OnUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnUpdate(animator , stateInfo , layerIndex);

        //if (AiStateMachine.attackerGo == null) { AiStateMachine.ChongFeng = false; return; }

        //强化版的冲锋
        if (chongFengPlus)
        {
            //确保y轴高度一致.
            Vector3 atkerPos = new Vector3(AiStateMachine.attackerGo.transform.position.x, AiStateMachine.transform.position.y, AiStateMachine.attackerGo.transform.position.z);
            Vector3 targetDir = (atkerPos - AiStateMachine.transform.position).normalized;
            AiStateMachine.CC.Move(targetDir * Time.deltaTime * Speed);
            //距离判断？
            if (Vector3.Distance(AiStateMachine.transform.position , AiStateMachine.attackerGo.transform.position) <= AiStateMachine.StoppingDistance)
            {
                AiStateMachine.ChongFeng = false;
                AiStateMachine.IsSkillCold = true;//进入技能cd
            }
            else 
            {
                atkerDir = AiStateMachine.attackerGo.transform.position - AiStateMachine.transform.position;
                atkerDir.y = 0;
                AiStateMachine.transform.rotation = Quaternion.Slerp(AiStateMachine.transform.rotation , Quaternion.LookRotation(atkerDir , Vector3.up) , Time.deltaTime * 6); 
            }
        }
        else
        {
            //确保y轴高度一致.
            norChongFengPoint.y = AiStateMachine.transform.position.y;
            Vector3 targetDir = (norChongFengPoint - AiStateMachine.transform.position).normalized;
            AiStateMachine.CC.Move(targetDir * Time.deltaTime * Speed);
            //距离判断
            if (Vector3.Distance(AiStateMachine.transform.position , norChongFengPoint) <= AiStateMachine.StoppingDistance)
            {
                AiStateMachine.ChongFeng = false;
                AiStateMachine.IsSkillCold = true;//进入技能cd
            }
            else 
            {
                atkerDir = norChongFengPoint - AiStateMachine.transform.position;
                atkerDir.y = 0;
                AiStateMachine.transform.rotation = Quaternion.Slerp(AiStateMachine.transform.rotation , Quaternion.LookRotation(atkerDir , Vector3.up) , Time.deltaTime * 6); 
            }
        }
    }

    public override void OnExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnExit(animator , stateInfo , layerIndex);

        AiStateMachine.ChongFeng = false;
        //关闭冲锋检测
        chongFeng.CloseChongFengTri();
        //已经进入冲锋动画
        AiStateMachine.IsPlayingMotion = false;
    }

}
