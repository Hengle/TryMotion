using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using FSG.MeshAnimator;
using FSG.MeshAnimator.Snapshot;
using FSG.MeshAnimator.ShaderAnimated;



#region 额外Class

/// <summary>
/// 敌人类型
/// </summary>
public enum EnemyType
{
    /// <summary>
    /// Boss猴王
    /// </summary>
    Boss_MonkeyKing,
    /// <summary>
    /// 远程猴子
    /// </summary>
    Monster_RemoteMonkey,
    /// <summary>
    /// 近战猴子
    /// </summary>
    Monster_CloseCombatMonkey,
}

/// <summary>
/// 敌人Info
/// </summary>
[System.Serializable]
public class EnemyInfo
{
    public enum TeamGroup 
    { 
        team01, team02, team03, team04, team05, team06, team07, team08, team09, team10,
        team11, team12, team13, team14, team15, team16, team17, team18, team19, team20,
    }


    public TeamGroup teamGroup;
    public EnemyType enemyType;
    public int Health;
}

#region AI状态-枚举

/// <summary>
/// AI状态类型
/// </summary>
public enum AIStateType
{
    None,           //无
    /// <summary>
    /// 闲置
    /// </summary>
    Idle,
    /// <summary>
    /// 警觉
    /// </summary>
    Alerted,
    /// <summary>
    /// 追踪
    /// </summary>
    Pursuit,
    /// <summary>
    /// 攻击
    /// </summary>
    Attack,
    /// <summary>
    /// 巡逻
    /// </summary>
    Patrol,
    /// <summary>
    /// 死亡
    /// </summary>
    Dead,
    /// <summary>
    /// 喂食
    /// </summary>
    Feeding,
    /// <summary>
    /// 玩耍
    /// </summary>
    Recreation,
    /// <summary>
    /// 战斗
    /// </summary>
    Fighting,
}

/// <summary>
/// AI目标类型
/// </summary>
public enum AITargetType
{
    [Description("无")]
    None,           //无
    Waypoint,       //路线
    /// <summary>
    /// 视觉威胁_Player
    /// </summary>
    Visual_Player,
    /// <summary>
    /// 视觉威胁_light
    /// </summary>
    Visual_Light,
    /// <summary>
    /// 视觉威胁_food
    /// </summary>
    Visual_Food,
    /// <summary>
    /// 声音威胁
    /// </summary>
    Audio
}

/// <summary>
/// AI触发事件类型
/// </summary>
public enum AITriggerEventType
{
    Enter,
    Stay,
    Exit
}

/// <summary>
/// AITarget结构类
/// </summary>
[System.Serializable]
public class AITarget
{
    [SerializeField] private AITargetType _type;
    [SerializeField] private Collider _collider;
    [SerializeField] private Vector3 _position;
    [SerializeField] private float _distance;
    [SerializeField] private float _time;

    public AITargetType type { get { return _type; } }

    public Collider collider { get { return _collider; } }

    public Vector3 position { get { return _position; } }

    public float distance { set { _distance = value; } get { return _distance; } }

    public float times { get { return _time; } }

    public void Set(AITargetType type , Collider collider , Vector3 position , float distance)
    {
        _type = type;
        _collider = collider;
        _position = position;
        _distance = distance;
        _time = Time.time;
    }

    public void Clear()
    {
        _type = AITargetType.None;
        _collider = null;
        _position = Vector3.zero;
        _distance = Mathf.Infinity;
        _time = 0.0f;
    }

}

#endregion

#endregion

/// <summary>
/// AI状态机
/// </summary>
public abstract class AIStateMachine : MonoBehaviour
{
    //public
    public EnemyInfo _enemyInfo;
    [Header("威胁"),Space(5)]
    public AITarget VisualThreat = new AITarget();  //视觉威胁
    public AITarget AudioThreat = new AITarget();   //声音威胁


    //protected
    protected Dictionary<AIStateType, AIState> _states = new Dictionary<AIStateType, AIState>();
    protected AITarget _target = new AITarget();
    protected AIState _currentState;
    [SerializeField] protected int _rootPositionRefCount = 0;
    [SerializeField] protected int _rootRotationRefCount = 0;


    // Protected Inspector Assigned
    [SerializeField] protected AIStateType _currentStateType = AIStateType.Idle;
    [SerializeField] protected AITargetType _currentTargetType = AITargetType.None;
    [SerializeField] protected SphereCollider _targetColiiderTrigger = null;  //目标触发
    [SerializeField] protected SphereCollider _sensorColiiderTrigger = null;  //警觉触发
    //[SerializeField] protected AINetWorkPoint _waypointNetwork = null;
    //[SerializeField] [Range(0, 15)] protected float _stoppingDistance = 1.0f;//这里直接使用NavAgent的stoppingDistance.
    protected bool _isTargetReached = false;

    // Component Cache
    protected Animator _anim;
    protected MeshAnimatorBase _meshAnimator;//网格状态机
    protected Rigidbody _rig;
    protected UnityEngine.AI.NavMeshAgent _agent;
    protected Collider _collider;
    protected CharacterController _cc;

