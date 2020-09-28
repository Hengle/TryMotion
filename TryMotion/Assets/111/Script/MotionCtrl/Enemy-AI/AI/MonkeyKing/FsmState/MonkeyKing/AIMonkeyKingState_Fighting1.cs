using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 猴王AI-战斗状态
/// </summary>
public class AIMonkeyKingState_Fighting1 : AIBossMonkeyKingState
{
    [Header("旋转、位移参数:")]
    [SerializeField] float skillColdTime = 5;
    [SerializeField] float _slerpSpeed = 5.0f;
    [SerializeField] float _attackDis = 3;

    [Header("被动动画概率区间"),Space(5)]
    [SerializeField] Vector2 passiveProbability = new Vector2(0.3f , 0.7f);//被动动画
    //0.3f概率
    [SerializeField] Vector2 backJumppProbability = new Vector2(0.2f , 0.8f);//重置攻击

    //0.7f概率
    [SerializeField] bool isTeam2Player;//是否拥有两个以上的队友
    bool IsTeam2Player 
    {
        get 
        {
           isTeam2Player = CheckCountForTeam(_enemyStateMachine._enemyInfo.teamGroup) >= 2 ? true : false;
           return isTeam2Player;
        }
    }

    public override void SetAIStateMachine(AIStateMachine aIStateMachine)
    {
        base.SetAIStateMachine(aIStateMachine);
    }

    private void Start()
    {
        Event_EnemyInfo();
    }

    #region >>>>>>>>事件监听

    //敌人死亡
    void Event_EnemyInfo() 
    {
        EnemyDieDispatcher.Instance.AddEventListener(ConstKey_EnemyInfo.OnEnemyDie , OnEnemyDie);
    }
    private void OnEnemyDie(object[] p)
    {

    }

    //受到伤害事件
    protected override void OnTakeDamageEvent(object[] objs)
    {
        base.OnTakeDamageEvent(objs);
        if (_enemyStateMachine.CurrentStateType == AIStateType.Fighting)
        {
            if (_enemyStateMachine.IsSkillCold && !_enemyStateMachine.IsPreparationFighting)
            {
                _enemyStateMachine.IsPreparationFighting = true;
                isByDamage = true;
                bool howToDo = AIState.CalcProbability(passiveProbability);
                if (howToDo)
                {
                    PreparationFighting1();//进入预备战斗1 (70%)
                }
                else
                {
                    PreparationFighting2();//进入预备战斗2 (30%)
                }
            }
        }
    }

    #endregion

    public override AIStateType GetStateType()
    {
        return AIStateType.Fighting;
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        StartCoroutine(CalcSkillCold());


        //猴王进入战斗状态-播放站立动画
        _enemyStateMachine.EnterFighting = true;
        _enemyStateMachine.ExitFighting = false;
        //_enemyStateMachine.Walk = false;
        _enemyStateMachine.NavAgentControl(true, false);
        _enemyStateMachine.IsPreparationFighting = false;
        //战斗预备
        //PreparationFighting1();
    }

