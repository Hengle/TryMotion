using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 属性类



[System.Serializable]
public class BaseAttribute
{
    //等级
    public int LV;
    /// <summary>
    ///血量，魔量 
    /// </summary>
    public float HP, MP;
    /// <summary>
    ///攻击力， 防御力， 力量， 智力， 敏捷， 体力 
    /// </summary>
    public float Attack, Defend, Strong, Intelligence, Agility, Power;
    /// <summary>
    /// 攻击速度，散步速度，跑步速度，后退速度，转身移动速度，转身速度，攻击范围，经验值，暴击概率,暴击伤害百分比；
    /// </summary>
    public float AttackSpeed, MoveSpeed, RotationSpeed, AttackRange, Exp, CriticalRate, CriticalDamage;
    /// <summary>
    /// 每秒血量回复，每秒魔量回复
    /// </summary>
    public float HPRecoverPerSecond, MPRecoverPerSecond;
}

[System.Serializable]
public class AdditionAttribute
{
    /// <summary>
    ///血量，魔量 
    /// </summary>
    public float HP, MP;
    /// <summary>
    ///攻击力， 防御力， 力量， 智力， 敏捷， 体力 
    /// </summary>
    public float Attack, Defend, Strong, Intelligence, Agility, Power;
    /// <summary>
    /// 攻击速度，散步速度，跑步速度，后退速度，转身移动速度，转身速度，攻击范围，暴击概率,暴击伤害百分比；//暴击概率和攻击范围仅由装备影响。
    /// </summary>
    public float AttackSpeed, MoveSpeed, RotationSpeed, AttackRange, CriticalRate, CriticalDamage;

    /// <summary>
    /// 每秒血量回复，每秒魔量回复
    /// </summary>
    public float HPRecoverPerSecond, MPRecoverPerSecond;
}

[System.Serializable]
public class CurAttribute
{
    /// <summary>
    ///血量，魔量 
    /// </summary>
    public float CurHP, CurMP;
    /// <summary>
    ///攻击力， 防御力， 力量， 智力， 敏捷， 体力 
    /// </summary>
    public float CurAttack, CurDefend, CurStrong, CurIntelligence, CurAgility, CurPower;
    /// <summary>
    /// 当前攻击速度，当前散步速度，当前跑步速度，当前后退速度，当前转身移动速度，当前转身速度，当前攻击范围，当前暴击概率， 当前暴击伤害百分比；
    /// </summary>
    public float CurAttackSpeed, CurMoveSpeed, CurRotationSpeed, CurAttackRange, CurCriticalRate, CurCriticalDamage;

    /// <summary>
    /// 每秒血量回复，每秒魔量回复
    /// </summary>
    public float HPRecoverPerSecond, MPRecoverPerSecond;
}

[System.Serializable]
public class SumAttribute
{
    /// <summary>
    ///血量，魔量 
    /// </summary>
    public float HP, MP;
    /// <summary>
    ///攻击力， 防御力， 力量， 智力， 敏捷， 体力 
    /// </summary>
    public float Attack, Defend, Strong, Intelligence, Agility, Power;
    /// <summary>
    /// 攻击速度，移动速度，转身速度，攻击范围，暴击概率,暴击伤害百分比；
    /// </summary>
    public float AttackSpeed, MoveSpeed, RotationSpeed, AttackRange, CriticalRate, CriticalDamage;

    /// <summary>
    /// 每秒血量回复，每秒魔量回复
    /// </summary>
    public float HPRecoverPerSecond, MPRecoverPerSecond;
}

[System.Serializable]
public class AttributeGrowth
{
    /// <summary>
    ///血量，魔量 
    /// </summary>
    public float HP, MP;
    /// <summary>
    ///攻击力， 防御力， 力量， 智力， 敏捷， 体力 
    /// </summary>
    public float Attack, Defend, Strong, Intelligence, Agility, Power;
}

#endregion

public class T_PlayerStatus : MonoBehaviour
{

    /// <summary>
    /// 最高等级
    /// </summary>
    public int MaxLV;
    /// <summary>
    /// 当前等级所需要的经验总量
    /// </summary>
    public float NeedExpPerLV;
    //当前等级到下一个等级还需要的经验量
    public float EXP2NextLV;
    //到当前为止所获得的所有经验量
    public float MaxExp;
    //各类的实例对象：
    [Header("属性状态")]
    public BaseAttribute BaseStatus;
    public AdditionAttribute AddStatus;
    public CurAttribute CurStatus;
    public SumAttribute SumStatus;

    public AttributeGrowth attributeGrowth, growthPoint;
    //定义各种速度：
    public float RunSpeed;
    public float WalkSpeed;
    public float TurnSpeed;
    public float BackSpeed;

    /// <summary>
    /// 计算经验
    /// </summary>
    void CalculateExp()
    {
        //更新到当前等级为止的所有经验，包括本级到下一级
        MaxExp = 40.0f * BaseStatus.LV * BaseStatus.LV + 60.0f * BaseStatus.LV;
        //更新本级到下一级所需要的经验值；
        NeedExpPerLV = MaxExp - (40.0f * (BaseStatus.LV - 1) * (BaseStatus.LV - 1) + 60.0f * (BaseStatus.LV - 1));
    }

