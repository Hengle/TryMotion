/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-08-25 17:50:34
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyKingSkill_norAtk1 : MonkeyKingSkillBase
{

    public override SkillTriggerId SkillTriggerId
    {
        get
        {
            return SkillTriggerId.NormalAttack1;
        }
    }

    private BoxCollider boxCollider;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.LogError("触发.name = " + other.name);
    }

    public void OpenNorAttack1Tri()
    {
        boxCollider.enabled = true;
    }

    public void CloseNorAttack1Tri()
    {
        boxCollider.enabled = false;
    }
}
