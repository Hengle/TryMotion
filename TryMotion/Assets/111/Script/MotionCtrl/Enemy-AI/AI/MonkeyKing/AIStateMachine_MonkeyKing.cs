using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;

/// <summary>
/// 猴王AI状态机
/// </summary>
[RequireComponent(typeof(Animator)), RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(CharacterController))]
public class AIStateMachine_MonkeyKing : AIStateMachine
{


    //Inspector Assigned
    #region 敌人属性
    [Header("额外属性:")]
    [SerializeField] [Range(0f , 360f)] float _fov = 50;                        //视野夹角
    [SerializeField] [Range(0f , 1f)] float _sight = 1;                         //视野比例因素
    [SerializeField] [Range(0f , 1f)] float _hearing = 1;                       //听觉比例因素
    [SerializeField] [Range(2f , 5f)] float normalAtkDistance = 3;              //普攻攻击距离

    //[SerializeField] [Range(0f , 1f)] float _hearing = 1;      //听觉
    //[SerializeField] [Range(0f , 1f)] float _sight = 1;        //视觉
    //[SerializeField] [Range(0.0f , 1.0f)] float _aggression = 0.5f;//侵略
    //[SerializeField] [Range(0f , 1f)] float _intelligence = 1;       //智力
    //[SerializeField] [Range(0f , 1f)] float _satisfaction = 1;       //饥饿值(满足感)
    //[SerializeField] [Range(0f , 1f)] float _replenishRate = 0.5f;       //补充率
    //[SerializeField] [Range(0f , 1f)] float _depletionRate = 0.1f;       //消耗率
    #endregion
    [Header("基础属性")]
    [SerializeField] [Range(0f , 10000f)] int _health = 1;       //血量
    [SerializeField] [Range(0f , 100f)] int _attack;           //攻击
    [SerializeField] [Range(0f , 100f)] int _defence;          //防御
    [SerializeField] [Range(0f , 100f)] int _moveSpeed;        //移动速度

    [Header("动作属性:")]
    [SerializeField] bool _isSkillCold = false;
    [SerializeField] bool _isPlayingMotion = false;
    [SerializeField] bool _isPreparationFighting = false;
    [SerializeField] [Range(0f , 5f)] float _stoppingDistance = 0.5f;

    [Header("技能触发")]
    public Dictionary<SkillTriggerId, MonkeyKingSkillBase> _skillTri_Dict = new Dictionary<SkillTriggerId, MonkeyKingSkillBase>();


    // Private
    #region 僵尸属性
    //private int _seeking = 0;
    //private bool _feeding = false;
    //private int _feedingType = 1;
    //private bool _crawling = false;
    //private int _attackType = 0;
    //private float _speed = 0.0f;
    #endregion
    private int activityType = 0;
    private bool walk = false;
    private bool alert = false;
    private bool enterFighting = false;
    private bool exitFighting = false;
    private bool backJump = false;
    private bool leftMove = false;
    private bool rightMove = false;
    private bool chuiXion = false;
    private bool chongFeng = false;
    private bool isChongFengDmg = false;//如果冲锋有造成伤害.这召唤小怪
    private int normalAtk = 0;
    private bool huiChun = false;
    private bool haoLing = false;
    private bool taiTanZhiWo = false;

    private float sensorRaduis = 10;



    // Hashes
    #region 敌人属性
    //private int _speedHash = Animator.StringToHash("speed");
    //private int _seekingHash = Animator.StringToHash("seeking");
    //private int _feedingHash = Animator.StringToHash("feeding");
    //private int _feedingTypeHash = Animator.StringToHash("feeding_type");
    //private int _attackHash = Animator.StringToHash("attack");
    #endregion
    private int _activityHash = Animator.StringToHash("activityType");
    private int _walkHash = Animator.StringToHash("walk");
    private int _alertHash = Animator.StringToHash("alert");
    private int _enterFightingHash = Animator.StringToHash("enterFighting");
    private int _exitFightingHash = Animator.StringToHash("exitFighting");
    private int _backJumpHash = Animator.StringToHash("backJump");
    private int _leftMoveHash = Animator.StringToHash("leftMove");
    private int _rightMoveHash = Animator.StringToHash("rightMove");
    private int _chuiXionHash = Animator.StringToHash("chuiXion");
    private int _chongFengHash = Animator.StringToHash("chongFeng");
    private int _isChongFengDmgHash = Animator.StringToHash("isChongFengDmg");
    private int _normalAtkHash = Animator.StringToHash("normalAtk");
    private int _huiChunHash = Animator.StringToHash("huiChun");
    private int _haoLingHash = Animator.StringToHash("haoLing");
    private int _taiTanZhiWoHash = Animator.StringToHash("taiTanZhiWo");

    // Public Properties
    #region 敌人属性
    //public float replenishRate { get { return _replenishRate; } }0.4


    //public bool crawling { get { return _crawling; } }
    //public float intelligence { get { return _intelligence; } }
    //public float satisfaction { get { return _satisfaction; } set { _satisfaction = value; } }
    //public float aggression { get { return _aggression; } set { _aggression = value; } }
    //public int attackType { get { return _attackType; } set { _attackType = value; } }
    //public bool feeding { get { return _feeding; } set { _feeding = value; } }
    //public int feedingType { get { return _feedingType; } set { _feedingType = value; } }
    //public int seeking { get { return _seeking; } set { _seeking = value; } }
    //public float speed
    //{
    //    get { return _speed; }
    //    set
    //    {
    //        _speed = value;
    //        //Agent.speed = _speed;
    //    }
    //}
    #endregion