    /// <summary>
    /// 计算属性点成长性
    /// <para>属性中强壮部分决定了攻击力和血量上限</para>
    /// <para>体力部分决定了防御力</para>
    /// <para>智力部分决定了魔法值上限</para>
    /// <para>敏捷部分决定了各种速度等等</para>
    /// </summary>
    void CalculateAttributePointGrowth()
    {
        if (growthPoint.HP > 0)
        {
            attributeGrowth.HP += 2 * growthPoint.HP;

        }
        if (growthPoint.Attack > 0)
        {
            attributeGrowth.Attack += 2 * growthPoint.Attack;
        }
        if (growthPoint.MP > 0)
        {
            attributeGrowth.MP += 2 * growthPoint.MP;
        }
        if (growthPoint.Defend > 0)
        {
            attributeGrowth.Defend += 2 * growthPoint.Defend;

        }
        if (growthPoint.Strong > 0)
        {
            attributeGrowth.Strong += 2 * growthPoint.Strong;

        }
        if (growthPoint.Intelligence > 0)
        {
            attributeGrowth.Intelligence += 2 * growthPoint.Intelligence;
        }
        if (growthPoint.Agility > 0)
        {
            attributeGrowth.Agility += 2 * growthPoint.Agility;

        }
        if (growthPoint.Power > 0)
        {
            attributeGrowth.Power += 2 * growthPoint.Power;
        }
    }

    bool GrowthisAdd = false;
    /// <summary>
    /// 先判断当前等级是否大于最高等级，再判断当前经验是否达到当前等级所要求的经验量，如果达到就升级，并更新相关属性。升级时恢复血量和魔量到最大值。
    /// </summary>
    void UpdateAttribute()
    {
        if (!GrowthisAdd)
        {
            CalculateAttributePointGrowth();
            BaseStatus.HP += attributeGrowth.HP;
            BaseStatus.MP += attributeGrowth.MP;
            BaseStatus.Attack += attributeGrowth.Attack;
            BaseStatus.Defend += attributeGrowth.Defend;
            BaseStatus.Strong += attributeGrowth.Strong;
            BaseStatus.Intelligence += attributeGrowth.Intelligence;
            BaseStatus.Agility += attributeGrowth.Agility;
            BaseStatus.Power += attributeGrowth.Power;
            GrowthisAdd = true;
        }
        SumStatus.Strong = BaseStatus.Strong + AddStatus.Strong;
        SumStatus.Intelligence = BaseStatus.Intelligence + AddStatus.Intelligence;
        SumStatus.Agility = BaseStatus.Agility + AddStatus.Agility;
        SumStatus.Power = BaseStatus.Power + AddStatus.Power;

        SumStatus.Attack = BaseStatus.Attack + AddStatus.Attack + Mathf.FloorToInt(SumStatus.Strong * 0.2f);
        SumStatus.Defend = BaseStatus.Defend + AddStatus.Defend + Mathf.FloorToInt(SumStatus.Power * 0.2f);

        SumStatus.HP = BaseStatus.HP + AddStatus.HP + Mathf.FloorToInt(SumStatus.Strong * 0.4f);
        SumStatus.MP = BaseStatus.MP + AddStatus.MP + Mathf.FloorToInt(SumStatus.Intelligence * 0.4f);
        SumStatus.AttackSpeed = BaseStatus.AttackSpeed + AddStatus.AttackSpeed + Mathf.FloorToInt(SumStatus.Agility * 0.2f);
        SumStatus.MoveSpeed = BaseStatus.MoveSpeed + AddStatus.MoveSpeed + Mathf.FloorToInt(SumStatus.Agility * 0.3f);
        SumStatus.RotationSpeed = BaseStatus.RotationSpeed + AddStatus.RotationSpeed + Mathf.FloorToInt(SumStatus.Agility * 0.2f);
        SumStatus.CriticalRate = BaseStatus.CriticalRate + AddStatus.CriticalRate;
        SumStatus.CriticalDamage = BaseStatus.CriticalDamage + AddStatus.CriticalDamage;

        CurStatus.CurAttack = SumStatus.Attack;
        CurStatus.CurDefend = SumStatus.Defend;
        CurStatus.CurStrong = SumStatus.Strong;
        CurStatus.CurIntelligence = SumStatus.Intelligence;
        CurStatus.CurAgility = SumStatus.Agility;
        CurStatus.CurPower = SumStatus.Power;
        CurStatus.CurCriticalRate = SumStatus.CriticalRate;
        CurStatus.CurCriticalDamage = SumStatus.CriticalDamage;
    }

    //检测HP、MP的当前值是否超出规定，每帧调用
    void CheckHPMP()
    {
        //AnimatorStateInfo H_CurrentState = H_Animator.GetCurrentAnimatorStateInfo(0);
        //if (!H_CurrentState.IsName("Base.H_Death"))
        //{
        //    CurStatus.CurHP += Mathf.FloorToInt(BaseStatus.HPRecoverPerSecond * 0.33f);
        //    CurStatus.CurMP += Mathf.FloorToInt(BaseStatus.MPRecoverPerSecond * 0.50f);
        //}
        //else
        //{
        //    CurStatus.CurHP = 0;
        //    CurStatus.CurMP = 0;
        //}
        //if (CurStatus.CurHP < 0)
        //{
        //    CurStatus.CurHP = 0;
        //}
        //if (CurStatus.CurMP < 0)
        //{
        //    CurStatus.CurMP = 0;
        //}
        //if (CurStatus.CurHP >= SumStatus.HP)
        //{
        //    CurStatus.CurHP = SumStatus.HP;
        //}
        //if (CurStatus.CurMP >= SumStatus.MP)
        //{
        //    CurStatus.CurMP = SumStatus.MP;
        //}
    }


}
