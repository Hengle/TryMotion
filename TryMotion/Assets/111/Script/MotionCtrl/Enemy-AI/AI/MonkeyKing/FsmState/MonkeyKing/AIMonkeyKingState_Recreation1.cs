using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 猴王状态-娱乐
/// </summary>
public class AIMonkeyKingState_Recreation1 : AIBossMonkeyKingState
{
    public override AIStateType GetStateType()
    {
        return AIStateType.Recreation;
    }

    public override void OnEnterState()
    {
        if (_aIStateMachine == null) { return; }
        //配置 state Machine
        _enemyStateMachine.NavAgentControl(true , false);
        //_enemyStateMachine.Agent.isStopped = true;
        //Debug.LogError("进入娱乐状态!");

        Recreation();
    }

    #region 事件监听

    protected override void OnTakeDamageEvent(object[] objs)
    {
        base.OnTakeDamageEvent(objs);
        if (_enemyStateMachine.CurrentStateType == AIStateType.Recreation) { this.isByDamage = true; }
        //StartCoroutine(delay2TakeDamage());
    }
    IEnumerator delay2TakeDamage()
    {
        yield return new WaitForSeconds(0.1f);
        this.isByDamage = true;
    }

    #endregion


    public override AIStateType OnUpdate()
    {
        //受到伤害
        if (isByDamage)
        {
            isByDamage = false;
            return AIStateType.Alerted;
        }

        AnimatorStateInfo animatorInfo = _enemyStateMachine.Anim.GetCurrentAnimatorStateInfo(0);
        if ((animatorInfo.normalizedTime >= 0.95f) && (animatorInfo.IsName("activity1") || animatorInfo.IsName("activity2")))
        {
            return AIStateType.Idle;
        }

        // 视觉威胁
        if (_enemyStateMachine.VisualThreat.type == AITargetType.Visual_Player)
        {
            _enemyStateMachine.SetTarget(_enemyStateMachine.VisualThreat);
            return AIStateType.Pursuit;
        }
        // 声音威胁
        else if (_enemyStateMachine.AudioThreat.type == AITargetType.Audio)
        {
            _enemyStateMachine.SetTarget(_enemyStateMachine.AudioThreat);
            return AIStateType.Alerted;
        }

        return AIStateType.Recreation;
    }


    public override void OnExitState()
    {
        _enemyStateMachine.ActivityType = 0;
        //Debug.LogError("退出娱乐状态!");
    }

    protected override void ResetAIState()
    {
        
    }

    #region 额外方法

    /// <summary>
    /// 猴王玩耍时间
    /// </summary>
    void Recreation()
    {
        int random = UnityEngine.Random.Range(0, 100);
        bool activity1 = random >= 50 ? true : false;
        if (activity1)//播放活动1
        {
            _enemyStateMachine.ActivityType = 1;
        }
        else
        {
            _enemyStateMachine.ActivityType = 2;
        }
    }

    #endregion
}
