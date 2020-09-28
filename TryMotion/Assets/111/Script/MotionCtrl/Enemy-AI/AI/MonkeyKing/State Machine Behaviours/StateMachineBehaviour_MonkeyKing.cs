using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBehaviour_MonkeyKing : StateMachineBehaviour
{
    public List<StateInfo_MonkeyKing> stateInfos;


    public override void OnStateEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        for (int i = 0; i < stateInfos.Count; i++)
        {
            stateInfos[i].AiStateMachine = animator.GetComponentInChildren<AIStateMachine_MonkeyKing>();
            stateInfos[i].OnEnter(animator , stateInfo , layerIndex);
        }

        base.OnStateEnter(animator , stateInfo , layerIndex);
    }

    public override void OnStateUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        for (int i = 0; i < stateInfos.Count; i++)
        {
            stateInfos[i].OnUpdate(animator , stateInfo , layerIndex);
        }
        base.OnStateUpdate(animator , stateInfo , layerIndex);
    }

    public override void OnStateExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        for (int i = 0; i < stateInfos.Count; i++)
        {
            stateInfos[i].OnExit(animator , stateInfo , layerIndex);
        }
        base.OnStateExit(animator , stateInfo , layerIndex);
    }
}