    public override AIStateType OnUpdate()
    {
        //if (_enemyStateMachine.ChongFeng) { return AIStateType.Fighting; }
        if (_enemyStateMachine.IsPlayingMotion) { return AIStateType.Fighting; } 

        //攻击目标选择：将使自身进入战斗状态的敌 人作为攻击目标，每次攻击后判定10m范围内
        //（玩家 > 血量较高 > 距离近的）选取哪个敌人为攻击目标，若与攻击目标距离大于10m
        //则清除攻击目标
        if (_enemyStateMachine.attackerGo == null) { Debug.LogError("丢失攻击目标!"); return AIStateType.Alerted; }
        else
        {
            //atkerGo脱离战斗!
            float dis = Vector3.Distance(_enemyStateMachine.attackerGo.transform.position , transform.position);
            if (dis >= 10)
            {
                Debug.LogError("距离超出10m,脱离战斗State!");
                _enemyStateMachine.attackerGo = null;
                return AIStateType.Alerted;
            }
        }
        //if (_enemyStateMachine.IsPlayingMotion) { return AIStateType.Fighting; }//当处于动画播放中.不在计算处理逻辑.

        //技能不在冷却
        if (!_enemyStateMachine.IsSkillCold)
        {   //是否拥有两个以上的队友
            if (IsTeam2Player)
            {
                //是否释放回春技能
                if (IsReleaseHuiChun())
                {
                    _enemyStateMachine.IsSkillCold = true;
                }
                else
                {
                    //if (Vector3.Distance(transform.position, _enemyStateMachine.attackerGo.transform.position) < 5) { return AIStateType.Fighting; } ;
                    bool useSkill = AIState.CalcProbability(new Vector2(0.4F,0.5F));//是否释放奔袭。
                    if (useSkill)
                    {
                        int value = UnityEngine.Random.Range(0, 100);
                        if (0 <= value && value < 40) { _enemyStateMachine.HaoLing = true; _enemyStateMachine.IsSkillCold = true; }//释放号令，进入攻击冷却5秒
                        else if (40 <= value && value < 80)
                        {
                            while (true)
                            {
                                _enemyStateMachine.SetTarget(_enemyStateMachine.VisualThreat);
                                _enemyStateMachine.Walk = true;
                                if (Vector3.Distance(transform.position, _enemyStateMachine.attackerGo.transform.position) < _attackDis)
                                {
                                    RandomNormalAtk();
                                    _enemyStateMachine.IsSkillCold = false;
                                    break;
                                }
                            }
                            _enemyStateMachine.IsSkillCold = true;
                        }//靠近至普攻范围，释放普攻并进入攻击冷却5s
                        else { _enemyStateMachine.HaoLing = true; _enemyStateMachine.IsSkillCold = true; }//释放泰坦之握并进入攻击冷却6s
                    }
                    else
                    {
                        _enemyStateMachine.ChongFeng = true;
                        _enemyStateMachine.IsSkillCold = true;
                    }
                }
            }
            else
            {
                float dis = Vector3.Distance(transform.position , _enemyStateMachine.attackerGo.transform.position);
                //1.距离  dis >= 5
                if (dis >= 5)
                {
                    _enemyStateMachine.Walk = false;
                    if (!_enemyStateMachine.ChongFeng) { _enemyStateMachine.ChongFeng = true; }//发动冲锋
                }
                //2.距离  2.5<= dis <5
                else if (_enemyStateMachine.NormalAtkDistance <= dis && dis < 5)//追击
                {
                    Debug.LogError("距离  2.5<= dis <5");
                    _enemyStateMachine.Walk = true;
                }
                //3.距离 dis < 2.5
                else
                {
                    _enemyStateMachine.Walk = false;
                    int value = UnityEngine.Random.Range(0,100);
                    if (value <= 40) { _enemyStateMachine.TaiTanZhiWo = true; }
                    else { RandomNormalAtk(); }
                }
            }
        }
        return AIStateType.Fighting;
    }

    public override void OnExitState()
    {
        base.OnExitState();

        _enemyStateMachine.BackJump = false;
        _enemyStateMachine.LeftMove = false;
        _enemyStateMachine.RightMove = false;
        _enemyStateMachine.EnterFighting = false;
        _enemyStateMachine.ExitFighting = true;
        _enemyStateMachine.ChuiXion = false;
        _enemyStateMachine.ChongFeng = false;
        _enemyStateMachine.HuiChun = false;
        _enemyStateMachine.Walk = false;
        _enemyStateMachine.NormalAtk = 0;
        _enemyStateMachine.attackerGo = null;

        _enemyStateMachine.SensorRaduis = 5;//退出战斗状态 触发器半径5m

        Debug.LogError("退出战斗状态！");

    }


    /// <summary>
    /// 到达NavAgent导航位置
    /// </summary>
    public override void OnDestinationReached(bool isReached)
    {
        base.OnDestinationReached(isReached);
    }

    /// <summary>
    /// 重置AIState
    /// </summary>
    protected override void ResetAIState()
    {
        _enemyStateMachine.EnterFighting = false;
    }


    #region 释放 攻击/技能

    /// <summary>
    /// 随机普攻
    /// </summary>
    void RandomNormalAtk()
    {
        int value = UnityEngine.Random.Range(1 , 3);
        if (value == 1) { _enemyStateMachine.NormalAtk = 1; }
        else { _enemyStateMachine.NormalAtk = 2; }
    }

