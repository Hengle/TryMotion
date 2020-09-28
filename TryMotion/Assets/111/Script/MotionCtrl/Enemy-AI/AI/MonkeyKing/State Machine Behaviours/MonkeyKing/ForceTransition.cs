using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 强制动画过渡
/// </summary>
[CreateAssetMenu(fileName = "ForceTransition" , menuName = "FSM/MonkeyKing/强制条件转换", order = -500)]
public class ForceTransition : StateInfo_MonkeyKing
{

    [Range(0f, 1f)] public float transitionTime = 0.7f; //跳转时间

    public override void OnEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnEnter(animator, stateInfo, layerIndex);
    }

    public override void OnUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnUpdate(animator, stateInfo, layerIndex);

        if (stateInfo.normalizedTime >= transitionTime)
        {
            animator.SetBool("ForceTransition" , true);
        }
    }
    public override void OnExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnExit(animator, stateInfo, layerIndex);

        if (stateInfo.normalizedTime >= transitionTime)
        {
            animator.SetBool("ForceTransition" , false);
        }
    }
}
