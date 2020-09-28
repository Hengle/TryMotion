/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-08-23 19:51:55
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonkeyKing_WalkForNorAtkState" , menuName = "FSM/MonkeyKing/MonkeyKing_WalkForNorAtkState " , order = -500)]
public class MonkeyKing_WalkForNorAtkState : StateInfo_MonkeyKing
{
    public float moveSpeed = 3;
    public float slerpSpeed = 6;

    Vector3 targetDir = Vector3.zero;
    Vector3 targetPos = Vector3.zero;

    //动画状态Enter
    public override void OnEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnEnter(animator , stateInfo , layerIndex);
        AiStateMachine.Agent.updateRotation = false;
        AiStateMachine.IsPlayingMotion = true;
    }

    //动画状态Update
    public override void OnUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnUpdate(animator , stateInfo , layerIndex);

        //赋值目标pos
        targetPos.x = AiStateMachine.attackerGo.transform.position.x;
        targetPos.y = AiStateMachine.transform.position.y;
        targetPos.z = AiStateMachine.attackerGo.transform.position.z;
        //获得目标朝向
        targetDir = targetPos - AiStateMachine.transform.position;
        //移动d
        AiStateMachine.CC.Move(targetDir.normalized * moveSpeed * Time.deltaTime);
        //距离判断?
        if (Vector3.Distance(AiStateMachine.transform.position, AiStateMachine.attackerGo.transform.position) <= AiStateMachine.NormalAtkDistance)
        {
            AiStateMachine.IsPlayingMotion = false;
            AiStateMachine.Walk = false;
        }
        AiStateMachine.transform.rotation = Quaternion.Slerp(AiStateMachine.transform.rotation, Quaternion.LookRotation(targetDir, Vector3.up), Time.deltaTime * slerpSpeed);
    }

    //动画状态Exit
    public override void OnExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnExit(animator , stateInfo , layerIndex);
        AiStateMachine.Agent.updateRotation = true;
    }
}