    /// <summary>
    /// 释放释放回春技能
    /// </summary>
    bool IsReleaseHuiChun() 
    {
        //1.小组成员中有一个血量低于50%,释放回春技能
        List<EnemyInfo> infos = CheckEnemyInfoForTeam(_enemyStateMachine._enemyInfo.teamGroup);
        for (int i = 0; i < infos.Count; i++)
        {
            
            if (infos[i].Health < infos[i].Health*0.5f)
            {
                _enemyStateMachine.HuiChun = true;  return true;
            }
        }
        //2.包括自身在内总体当前血量是否小于总体血量上限60%,释放回春技能
        int totalHealth = 0;
        for (int i = 0; i < infos.Count; i++) { totalHealth += infos[i].Health; }
        if (_enemyStateMachine._enemyInfo.Health <= totalHealth * 0.6f)
        {
            _enemyStateMachine.HuiChun = true;      return true;
        }
        return false;
    }


    /// <summary>
    /// 战斗预备1
    /// <para>包括:左移、右移、战斗站立、捶胸 （攻击冷却清空）</para>
    /// </summary>
    void PreparationFighting1()
    {
        //计算[左跳/右跳/Idle/捶胸]概率
        int value = UnityEngine.Random.Range(0, 100);
        if (0 <= value && value < 20)
        {//左跳
            _enemyStateMachine.LeftMove = true;
        }
        else if (20 <= value && value < 40)
        {//右跳
            _enemyStateMachine.RightMove = true;
        }
        else if (40 <= value && value < 70)
        {//Idle
            //保持着猴王站立动画!
        }
        else if (70 <= value && value < 100)
        {//捶胸
            _enemyStateMachine.ChuiXion = true;
        }
        Debug.LogError("战斗预备1,value= " + value);
    }
    /// <summary>
    /// 战斗预备2
    /// <para>包括:后跳 （攻击冷却清空）</para>
    /// </summary>
    void PreparationFighting2()
    {
        //计算[后跳/攻击冷却清零]概率
        bool isEnterBackJump = AIState.CalcProbability(backJumppProbability);
        if (isEnterBackJump) { _enemyStateMachine.BackJump = true; }
        else { _enemyStateMachine.IsSkillCold = false;  }
        Debug.LogError("战斗预备2");
    }

    #endregion


    /// <summary>
    /// 开启技能冷却
    /// </summary>
    public void OpenSkillCold() 
    {
        
    }

    /// <summary>
    /// 关闭技能冷却
    /// </summary>
    public void CloseSkillCold() 
    {
    
    }

    /// <summary>
    /// 重置技能冷却
    /// </summary>
    public void ResetSkillCold() 
    {
        timer = 0;
    }

    [SerializeField,Header("技能计时器:")]float timer = 0;
    /// <summary>
    /// 计算技能冷却
    /// </summary>
    IEnumerator CalcSkillCold() 
    {
        while (true)
        {
            //技能正在冷却.
            //播放攻击动画.不算入技能冷却.
            if (_enemyStateMachine.IsSkillCold && !_enemyStateMachine.IsPlayingMotion)
            {
                timer += Time.fixedDeltaTime;
                if (timer >= skillColdTime)
                {
                    _enemyStateMachine.IsSkillCold = false;
                    timer = 0;
                }
            }
            yield return null;
        }
    }

    #region 检测

    /// <summary>
    /// 检测小队数量
    /// </summary>
    public int CheckCountForTeam(EnemyInfo.TeamGroup teamGroup)
    {
        int count = 0;
        Test_EnemySpawn spawn = Test_EnemySpawn.Instance;
        for (int i = 0; i < spawn.currentEnemyList.Count; i++)
        {
            if (spawn.currentEnemyList[i].teamGroup == teamGroup)
            {
                count++;
            }
        }
        return count;
    }
    List<EnemyInfo> enemyInfoList = new List<EnemyInfo>();
    /// <summary>
    /// 检测小队血量
    /// </summary>
    public List<EnemyInfo> CheckEnemyInfoForTeam(EnemyInfo.TeamGroup teamGroup)
    {
        enemyInfoList.Clear();
        Test_EnemySpawn spawn = Test_EnemySpawn.Instance;
        for (int i = 0; i < spawn.currentEnemyList.Count; i++)
        {
            if (spawn.currentEnemyList[i].teamGroup == teamGroup)
            {
                enemyInfoList.Add(spawn.currentEnemyList[i]);
            }
        }
        return enemyInfoList;
    }


    #endregion


    /*-------------------------------------  外部调用 -------------------------------------*/


    /*------------------------------------------------------------------------------------*/


}
