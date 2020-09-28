using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonkeyKingSkill_ChongFeng : MonkeyKingSkillBase
{

    public override SkillTriggerId SkillTriggerId
    {
        get
        {
            return SkillTriggerId.ChongFeng;
        } 
    }

    private BoxCollider boxCollider;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            King.IsChongFengDmg = true;
            Debug.LogError("播放角色受击动画,位移！");
        }

    }

    public void OpenChongFengTri() 
    {
        boxCollider.enabled = true;
    }

    public void CloseChongFengTri()
    {
        boxCollider.enabled = false;
        King.IsChongFengDmg = false;
    }
}
