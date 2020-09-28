/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-25 15:57:38
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponMgr : MonoBehaviour
{
    public Button arrowBtn;
    public Button swordBtn;
    public Button nonFightBtn;

    [Header("角色")]
    public PlayerAnimationController aimController;

    // Start is called before the first frame update
    void Start()
    {
        arrowBtn.onClick.AddListener(OnArrowSwitch);
        swordBtn.onClick.AddListener(OnSwordSwitch);
        nonFightBtn.onClick.AddListener(ResetSwitch);

    }

    private void OnSwordSwitch()
    {
        aimController.InSwordFight_Enter = true;
        aimController.InSwordFight_Exit = false;
        aimController.InArrowFight_Enter = false;
        aimController.InArrowFight_Exit = true;
        //标记一下 处于持剑模式.
        aimController.InSwordFight = true;
        aimController.InArrowFight = false;
        StartCoroutine(Delay2False());
    }

    private void OnArrowSwitch()
    {
        aimController.InSwordFight_Enter = false;
        aimController.InSwordFight_Exit = true;
        aimController.InArrowFight_Enter = true;
        aimController.InArrowFight_Exit = false;
        //标记一下 处于弓箭模式.
        aimController.InArrowFight = true;
        aimController.InSwordFight = false;
        StartCoroutine(Delay2False());
    }

    void ResetSwitch() 
    {
        aimController.InSwordFight_Enter = false;
        aimController.InSwordFight_Exit = false;
        aimController.InArrowFight_Enter = false;
        aimController.InArrowFight_Exit = false;
        //标记一下 处于非战斗模式.
        aimController.InSwordFight = false;
        aimController.InArrowFight = false;
    }

    IEnumerator Delay2False() 
    {
        yield return new WaitForSeconds(0.1f);
        aimController.InSwordFight_Enter = false;
        aimController.InSwordFight_Exit = false;
        aimController.InArrowFight_Enter = false;
        aimController.InArrowFight_Exit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnSwordSwitch();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnArrowSwitch();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ResetSwitch();
        }
    }
}
