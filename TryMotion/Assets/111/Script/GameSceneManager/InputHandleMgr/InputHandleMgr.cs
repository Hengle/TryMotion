using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 输入系统管理
/// </summary>
public class InputHandleMgr : MonoBehaviour
{
    #region 单例

    private static InputHandleMgr _instance;
    public static InputHandleMgr Instsance 
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InputHandleMgr>();
            }
            return _instance;
        }
    }

    #endregion


    [Tooltip("Sensitivity multiplier for moving the camera around")]
    public float lookSensitivity = 1f;
    [Tooltip("Additional sensitivity multiplier for WebGL")]
    public float webglLookSensitivityMultiplier = 0.25f;
    [Tooltip("Limit to consider an input when using a trigger on a controller")]
    public float triggerAxisThreshold = 0.4f;
    [Tooltip("Used to flip the vertical input axis")]
    public bool invertYAxis = false;
    [Tooltip("Used to flip the horizontal input axis")]
    public bool invertXAxis = false;

    //GameFlowManager m_GameFlowManager;
    //PlayerCharacterController m_PlayerCharacterController;
    bool m_FireInputWasHeld;

    private void Start()
    {
        //m_PlayerCharacterController = GetComponent<PlayerCharacterController>();
        //m_GameFlowManager = FindObjectOfType<GameFlowManager>();

        //// 鼠标锁定住
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

    }

    private void LateUpdate()
    {
        m_FireInputWasHeld = GetFireInputHeld();
    }

    /// <summary>
    /// 是否-处于可以控制
    /// </summary>
    public bool CanProcessInput()
    {
        //return Cursor.lockState == CursorLockMode.Locked && !m_GameFlowManager.gameIsEnding;
        return true;
    }

    /// <summary>
    /// 得到-移动X/Y轴
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMoveInput()
    {
        if (CanProcessInput())
        {
            //整数 Vector3
            Vector3 move = new Vector3(Input.GetAxisRaw(GameConstants.k_AxisNameHorizontal) , 0f , Input.GetAxisRaw(GameConstants.k_AxisNameVertical));

            // 将移动输入的最大幅度限制为1，否则对角移动可能超过定义的最大移动速度
            move = Vector3.ClampMagnitude(move , 1);

            return move;
        }

        return Vector3.zero;
    }

    /// <summary>
    /// GetLook 水平输入
    /// </summary>
    /// <returns></returns>
    public float GetLookInputsHorizontal()
    {
        return GetMouseOrStickLookAxis(GameConstants.k_MouseAxisNameHorizontal , GameConstants.k_AxisNameJoystickLookHorizontal);
    }

    /// <summary>
    /// GetLook 垂直输入
    /// </summary>
    /// <returns></returns>
    public float GetLookInputsVertical()
    {
        return GetMouseOrStickLookAxis(GameConstants.k_MouseAxisNameVertical , GameConstants.k_AxisNameJoystickLookVertical);
    }

    /// <summary>
    /// 跳跃-按下
    /// </summary>
    /// <returns></returns>
    public bool GetJumpInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown(GameConstants.k_ButtonNameJump);
        }

        return false;
    }

    /// <summary>
    /// 跳跃-按住
    /// </summary>
    /// <returns></returns>
    public bool GetJumpInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetButton(GameConstants.k_ButtonNameJump);
        }

        return false;
    }

    /// <summary>
    /// 开火-按下
    /// </summary>
    public bool GetFireInputDown()
    {
        return GetFireInputHeld() && !m_FireInputWasHeld;
    }

    /// <summary>
    /// 开火-松开
    /// </summary>
    public bool GetFireInputReleased()
    {
        return !GetFireInputHeld() && m_FireInputWasHeld;
    }

    /// <summary>
    /// 开火-按住
    /// </summary>
    public bool GetFireInputHeld()
    {
        if (CanProcessInput())
        {
            bool isGamepad = Input.GetAxis(GameConstants.k_ButtonNameGamepadFire) != 0f;
            if (isGamepad)
            {
                return Input.GetAxis(GameConstants.k_ButtonNameGamepadFire) >= triggerAxisThreshold;
            }
            else
            {
                return Input.GetButton(GameConstants.k_ButtonNameFire);
            }
        }

        return false;
    }

    /// <summary>
    /// 瞄准-按住 (鼠标右键)
    /// </summary>
    /// <returns></returns>
    public bool GetAimInputHeld()
    {
        if (CanProcessInput())
        {
            bool isGamepad = Input.GetAxis(GameConstants.k_ButtonNameGamepadAim) != 0f;
            bool i = isGamepad ? (Input.GetAxis(GameConstants.k_ButtonNameGamepadAim) > 0f) : Input.GetButton(GameConstants.k_ButtonNameAnim);
            return i;
        }

        return false;
    }

    /// <summary>
    /// 冲刺-按住
    /// </summary>
    /// <returns></returns>
    public bool GetSprintInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetButton(GameConstants.k_ButtonNameSprint);
        }

        return false;
    }

    /// <summary>
    /// 匍匐-按下
    /// </summary>
    /// <returns></returns>
    public bool GetCrouchInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown(GameConstants.k_ButtonNameCrouch);
        }

        return false;
    }

    /// <summary>
    /// 匍匐-松开
    /// </summary>
    /// <returns></returns>
    public bool GetCrouchInputReleased()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonUp(GameConstants.k_ButtonNameCrouch);
        }

        return false;
    }

    /// <summary>
    /// 切换-武器 (鼠标滚轮/手柄按钮)
    /// </summary>
    /// <returns></returns>
    public int GetSwitchWeaponInput()
    {
        if (CanProcessInput())
        {

            bool isGamepad = Input.GetAxis(GameConstants.k_ButtonNameGamepadSwitchWeapon) != 0f;
            string axisName = isGamepad ? GameConstants.k_ButtonNameGamepadSwitchWeapon : GameConstants.k_ButtonNameSwitchWeapon;

            if (Input.GetAxis(axisName) > 0f)
                return -1;
            else if (Input.GetAxis(axisName) < 0f)
                return 1;
            else if (Input.GetAxis(GameConstants.k_ButtonNameNextWeapon) > 0f)
                return -1;
            else if (Input.GetAxis(GameConstants.k_ButtonNameNextWeapon) < 0f)
                return 1;
        }

        return 0;
    }

    /// <summary>
    /// 切换-武器 (键盘 1,2,3,4,5,6)
    /// </summary>
    /// <returns></returns>
    public int GetSelectWeaponInput()
    {
        if (CanProcessInput())
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                return 1;
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                return 2;
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                return 3;
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                return 4;
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                return 5;
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                return 6;
            else
                return 0;
        }

        return 0;
    }

    /// <summary>
    /// 限制 视野轴
    /// <para>鼠标:</para>
    /// <para>Mouse X /Mouse Y</para>
    ///  <para>手柄:</para>
    /// <para>Look X /Look Y</para>
    /// </summary>
    /// <param name="mouseInputName"></param>
    /// <param name="stickInputName"></param>
    /// <returns></returns>
    float GetMouseOrStickLookAxis(string mouseInputName , string stickInputName)
    {
        if (CanProcessInput())
        {
            // 检查这个look输入是否来自鼠标
            bool isGamepad = Input.GetAxis(stickInputName) != 0f;
            float i = isGamepad ? Input.GetAxis(stickInputName) : Input.GetAxisRaw(mouseInputName);

            // 手柄反转垂直输入
            if (invertYAxis)
                i *= -1f;

            // 应用灵敏度乘数
            i *= lookSensitivity;

            if (isGamepad)
            {
                // since mouse input is already deltaTime-dependant, only scale input with frame time if it's coming from sticks
                i *= Time.deltaTime;
            }
            else
            {
                // 减少鼠标输入量，使其与鼠标移动相等
                i *= 0.01f;
#if UNITY_WEBGL
                // Mouse tends to be even more sensitive in WebGL due to mouse acceleration, so reduce it even more
                i *= webglLookSensitivityMultiplier;
#endif
            }

            return i;
        }

        return 0f;
    }
}


