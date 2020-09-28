using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMonkeyKingState_TestIK : MonoBehaviour
{
    #region 属性

    private Animator anim;
    public Animator Anim
    {
        get
        {
            if (anim == null)
            {
                anim = GetComponent<Animator>();
            }
            return anim;
        }
    }

    #endregion

    public Transform leftHand;
    public Transform rightHand;

    void Start()
    {

    }

    bool holdLog = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            holdLog = !holdLog;
            if (holdLog)
            {
                Anim.SetBool("HoldLog",true);
            }
            else
            {
                Anim.SetBool("HoldLog" , false);
            }
        }
    }

    void OnAnimatorIK(int layerIndex) 
    {
        Debug.LogError(layerIndex);

        if (holdLog)
        {
            //设置 IK权重
            Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand , 1f);
            Anim.SetIKRotationWeight(AvatarIKGoal.LeftHand , 1f);
            Anim.SetIKPositionWeight(AvatarIKGoal.RightHand , 1f);
            Anim.SetIKRotationWeight(AvatarIKGoal.RightHand , 1f);

            //设置 IK
            Anim.SetIKPosition(AvatarIKGoal.LeftHand , leftHand.position);
            Anim.SetIKRotation(AvatarIKGoal.LeftHand , leftHand.rotation);
            Anim.SetIKPosition(AvatarIKGoal.RightHand , rightHand.position);
            Anim.SetIKRotation(AvatarIKGoal.RightHand , rightHand.rotation);
        }
        else
        {
            Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand , 0);
            Anim.SetIKRotationWeight(AvatarIKGoal.LeftHand , 0);
            Anim.SetIKPositionWeight(AvatarIKGoal.RightHand , 0);
            Anim.SetIKRotationWeight(AvatarIKGoal.RightHand , 0);
        }
       

    }


}