    // Public Properties
    public Animator Anim { get { return _anim; } }
    public MeshAnimatorBase MeshAnimator { get { return _meshAnimator; } }//网格状态机
    public UnityEngine.AI.NavMeshAgent Agent { get { return _agent; } }
    public Rigidbody Rig { get { return _rig; } }
    public CharacterController CC { get { return _cc; } }
    public Vector3 sensorPosition
    {
        get
        {
            if (_sensorColiiderTrigger == null) return Vector3.zero;
            Vector3 point = _sensorColiiderTrigger.transform.position;
            point.x += _sensorColiiderTrigger.center.x * _sensorColiiderTrigger.transform.lossyScale.x;
            point.y += _sensorColiiderTrigger.center.y * _sensorColiiderTrigger.transform.lossyScale.y;
            point.z += _sensorColiiderTrigger.center.z * _sensorColiiderTrigger.transform.lossyScale.z;
            return point;
        }
    }
    public float sensorRadius
    {
        get
        {
            //求取,三坐标轴. 得出最佳 raduis
            if (_sensorColiiderTrigger == null) return 0.0f;
            float radius = Mathf.Max(_sensorColiiderTrigger.radius * _sensorColiiderTrigger.transform.lossyScale.x,
                          _sensorColiiderTrigger.radius * _sensorColiiderTrigger.transform.lossyScale.y);
            return Mathf.Max(radius, _sensorColiiderTrigger.radius * _sensorColiiderTrigger.transform.lossyScale.z);
        }
    }
    public bool useRootPosition { get { return _rootPositionRefCount > 0; } }
    public bool useRootRotation { get { return _rootRotationRefCount > 0; } }
    public AITargetType currentTargetType
    {
        get
        {
            _currentTargetType = _target.type;
            return _currentTargetType;
        }
    }
    public AITarget currentTarget { get { return _target; } }
    public Vector3 targetPosition { get { return _target.position; } }
    public bool isTargetReached { get { return _isTargetReached; } }
    public bool inMeleeRange { get; set; }
    public int currentID { get { return _targetColiiderTrigger.GetInstanceID(); } }
    [SerializeField] GameObject atkerGo;
    public GameObject attackerGo{ set { atkerGo = value; } get { return atkerGo; } }
    public AIStateType CurrentStateType { get => _currentStateType; set => _currentStateType = value; }

    protected virtual void Awake()
    {
        //为组件去赋值
        _anim = GetComponentInChildren<Animator>();
        _meshAnimator = GetComponentInChildren<MeshAnimatorBase>(true);
        _agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>(true);
        _rig = GetComponentInChildren<Rigidbody>();
        _collider = GetComponent<Collider>();
        _cc = GetComponent<CharacterController>();

        if (GameSceneManager.Instance != null)
        {
            //每一个物体的InstanceID在场景中是唯一的
            //注册到 [游戏场景管理]
            if (_collider) GameSceneManager.Instance.RegisterAiStateMachine(_collider.GetInstanceID(),this);
            if (_sensorColiiderTrigger) GameSceneManager.Instance.RegisterAiStateMachine(_sensorColiiderTrigger.GetInstanceID(),this);
        }
    }

    protected virtual void Start()
    {
        if (_sensorColiiderTrigger != null)
        {
            AISensor script = _sensorColiiderTrigger.GetComponent<AISensor>();
            if (script!=null)
            {
                script.SetAIStateMachine(this);
            }
        }

        AIState[] states = GetComponents<AIState>(); //我们在怪物本身上挂载,多个【AIState】来分别控制。

        foreach (AIState state in states)
        {
            if (state != null && !_states.ContainsKey(state.GetStateType()))
            {
                _states.Add(state.GetStateType(), state);
                state.SetAIStateMachine(this);
            }
        }
        //启用,当前state
        if (_states.ContainsKey(_currentStateType))
        {
            _currentState = _states[_currentStateType];
            _currentState.OnEnterState();
        }
        else
        {
            _currentState = null;
        }

        // 获取从动画器派生的所有AIStateMachineLink行为，
        // 并将它们的状态机引用设置为该状态机.
        if (_anim)
        {
            AIStateMachineLink[] scripts = _anim.GetBehaviours<AIStateMachineLink>();
            foreach (AIStateMachineLink script in scripts)
            {
                script.stateMachine = this;
            }
        }
    }


    protected virtual void FixedUpdate()
    {
        //Debug.LogError("是否； " + _agent.updatePosition);
        VisualThreat.Clear();
        AudioThreat.Clear();

        if (_target.type != AITargetType.None)
        {
            _target.distance = Vector3.Distance(transform.position , _target.position);
        }
    }

