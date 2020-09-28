/* ========================================================
*      作 者：Lin
*      主 题：远程猴子(金) 待机
*      主要功能：

*      详细描述：

*      创建时间：2020-09-22 15:22:57
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 远程猴子(金) 待机
/// </summary>
public class AIRMonkeyGoldState_Idle1 : AIRemoteMonkeyGoldState
{
    [SerializeField] Vector2 moveSection = new Vector2(0.2f,0.8f);
    //private
    [SerializeField] bool enterActivity = false; //是否进入活动状态
    [SerializeField] bool enterPatrol = false;//是否进入巡逻状态
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


        _enemyStateMachine.CurrentAnimationName = MeshAnimationName.RemoteGold.idle;

        //清除目标
        _enemyStateMachine.ClearTarget();

        //超出了,初始(5m)范围.
        if (Vector3.Distance(oriPos , _enemyStateMachine.transform.position) > 5)
        {
            _enemyStateMachine.Agent.SetDestination(oriPos);
            _enemyStateMachine.CurrentAnimationName = MeshAnimationName.RemoteGold.move;
        }
        else
        {
            //计算 进入那个状态
            bool isRecreation = AIState.CalcProbability(moveSection);
            StartCoroutine(StayIdle(isRecreation));
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

        ////寻路结束.
        //if (_enemyStateMachine.Agent.remainingDistance <= _enemyStateMachine.Agent.stoppingDistance && 
        //    !_enemyStateMachine.Agent.pathPending)
        //{
        //    _enemyStateMachine.CurrentAnimationName = MeshAnimationName.RemoteGold.idle;
        //    return AIStateType.Idle;
        //}

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

        //进入活动状态
        if (enterActivity)
        {
            return AIStateType.Recreation;
        }
        else
        {
            return AIStateType.Patrol;
        }
    }

    public override void OnExitState()
    {
        base.OnExitState();

        StopCoroutine(StayIdle());
        enterActivity = false;
        enterPatrol = false;
    }

    protected override void ResetAIState()
    {
        enterActivity = false;
        enterPatrol = false;
    }

    //\----------------------------------跳上这快车终点方向未明------------------------------------

    /// <summary>
    /// 延迟待机
    /// </summary>
    IEnumerator StayIdle(bool enterRecreation = false) 
    {
        //娱乐模式
        if (enterRecreation)
        {
            float timer = 0;
            float stayTime = 2;
            while (!enterActivity)
            {
                if (timer >= stayTime)
                {
                    enterActivity = true;
                }
                timer += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            float timer = 0;
            float stayTime = 0.5f;
            while (!enterPatrol)
            {
                if (timer >= stayTime)
                {
                    enterPatrol = true;
                }
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}
