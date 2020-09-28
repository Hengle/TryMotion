using UnityEngine;
using System.Collections;

public class AIMonkeyKingState_Patrol1 : AIBossMonkeyKingState
{
    // Inpsector Assigned 
    [SerializeField] float _slerpSpeed = 5.0f;
    [SerializeField] float _turnOnSpotThreshold = 60f;

    [SerializeField] [Range(0.0f, 3.0f)] float _speed = 1.0f;


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

        //配置nav
        _enemyStateMachine.NavAgentControl(true,false);

        LookingForAPointUnit5Meter();
        _enemyStateMachine.Walk = true;

        //重新-导航-路径
        //_enemyStateMachine.Agent.Resume();
        //_enemyStateMachine.Agent.isStopped = false;
    }

    #region 事件监听

    protected override void OnTakeDamageEvent(object[] objs)
    {
        base.OnTakeDamageEvent(objs);
        if (_enemyStateMachine.CurrentStateType == AIStateType.Patrol) { this.isByDamage = true; }
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

        if (_enemyStateMachine.VisualThreat.type == AITargetType.Visual_Player)
        {
            _enemyStateMachine.SetTarget(_enemyStateMachine.VisualThreat);
            return AIStateType.Alerted;
        }



        #region 当猴王在游走过程中,看到了角色.??? 是否马上就进入警觉模式!

        //// 计算,两个向量夹角度数.
        //float angle = Vector3.Angle(transform.forward, (_enemyStateMachine.Agent.steeringTarget - transform.position));

        //// 如果,角度>转向阈值. 切换为警觉State.
        //if (angle > _turnOnSpotThreshold)
        //{
        //    return AIStateType.Alerted;
        //}

        #endregion

        // 如果,不使用根运动,则NavAgent来控制旋转.
        if (!_enemyStateMachine.useRootRotation)
        {
            Debug.LogError("使用导航朝向来控制旋转!");
            Quaternion newRot = Quaternion.LookRotation(_enemyStateMachine.Agent.desiredVelocity);
            _enemyStateMachine.transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.deltaTime * _slerpSpeed);
        }

        //// 出现,导航路径丢失的情况.重新配置下一个节点.
        //if (_enemyStateMachine.Agent.isPathStale ||
        //    !_enemyStateMachine.Agent.hasPath ||
        //    _enemyStateMachine.Agent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathComplete)
        //{
        //    return AIStateType.Idle;
        //}

        //----------------------
        // 威胁判断!!!!
        // ----------------------
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
        _enemyStateMachine.Walk = false;
        _enemyStateMachine.Agent.ResetPath();//每次退出.巡逻.都需要ResetPath()一下.
    }

    public override void OnDestinationReached(bool isReached)
    {
        if (_enemyStateMachine == null || isReached == false)
            return;
    }

    protected override void ResetAIState()
    {
        
    }

    #region 额外方法

    /// <summary>
    /// 在初始坐标5米范围内随机一个坐标作为目标点，朝向目标点移动
    /// </summary>
    void LookingForAPointUnit5Meter()
    {
        Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle * 5;//随机一个半径为5的圆.
        Vector3 targetPos = oriPos + new Vector3(insideUnitCircle.x, 0, insideUnitCircle.y);
        _enemyStateMachine.Agent.SetDestination(targetPos);
    }


    #endregion
}
