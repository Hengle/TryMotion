/* ========================================================
*      作 者：Lin
*      主 题：远程猴子(金) 巡逻
*      主要功能：

*      详细描述：

*      创建时间：2020-09-22 16:55:10
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AIRMonkeyGoldState_Patrol1 : AIRemoteMonkeyGoldState
{

    // Inpsector Assigned 
    [SerializeField] float _slerpSpeed = 5.0f;
    [SerializeField] float _turnOnSpotThreshold = 60f;
    [SerializeField] [Range(0.0f , 3.0f)] float _speed = 1.0f;
    //private
    Vector3 oriPos;

    private void Start()
    {
        oriPos = transform.position;
    }

    public override AIStateType GetStateType()
    {
        return AIStateType.Patrol;
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        if (_enemyStateMachine == null)
            return;

        _enemyStateMachine.NavAgentControl(true , false);

        LookingForAPointUnit5Meter();
    }

    public override AIStateType OnUpdate()
    {

        //受到伤害
        if (isByDamage)
        {
            isByDamage = false;
            return AIStateType.Alerted;
        }

        if (_enemyStateMachine.VisualThreat.type == AITargetType.Visual_Player)
        {
            _enemyStateMachine.SetTarget(_enemyStateMachine.VisualThreat);
            return AIStateType.Alerted;
        }

        ///游走的过程中,看到了角色.  是否马上进入警觉模式!
        ///

        // 如果,不使用根运动,则NavAgent来控制旋转.
        if (!_enemyStateMachine.useRootRotation)
        {
            Debug.LogError("使用导航朝向来控制旋转!");
            Quaternion newRot = Quaternion.LookRotation(_enemyStateMachine.Agent.desiredVelocity);
            _enemyStateMachine.transform.rotation = Quaternion.Slerp(transform.rotation , newRot , Time.deltaTime * _slerpSpeed);
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

        //已游走到指定位置.
        if (_enemyStateMachine.Agent.remainingDistance <= _enemyStateMachine.Agent.stoppingDistance)
        {
            _enemyStateMachine.Agent.ResetPath();
            return AIStateType.Idle;
        }

        return AIStateType.Patrol;
    }

    public override void OnExitState()
    {
        base.OnExitState();


    }

    public override void OnDestinationReached(bool isReached)
    {
        base.OnDestinationReached(isReached);

        Debug.LogError("是否到达目标位置? " + isReached);
    }

    protected override void ResetAIState()
    {
        
    }




    //\----------------------------------跳上这快车终点方向未明------------------------------------


    /// <summary>
    /// 在初始坐标5米范围内随机一个坐标作为目标点，朝向目标点移动
    /// </summary>
    void LookingForAPointUnit5Meter()
    {
        Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle * 5;//随机一个半径为5的圆.
        Vector3 targetPos = oriPos + new Vector3(insideUnitCircle.x , 0 , insideUnitCircle.y);
        _enemyStateMachine.Agent.SetDestination(targetPos);
        _enemyStateMachine.CurrentAnimationName = MeshAnimationName.RemoteGold.move;
    }

    #region 伤害事件监听

    protected override void OnTakeDamageEvent(object[] objs)
    {
        base.OnTakeDamageEvent(objs);
        if (_enemyStateMachine.CurrentStateType == AIStateType.Patrol) { this.isByDamage = true; }
    }

    #endregion
}
