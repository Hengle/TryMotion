﻿using NewInputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInputManager : MonoBehaviour
{
    #region 单例

    private static NewInputManager _instance;
    public static NewInputManager Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<NewInputManager>();
            }
            return _instance;
        }
    }

    #endregion

    /// <summary>
    /// 移动端Vector2值
    /// </summary>
    public Vector2 Movement;
    /// <summary>
    /// 是否跳跃按钮
    /// </summary>
    public bool IsJump;
    /// <summary>
    /// 是否瞄准
    /// </summary>
    public bool IsAim;

    /// <summary>
    /// 是否下蹲
    /// </summary>
    public bool IsCrouch;

    /// <summary>
    /// 是否射击
    /// </summary>
    public bool IsShot;

    /// <summary>
    /// 是否冲刺
    /// </summary>
    public bool IsSprint;


    public Vector2 LookPos;

    public Vector2 MouseDrag;


    //新版 输入管理系统
    private XuShuSpaceInputManager xuShuSpaceInputManager;

    protected virtual void Awake()
    {
        xuShuSpaceInputManager = new NewInputSystem.XuShuSpaceInputManager();

        //移动
        xuShuSpaceInputManager.PC_Ctrl.Movement.performed += context => Movement = context.ReadValue<Vector2>();
        xuShuSpaceInputManager.PC_Ctrl.Movement.canceled += context => Movement = Vector2.zero;

        //跳跃
        xuShuSpaceInputManager.PC_Ctrl.Jump.performed += context => IsJump = context.ReadValue<float>() > 0.5f;
        xuShuSpaceInputManager.PC_Ctrl.Jump.canceled += context => IsJump = false;

        //瞄准
        xuShuSpaceInputManager.PC_Ctrl.Aim.performed += context => IsAim = context.ReadValue<float>() > 0.5f;
        xuShuSpaceInputManager.PC_Ctrl.Aim.canceled += context => IsAim = false;

        //射击
        xuShuSpaceInputManager.PC_Ctrl.Shoot.performed += context => IsShot = context.ReadValue<float>() > 0.5f;
        xuShuSpaceInputManager.PC_Ctrl.Shoot.canceled += context => IsShot = false;

        //视野
        xuShuSpaceInputManager.PC_Ctrl.Look.performed += context => LookPos = context.ReadValue<Vector2>();
        xuShuSpaceInputManager.PC_Ctrl.Look.performed += context => LookPos = Vector2.zero;

        //xuShuSpaceInputManager.touchCtrl.MouseDrag.performed += context => MouseDrag = context.ReadValue<Vector2>();
    }


    protected virtual void OnEnable()
    {
        xuShuSpaceInputManager?.Enable();
    }

    protected virtual void OnDisable()
    {
        xuShuSpaceInputManager?.Disable();
    }


}