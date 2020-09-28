/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-24 09:41:05
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动画参数类型
/// </summary>
public enum AnimEnumType 
{
    //Death,
    //Forward,
    //Turn,
    //Hit,
    //Jump,
    //IsOnGround,
    //InArrowFight_Enter,
    //InArrowFight_Exit,
    //ArrowAttack,
    //InSwordFight_Enter,
    //InSwordFight_Exit,
    //SwordAttack,
    //SwordNorAttackType,
}

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _anim;
    public bool InArrowFight;
    public bool InSwordFight;

    //具体数值
    public float Forward { get; set; }
    public float Turn { get; set; }
    public bool Jump { get; set; }
    public bool Earth { get; set; }
    public bool Hit { get; set; }
    public bool IsOnGround { get; set; }
    public bool InArrowFight_Enter { set; get; }
    public bool InArrowFight_Exit { set; get; }
    public bool InSwordFight_Enter { get; set; }
    public bool InSwordFight_Exit { get; set; }
    public int ArrowAttack { get; set; }
    public int SwordAttack { get; set; }
    public int SwordNorAttackType { get; set; }

    //animator-hash
    //common
    private int death = Animator.StringToHash("Death");
    private int forward = Animator.StringToHash("Forward");
    private int turn = Animator.StringToHash("Turn");
    private int hit = Animator.StringToHash("Hit");
    private int jump = Animator.StringToHash("Jump");
    private int isOnGround = Animator.StringToHash("IsOnGround");
    //arrow-fight
    private int inArrowFight_Enter = Animator.StringToHash("InArrowFight_Enter");
    private int inArrowFight_Exit = Animator.StringToHash("InArrowFight_Exit");
    private int arrowFight_ArrowAttack = Animator.StringToHash("ArrowAttack");
    //sword-fight
    private int inSwordFight_Enter = Animator.StringToHash("InSwordFight_Enter");
    private int inSwordFight_Exit = Animator.StringToHash("InSwordFight_Exit");
    private int swordFight_SwordAttack = Animator.StringToHash("SwordAttack");
    private int swordFight_SwordNorAttackType = Animator.StringToHash("SwordNorAttackType");


    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Start()
    {

    }


    void Update()
    {

        _anim.SetFloat(forward , Forward , 0.3f , 0.1f);
        _anim.SetFloat(turn , Turn , 0.5f , 0.02f);
        _anim.SetBool(jump , Jump);
        _anim.SetBool(death , Earth);
        _anim.SetBool(hit , Hit);
        _anim.SetBool(isOnGround , IsOnGround);
        _anim.SetBool(inArrowFight_Enter , InArrowFight_Enter);
        _anim.SetBool(inArrowFight_Exit , InArrowFight_Exit);
        _anim.SetBool(inSwordFight_Enter , InSwordFight_Enter);
        _anim.SetBool(inSwordFight_Exit , InSwordFight_Exit);
        _anim.SetInteger(arrowFight_ArrowAttack , ArrowAttack);
        _anim.SetInteger(swordFight_SwordAttack , SwordAttack);
        _anim.SetInteger(swordFight_SwordNorAttackType , SwordNorAttackType);
    }
}