    //基础属性
    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
        }
    }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int Defence { get { return _defence; } set { _defence = value; } }
    public int MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    //额外因素
    public float Sight { get { return _sight; } }
    public float Fov { get { return _fov; } }
    public float Hearing { get { return _hearing; } }
    public float NormalAtkDistance { get => normalAtkDistance; set => normalAtkDistance = value; }
    //动作因素
    public bool IsSkillCold { get { return _isSkillCold; } set { _isSkillCold = value; } }
    public bool IsPlayingMotion { get { return _isPlayingMotion; } set { _isPlayingMotion = value; } }
    /// <summary>
    /// 是否处于战斗预备
    /// </summary>
    public bool IsPreparationFighting { get { return _isPreparationFighting; } set { _isPreparationFighting = value; } }
    public float StoppingDistance { get => _stoppingDistance; set { Agent.stoppingDistance = value; _stoppingDistance = value; } }

    /// <summary>
    /// 实际触发
    /// <para>方便得到、计算</para>
    /// </summary>
    public float SensorRaduis
    {
        get { return sensorRaduis; }
        set
        {
            if (sensorRaduis != value)
            {
                sensorRaduis = value;
                _sensorColiiderTrigger.radius = sensorRaduis;
            }
        }
    }

    //实际控制动画
    public int ActivityType { get => activityType; set => activityType = value; }
    public bool Walk { get => walk; set => walk = value; }
    public bool Alert { get => alert; set => alert = value; }
    public bool EnterFighting { get => enterFighting; set => enterFighting = value; }
    public bool ExitFighting { get => exitFighting; set => exitFighting = value; }
    public bool BackJump { get => backJump; set => backJump = value; }
    public bool LeftMove { get => leftMove; set => leftMove = value; }
    public bool RightMove { get => rightMove; set => rightMove = value; }
    public bool ChuiXion { get => chuiXion; set => chuiXion = value; }
    public bool ChongFeng { get => chongFeng; set { chongFeng = value; } }
    public bool IsChongFengDmg { get => isChongFengDmg; set => isChongFengDmg = value; }
    public int NormalAtk { get => normalAtk; set => normalAtk = value; }
    public bool HuiChun { get => huiChun; set => huiChun = value; }
    public bool HaoLing { get => haoLing; set => haoLing = value; }
    public bool TaiTanZhiWo { get => taiTanZhiWo; set => taiTanZhiWo = value; }

    [Header("技能触发类:")]
    public List<MonkeyKingSkillBase> skillTriggerList = new List<MonkeyKingSkillBase>();

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        //对应的技能触发.
        for (int i = 0; i < skillTriggerList.Count; i++)
        {
            skillTriggerList[i].King = this;
            _skillTri_Dict.Add(skillTriggerList[i].SkillTriggerId, skillTriggerList[i]);
        }
    }



    // ---------------------------------------------------------
    // Name	:	Update
    // Desc	:	用最新的值刷新动画器
    // ---------------------------------------------------------
    protected override void Update()
    {
        base.Update();

        if (_anim != null)
        {
            #region 僵尸属性
            //_anim.SetFloat(_speedHash, _speed);
            //_anim.SetBool(_feedingHash, _feeding);
            //_anim.SetInteger(_feedingTypeHash, _feedingType );
            //_anim.SetInteger(_seekingHash, _seeking);
            //_anim.SetInteger(_attackHash, _attackType);
            #endregion

            _anim.SetBool(_walkHash , Walk);
            _anim.SetInteger(_activityHash , ActivityType);
            _anim.SetBool(_alertHash , Alert);
            _anim.SetBool(_enterFightingHash , EnterFighting);
            _anim.SetBool(_exitFightingHash , ExitFighting);
            _anim.SetBool(_backJumpHash , BackJump);
            _anim.SetBool(_leftMoveHash , LeftMove);
            _anim.SetBool(_rightMoveHash , RightMove);
            _anim.SetBool(_chuiXionHash , ChuiXion);
            _anim.SetBool(_chongFengHash , ChongFeng);
            _anim.SetBool(_isChongFengDmgHash , IsChongFengDmg);
            _anim.SetInteger(_normalAtkHash , NormalAtk);
            _anim.SetBool(_huiChunHash , HuiChun);
            _anim.SetBool(_haoLingHash , HaoLing);
            _anim.SetBool(_taiTanZhiWoHash , TaiTanZhiWo);
        }


        //_satisfaction -= Mathf.Max(0 , 0.05f * Time.deltaTime * animationCurve.Evaluate(speed));

        //_satisfaction = Mathf.Max(0, _satisfaction - ((_depletionRate * Time.deltaTime) / 100.0f) * Mathf.Pow(_speed, 3.0f));
    }


    /*--------
    -------- 查询方法 --------
    -------- 🐟🐟🐟 --------
    -------- 🐟🐟🐟 --------
    ------*/

    //得到skillTri by id
    public MonkeyKingSkillBase GetSkillTriggerById(SkillTriggerId triggerId)
    {
        _skillTri_Dict.TryGetValue(triggerId, out MonkeyKingSkillBase triSkill);
        return triSkill;
    }

}

