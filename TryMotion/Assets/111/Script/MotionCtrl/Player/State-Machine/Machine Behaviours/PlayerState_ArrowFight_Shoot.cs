/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-27 14:35:04
*      修改记录：
*      版 本：1.0
 ========================================================*/
using L_Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerState_ArrowFight_Shoot" , menuName = "FSM/Player/PlayerState_ArrowFight_Shoot" , order = -200)]
public class PlayerState_ArrowFight_Shoot : PlayerStateInfo
{
    //private
    private PlayerAnimationController _playerAnimCtrl;
    private PlayerHangingObj _playerHangingObj;

    private GameObject arrowGo;
    private Arrow_Bullet arrow;
    public Transform shot_start;
    public Transform shot_end;
    private Vector3 shotDir;

    public override void OnStateEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        if (_playerAnimCtrl == null) { _playerAnimCtrl = player.GetComponent<PlayerAnimationController>(); }
        if (_playerHangingObj == null) { _playerHangingObj = player.GetComponent<PlayerHangingObj>(); }
        arrowGo = AllPoolMgr.Instance.missilesPool.FindCorrespondingPoolByPrefName("Arrow").Recycle();

        shot_start = _playerHangingObj.hangingContainer[1].transform;
        shot_end = _playerHangingObj.hangingContainer[0].transform;
        shotDir = (shot_end.position - shot_start.position).normalized;

        arrowGo.transform.SetParent(_playerHangingObj.hangingContainer[0]);
        arrowGo.transform.localPosition = Vector3.zero;
        arrowGo.transform.localRotation = Quaternion.LookRotation(shotDir,Vector3.up);
        arrow = arrowGo.GetComponent<Arrow_Bullet>();
    }

    public override void OnStateUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {
        if(!InputManager.Instance.IsAim)
        {
            _playerAnimCtrl.ArrowAttack = 3;
            arrowGo.SetActive(false);
        }
        else
        {
            if (stateInfo.normalizedTime >= 0.2f)
            {
                arrow.OpenMotion();
            }
        }
    }

    public override void OnStateExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {

    }

    public override void OnStateIK(Animator animator , AnimatorStateInfo stateInfo , int layerIndex)
    {

    }
}
