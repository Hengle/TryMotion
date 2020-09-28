/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-08-25 17:51:19
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyKingSkill_norAtk2 : MonkeyKingSkillBase
{
    public override SkillTriggerId SkillTriggerId
    {
        get
        {
            return SkillTriggerId.NormalAttack2;
        }
    }

    private BoxCollider boxCollider;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("触发.name = " + other.name);
    }

    public void OpenNorAttack2Tri()
    {
        boxCollider.enabled = true;
    }

    public void CloseNorAttack2Tri()
    {
        boxCollider.enabled = false;
    }
}
