/* ========================================================
*      作 者：Lin
*      主 题：Fsm状态机
*      主要功能：

*      详细描述：

*      创建时间：2020-09-22 10:15:52
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AIStateMachine_RemoteMonkey : AIStateMachine
{

    [Header("额外属性:")]
    [SerializeField] [Range(0f , 360f)] float _fov = 50;                        //视野夹角
    [SerializeField] [Range(0f , 1f)] float _sight = 1;                         //视野比例因素
    [SerializeField] [Range(0f , 1f)] float _hearing = 1;                       //听觉比例因素
    [SerializeField] [Range(2f , 5f)] float _normalAtkDistance = 3;             //普攻攻击距离
    [SerializeField] [Range(0 , 10)] float _sensorRaduis;                       //触发范围

    //额外因素
    public float Sight { get { return _sight; } }
    public float Fov { get { return _fov; } }
    public float Hearing { get { return _hearing; } }
    public float NormalAtkDistance { get => _normalAtkDistance; set => _normalAtkDistance = value; }
    /// <summary>
    /// 实际触发
    /// <para>方便得到、计算</para>
    /// </summary>
    public float SensorRaduis
    {
        get { return _sensorRaduis; }
        set
        {
            if (_sensorRaduis != value)
            {
                _sensorRaduis = value;
                _sensorColiiderTrigger.radius = _sensorRaduis;
            }
        }
    }

    /// <summary>
    /// 当前播放的动画名称
    /// </summary>
    public MeshAnimationName.RemoteGold CurrentAnimationName = MeshAnimationName.RemoteGold.idle;
    /// <summary>
    /// 当前动画过渡因子
    /// </summary>
    public float CurrentCrossFadeFactor = 0.1f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        if (_meshAnimator != null)
        {
            _meshAnimator.Crossfade(CurrentAnimationName.ToString() , CurrentCrossFadeFactor);
        }
    }


}

/// <summary>
/// 网格动画名称
/// </summary>
public static class MeshAnimationName
{
    /// <summary>
    /// 远程金
    /// </summary>
    public enum RemoteGold 
    {
        /// <summary>
        /// 待机
        /// </summary>
        idle,
        /// <summary>
        /// 受击1
        /// </summary>
        takeDamage1,
        /// <summary>
        /// 受击2
        /// </summary>
        takeDamage2,
        /// <summary>
        /// 受击3
        /// </summary>
        takeDamage3,
        /// <summary>
        /// 战斗待机
        /// </summary>
        fightIdle,
        /// <summary>
        /// 技能-木
        /// </summary>
        skill_Wood,
        /// <summary>
        /// 技能-土
        /// </summary>
        skill_Earth,
        /// <summary>
        /// 技能-金
        /// </summary>
        skill_Gold,
        /// <summary>
        /// 挑衅
        /// </summary>
        defiant,
        /// <summary>
        /// 死亡
        /// </summary>
        death,
        /// <summary>
        /// 活动1
        /// </summary>
        recreation1,
        /// <summary>
        /// 活动2
        /// </summary>
        recreation2,
        /// <summary>
        /// 移动
        /// </summary>
        move,
        /// <summary>
        /// 观望
        /// </summary>
        lookAround,
        /// <summary>
        /// 逃脱
        /// </summary>
        escape
    }
}