    protected virtual void Update()
    {
        if (_currentState == null)
            return;
        AIStateType newAIStateType = _currentState.OnUpdate();
        //每次,更新了[AIState] 并初始化掉,起始方法
        if (newAIStateType != _currentStateType)
        {
            AIState newState = null;
            if (_states.TryGetValue(newAIStateType,out newState))
            {
                _currentState.OnExitState();//当前状态退出
                newState.OnEnterState();    //new状态进入
                _currentState = newState;   //当前状态更新
            }
            //增加一层,检验层。状态不见,idle接上.
            else if (_states.TryGetValue(AIStateType.Idle, out newState))
            {
                _currentState.OnExitState();
                newState.OnEnterState();    
                _currentState = newState;   
            }

            _currentStateType = newAIStateType;
        }
    }

    protected virtual void LateUpdate() 
    {
        ////Debug.LogError("是否； " + _agent.updatePosition);
        //VisualThreat.Clear();
        //AudioThreat.Clear();

        //if (_target.type != AITargetType.None)
        //{
        //    _target.distance = Vector3.Distance(transform.position , _target.position);
        //}
    }

    public void SetTarget(AITargetType t, Collider c, Vector3 p, float d)
    {
        _target.Set(t, c, p, d);
        attackerGo = c.gameObject;

        if (_targetColiiderTrigger != null)
        {
            _targetColiiderTrigger.radius = Agent.stoppingDistance;
            _targetColiiderTrigger.transform.position = _target.position;
            _targetColiiderTrigger.enabled = true;
        }
    }

    public void SetTarget(AITarget t)
    {
        _target = t;
        attackerGo = t.collider.gameObject;

        if (_targetColiiderTrigger != null)
        {
            _targetColiiderTrigger.radius = Agent.stoppingDistance;
            _targetColiiderTrigger.transform.position = _target.position;
            _targetColiiderTrigger.enabled = true;
        }
    }

    public void ClearTarget()
    {
        _target.Clear();
        if (_targetColiiderTrigger != null)
        {
            _targetColiiderTrigger.enabled = false;
        }
    }

    //这个是本体的触发-Enter
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (_targetColiiderTrigger == null || _targetColiiderTrigger != other) return;

        if (_currentState != null)
        {
            _isTargetReached = true;
            _currentState.OnDestinationReached(true);//到达目的地
        }
    }
    //这个是本体的触发-Exit
    protected virtual void OnTriggerExit(Collider other)
    {
        if (_targetColiiderTrigger == null || _targetColiiderTrigger != other) return;

        if (_currentState != null)
        {
            _isTargetReached = false;
            _currentState.OnDestinationReached(false);//
        }
    }

    //AISensor触发
    public virtual void OnTriggerEvent(AITriggerEventType type,Collider other)
    {
        if (_currentState!=null) //当前,有状态。
        {
            _currentState.OnTriggerEvent(type, other);
        }
    }

    /// <summary>
    /// 用于修改根运动处理动画移动的回调,该回调将在每帧调用。
    /// <para> 在OnAnimatorIK之前调用</para>
    /// </summary>
    protected virtual void OnAnimatorMove()
    {
        if (_currentState != null) //当前,有状态。
        {
            _currentState.OnAnimatorUpdated();
        }
    }

    protected virtual void OnAnimatorIK(int layerIndex)
    {
        if (_currentState != null) //当前,有状态。
        {
            _currentState.OnAnimatorIKUpdated();
        }
    }

    /// <summary>
    /// NavAgent-寻路系统
    /// </summary>
    /// <param name="positionUpdate"></param>
    /// <param name="rotationUpdate"></param>
    public virtual void NavAgentControl(bool positionUpdate,bool rotationUpdate)
    {
        if (_agent != null) //当前,有状态。
        {
            _agent.updatePosition = positionUpdate;
            _agent.updateRotation = rotationUpdate;
        }
    }

    /// <summary>
    /// 动画-根运动
    /// </summary>
    /// <param name="rootPosition"></param>
    /// <param name="rootRotation"></param>
    public void AddRootMotionRequest(int rootPosition, int rootRotation)
    {
        _rootPositionRefCount += rootPosition;
        _rootRotationRefCount += rootRotation;
    }


    ///-------------------------------------我是分割线---------------------------------------

    #region 先注释掉.固定路径导航点
    //int _currentIndex = 0;
    //public Vector3 GetWayPointPosition(bool increment , bool isRandom = false)
    //{
    //    Transform target;
    //    float distance = 0;

    //    //随机,巡逻点.
    //    if (isRandom)
    //    {
    //        int randIndex = Random.Range(0 , _waypointNetwork.Points.Length);
    //        target = _waypointNetwork.Points[randIndex];
    //        distance = Vector3.Distance(transform.position , target.position);

    //        SetTarget(AITargetType.Waypoint , null , target.position , distance);
    //        return target.position;
    //    }
    //    else
    //    {
    //        //有递增,index++;
    //        if (increment)
    //        {
    //            _currentIndex++;
    //        }
    //        _currentIndex %= _waypointNetwork.Points.Length;
    //        target = _waypointNetwork.Points[_currentIndex];
    //        distance = Vector3.Distance(transform.position , target.position);
    //    }

    //    SetTarget(AITargetType.Waypoint , null , target.position , distance);
    //    return target.position;
    //}
    #endregion


}
