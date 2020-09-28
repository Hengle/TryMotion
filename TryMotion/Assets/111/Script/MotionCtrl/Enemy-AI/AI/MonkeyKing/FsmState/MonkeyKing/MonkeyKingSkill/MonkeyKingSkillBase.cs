/* ========================================================
*      作 者：Lin 
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-08-20 10:32:58
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能触发ID
/// </summary>
public enum SkillTriggerId
{
    ChongFeng,
    NormalAttack1,
    NormalAttack2,
}

public class MonkeyKingSkillBase : MonoBehaviour
{
    public virtual SkillTriggerId SkillTriggerId { get; }
    
    private AIStateMachine_MonkeyKing king;
    public AIStateMachine_MonkeyKing King { get => king; set => king = value; }
}
