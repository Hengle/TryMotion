using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 猴王AI-Idle
/// </summary>
public class AIMonkeyKingState_Idle1 : AIBossMonkeyKingState
{
    //idle状态类型概率
    [SerializeField] private Vector2 _idleTypeFactor = new Vector2(0.2f,0.8f);

    //private
    [SerializeField] bool enterActivity = false; //是否进入活动状态
    Vector3 oriPos = Vector3.zero;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        oriPos = _enemyStateMachine.transform.position;
    }

    public override AIStateType GetStateType()
    {
        return AIStateType.Idle;
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        if (_enemyStateMachine == null)
        {
            return;
        }
        enterActivity = AIState.CalcProbability(_idleTypeFactor);//进入娱乐模式 (20%)

        _enemyStateMachine.NavAgentControl(true, true);
        _enemyStateMachine.Walk = false;
        _enemyStateMachine.ActivityType = 0;
        _enemyStateMachine.ExitFighting = true;

        _enemyStateMachine.ClearTarget();


        //超出了,初始(5m)范围.
        if (Vector3.Distance(oriPos, _enemyStateMachine.transform.position) > 5)
        {
            //_enemyStateMachine.Agent.isStopped = false;
            _enemyStateMachine.Agent.SetDestination(oriPos);
            _enemyStateMachine.Walk = true;
        }
    }


    #region 事件监听

    protected override void OnTakeDamageEvent(object[] objs)
    {
        base.OnTakeDamageEvent(objs);
        if (_enemyStateMachine.CurrentStateType == AIStateType.Idle) { this.isByDamage = true; }
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

        //当处于回到原点状态.才进行判断.
        if (_enemyStateMachine.Walk)
        {
            if (_enemyStateMachine.Agent.remainingDistance <= _enemyStateMachine.Agent.stoppingDistance && !_enemyStateMachine.Agent.pathPending)
            {
                _enemyStateMachine.Walk = false;
                return AIStateType.Idle;
            }
        }

        //先进行威胁判断.
        // 视觉威胁
        if (_enemyStateMachine.VisualThreat.type == AITargetType.Visual_Player)
        {
            _enemyStateMachine.SetTarget(_enemyStateMachine.VisualThreat);
            return AIStateType.Alerted;
        }
        // 声音威胁
        else if (_enemyStateMachine.AudioThreat.type == AITargetType.Audio)
        {
            _enemyStateMachine.SetTarget(_enemyStateMachine.AudioThreat);
            return AIStateType.Alerted;
        }


        //常规动画播放
        AnimatorStateInfo animatorInfo = _enemyStateMachine.Anim.GetCurrentAnimatorStateInfo(0);
        if ((animatorInfo.normalizedTime >= 1) && (animatorInfo.IsName("idle")))
        {
            //进入 活动动画播放
            if (enterActivity)
            {
                bool activity1 = AIState.CalcProbability(Vector2.one * 0.5f);
                if (activity1)//播放活动1
                {
                    _enemyStateMachine.ActivityType = 1;
                }
                else
                {
                    _enemyStateMachine.ActivityType = 2;
                }
                return AIStateType.Recreation;
            }
            else
            {
                return AIStateType.Patrol;
            }
        }


        #region 猴王.不知道要不要加.1.视觉监听 2.听觉 3.食物

        //else if (_enemyStateMachine.VisualThreat.type == AITargetType.Visual_Light)
        //{
        //    _enemyStateMachine.SetTarget(_enemyStateMachine.VisualThreat);
        //    return AIStateType.Alerted;
        //}
        //else if (_enemyStateMachine.AudioThreat.type == AITargetType.Audio)
        //{
        //    _enemyStateMachine.SetTarget(_enemyStateMachine.AudioThreat);
        //    return AIStateType.Alerted;
        //}
        //else if (_enemyStateMachine.VisualThreat.type == AITargetType.Visual_Food)
        //{
        //    _enemyStateMachine.SetTarget(_enemyStateMachine.VisualThreat);
        //    return AIStateType.Pursuit;
        //}

        #endregion

        return AIStateType.Idle;
    }

    public override void OnExitState()
    {
        enterActivity = false;
        _enemyStateMachine.ActivityType = 0;
        _enemyStateMachine.Agent.ResetPath();
        _enemyStateMachine.Walk = false;
    }

    protected override void ResetAIState()
    {
        
    }
}