/// <summary>
/// 控制输入名称
/// </summary>
public class GameConstants
{
    //PC端
    public const string k_AxisNameVertical = "Vertical";
    public const string k_AxisNameHorizontal = "Horizontal";
    public const string k_MouseAxisNameHorizontal = "Mouse X";
    public const string k_MouseAxisNameVertical = "Mouse Y";
    public const string k_ButtonNameFire = "Fire";
    public const string k_ButtonNameAnim = "Anim";
    public const string k_ButtonNameJump = "Jump";
    public const string k_ButtonNameSprint = "Sprint";
    public const string k_ButtonNameCrouch = "Crouch"; 
    public const string k_ButtonNameNextWeapon = "NextWeapon";
    public const string k_ButtonNameSwitchWeapon = "Mouse ScrollWheel";
    //手柄
    public const string k_AxisNameJoystickMoveHorizontal = "Move X";
    public const string k_AxisNameJoystickMoveVertical = "Move Y";
    public const string k_AxisNameJoystickLookHorizontal = "Look X";
    public const string k_AxisNameJoystickLookVertical = "Look Y";
    public const string k_ButtonNameGamepadFire = "Gamepad Fire";
    public const string k_ButtonNameGamepadAim = "Gamepad Aim";
    public const string k_ButtonNameGamepadSprint = "Gamepad Sprint";
    public const string k_ButtonNameGamePadCrouch = "Gamepad Crouch";
    public const string k_ButtonNameGamepadSwitchWeapon = "Gamepad Switch";
    //共用
    public const string k_ButtonNamePauseMenu = "Pause Menu";
    public const string k_ButtonNameSubmit = "Submit";
    public const string k_ButtonNameCancel = "Cancel";
}