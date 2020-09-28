using UnityEngine;
using System.Collections;

public class AIMonkeyKingState_Alert1 : AIBossMonkeyKingState
{
    [SerializeField] float timer = 0;
    [SerializeField] [Range(1,10)]float lookAroundTargetTime = 5;


    void Start()
    {
        
    }

    public override AIStateType GetStateType()
    {
        return AIStateType.Alerted;
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        if (_aIStateMachine == null) { return; }

        _enemyStateMachine.SensorRaduis = 10;//进入警觉状态 触发器半径10m

        //配置 state Machine
        _enemyStateMachine.NavAgentControl(true, false);

        _enemyStateMachine.Alert = true;
        _enemyStateMachine.Agent.ResetPath();
        //_enemyStateMachine.speed = 0;
        //_enemyStateMachine.seeking = 0;
        //_enemyStateMachine.feeding = false;
        //_enemyStateMachine.attackType = 0;

        //_enemyStateMachine.Agent.isStopped = false;
    }

    #region 事件监听

    protected override void OnTakeDamageEvent(object[] objs)
    {
        base.OnTakeDamageEvent(objs);
        if (_enemyStateMachine.CurrentStateType == AIStateType.Alerted) { this.isByDamage = true; }
    }
    #endregion

    public override AIStateType OnUpdate()
    {
        //如果受到攻击
        if (isByDamage)
        {
            isByDamage = false;
            return AIStateType.Fighting;
        }

        // 视觉威胁
        if (_enemyStateMachine.VisualThreat.type == AITargetType.Visual_Player)
        {
            _enemyStateMachine.SetTarget(_enemyStateMachine.VisualThreat);
            _enemyStateMachine.attackerGo = _enemyStateMachine.VisualThreat.collider.gameObject;//获得攻击者目标
            return AIStateType.Fighting;
        }


        timer += Time.deltaTime;
        if (timer >= lookAroundTargetTime)
        {
            timer = 0;
            _enemyStateMachine.Alert = false;
            if (_enemyStateMachine.attackerGo != null) { return AIStateType.Fighting; }
            return AIStateType.Idle;
        }

        //if (_enemyStateMachine.VisualThreat.type == AITargetType.Visual_Food 
        //    && _enemyStateMachine.AudioThreat.type == AITargetType.Audio)
        //{
        //    // If the distance to hunger ratio means we are hungry enough to stray off the path that far
        //    if ((1.0f - _enemyStateMachine.satisfaction) > (_enemyStateMachine.VisualThreat.distance / _enemyStateMachine.sensorRadius))
        //    {
        //        _enemyStateMachine.SetTarget(_enemyStateMachine.VisualThreat);
        //        return AIStateType.Pursuit;
        //    }
        //}

        //// 威胁情况:
        //// 1.targetType由威胁过渡而来.
        //if ((_enemyStateMachine.currentTargetType == AITargetType.Audio || _enemyStateMachine.currentTargetType == AITargetType.Visual_Light) && _enemyStateMachine.isTargetReached)
        //{
        //    //处于状态过渡中... 视野角度 < _viewThreshold.
        //    float angle = AIState.FindSingleAngle(transform.forward, _enemyStateMachine.Agent.steeringTarget - transform.position);

        //    if (_enemyStateMachine.currentTargetType == AITargetType.Audio && Mathf.Abs(angle) < _viewThreshold)
        //    {
        //        return AIStateType.Pursuit;
        //    }
        //    if (_viewChanageTimer >= _viewMaxDuration)
        //    {
        //        if (Random.value <= _enemyStateMachine.intelligence)
        //        {
        //            _enemyStateMachine.seeking = (int)Mathf.Sign(angle);
        //        }
        //        else
        //        {
        //            _enemyStateMachine.seeking = (int)Mathf.Sign(Random.Range(-1, 1));
        //        }
        //        _viewChanageTimer = 0;
        //    }
        //}
        //// 2.targetType是wayPoint类型.
        //else if (_enemyStateMachine.currentTargetType == AITargetType.Waypoint && !_enemyStateMachine.Agent.pathPending)
        //{
        //    //处于 当视野角度 < _wayPointThreshold
        //    float angle = AIState.FindSingleAngle(transform.forward, _enemyStateMachine.Agent.steeringTarget - transform.position);

        //    if (Mathf.Abs(angle) < _wayPointThreshold)
        //    {
        //        Debug.LogError("Patrol");
        //        //_enemyStateMachine.Agent.SetDestination(_enemyStateMachine.GetWayPointPosition(true));
        //        return AIStateType.Patrol;
        //    }

        //    _enemyStateMachine.seeking = 0;
        //    if (_viewChanageTimer >= _viewMaxDuration)
        //    {
        //        if (Random.value <= _enemyStateMachine.intelligence)
        //        {
        //            _enemyStateMachine.seeking = (int)Mathf.Sign(angle);
        //        }
        //        else
        //        {
        //            _enemyStateMachine.seeking = (int)Mathf.Sign(Random.Range(-1, 1));
        //        }
        //        _viewChanageTimer = 0;
        //    }
        //}
        //// 3.目标都消失. enemy警觉
        //else
        //{
        //    Debug.LogError("Patrol++2");
        //    if (_viewChanageTimer >= _viewMaxDuration)
        //    {
        //        _enemyStateMachine.seeking = (int)Mathf.Sign(Random.Range(-1, 1));
        //        _viewChanageTimer = 0;
        //    }
        //}

        return AIStateType.Alerted;
    }


    public override void OnExitState()
    {
        base.OnExitState();

        
        _enemyStateMachine.Alert = false;
        timer = 0;
    }

    protected override void ResetAIState()
    {
        
    }
}
