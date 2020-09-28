/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-22 15:24:16
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 远程猴子(金) 活动状态
/// </summary>
public class AIRMonkeyGoldState_Recreation1 : AIRemoteMonkeyGoldState
{
    //private
    private bool isExitRecreation = false;//退出娱乐状态

    public override AIStateType GetStateType()
    {
        return AIStateType.Recreation;
    }

    public override void OnEnterState()
    {
        base.OnEnterState();

        //监听MeshAnimator事件
        _enemyStateMachine.MeshAnimator.OnAnimationFinished += OnAnimationFinished;

        Recreation();
    }


    public override AIStateType OnUpdate()
    {
        //受到伤害
        if (isByDamage)
        {
            isByDamage = false;
            return AIStateType.Alerted;
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

        //退出娱乐状态
        if (isExitRecreation)
        {
            isExitRecreation = false;
            return AIStateType.Idle;
        }


        return AIStateType.Recreation;
    }

    public override void OnExitState()
    {
        base.OnExitState();


    }

    protected override void ResetAIState()
    {
        
    }


    //\----------------------------------跳上这快车终点方向未明------------------------------------


    /// <summary>
    /// 傻猴娱乐
    /// </summary>
    void Recreation() 
    {
        int random = UnityEngine.Random.Range(0 , 100);
        bool activity1 = random >= 50 ? true : false;
        if (activity1)//播放活动1
        {
            _enemyStateMachine.CurrentAnimationName = MeshAnimationName.RemoteGold.recreation1;
        }
        else
        {
            _enemyStateMachine.CurrentAnimationName = MeshAnimationName.RemoteGold.recreation2;
        }
    }

    #region MeshAnimator事件监听

    //动画播放完毕
    private void OnAnimationFinished(string aniName)
    {
        if (aniName == MeshAnimationName.RemoteGold.recreation1.ToString()||
            aniName == MeshAnimationName.RemoteGold.recreation2.ToString())
        {
            //傻猴娱乐完毕.
            isExitRecreation = true;
        }
    }

    #endregion
}
