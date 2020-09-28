using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonkeyKing_MoveRightState" , menuName = "FSM/MonkeyKing/MonkeyKing_MoveRightState" , order = -500)]
public class MonkeyKing_MoveRightState : StateInfo_MonkeyKing
{
    [Header("移动速度")]
    public float moveSpeed = 1;
    [Header("权重:")]
    [SerializeField, Range(0 , 1f)] float weight = 0;       //总权重
    [SerializeField, Range(0 , 1f)] float bodyWeight = 0;   //身体权重
    [SerializeField, Range(0 , 1f)] float headWeight = 0;   //头部权重

    //private
    Vector3 targetPos;//朝向坐标
    float defaultSpeed = 0;

    public override void OnEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnEnter(animator , stateInfo , layerIndex);

        //进入状态就马上关闭NavAgent的旋转控制.
        AiStateMachine.Agent.updateRotation = false;
        //设置Agent导航速度.
        defaultSpeed = AiStateMachine.Agent.speed;
        AiStateMachine.Agent.speed = moveSpeed;

        //随机2~4m移动距离.
        Vector3 movePos = AiStateMachine.transform.right;
        float dis = UnityEngine.Random.Range(2 , 5);
        movePos *= dis;
        //朝右移动
        targetPos = AiStateMachine.transform.position + movePos;
        AiStateMachine.Agent.SetDestination(targetPos);
    }

    public override void OnUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnUpdate(animator , stateInfo , layerIndex);

        //已经导航到了目标位置.
        if (AiStateMachine.Agent.remainingDistance <= AiStateMachine.Agent.stoppingDistance)
        {
            AiStateMachine.Agent.ResetPath();
            AiStateMachine.RightMove = false;
        }
        else
        {
            if (AiStateMachine.attackerGo != null)
            {
                //方案1 (头部和躯干朝向目标)
                animator.SetLookAtWeight(weight , bodyWeight , headWeight);
                animator.SetLookAtPosition(targetPos);

                //方案2(全朝向目标)
                ////朝向atkerGo.(一直朝向)
                //Vector3 targetDir = AiStateMachine.attackerGo.transform.position - AiStateMachine.transform.position;
                //AiStateMachine.transform.rotation = Quaternion.Slerp(AiStateMachine.transform.rotation , Quaternion.LookRotation(targetDir , Vector3.up) , Time.deltaTime * 6);
            }
        }
    }

    public override void OnExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        base.OnExit(animator , stateInfo , layerIndex);
        AiStateMachine.Agent.updateRotation = true;
        AiStateMachine.Agent.speed = defaultSpeed;
        AiStateMachine.IsPreparationFighting = false;//退出战斗预备
    }
}
