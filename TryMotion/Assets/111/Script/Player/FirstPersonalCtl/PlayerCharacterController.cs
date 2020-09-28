using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



[System.Serializable]
/// <summary>
/// 地面检测Struct
/// </summary>
public struct GroundStruct
{
    public LayerMask onGroundLayer;
    public float groundCheckDistance; //地面监测(CC.SkinnedRadius)
    public bool isOnGround;         //是否在地面
    public float dropSpeed;            //下落速度
    public Vector3 groundNormal;            //与地面发生接触的法线
    [Header("下坠")]
    public float dropMinHight;              //坠落最小高度
    public float dropMaxHight;              //坠落最大高度
    [Header("牛顿")]
    public float G;

    public void Init()
    {
        groundCheckDistance = 0.05f;
        isOnGround = false;
        dropSpeed = 10;
        G = -9.81f;
    }
}

[System.Serializable]
/// <summary>
/// 角色属性Struct
/// </summary>
public struct CharacterPropertyStruct
{
    [Header("角色属性")]
    public float characterHeight_Y;
    public float characterCrouchHeight_Y;

    [Header("视野")]
    public float rotationSpeed;
    public float RotationMultiplier;
    public float verticalMaxLimit;
    public float verticalMinLimit;

    [Header("移动")]
    public float moveSpeed;

    [Header("冲刺")]
    public float sprintMoveRatio;

    [Header("跳跃")]
    public float forceJumpPower;
    public float checkDistanceInAir;
    public AnimationCurve jumpCurve;//跳跃曲线


    public void Init()
    {
        characterHeight_Y = 2;
        characterCrouchHeight_Y = 1;
        rotationSpeed = 120;
        RotationMultiplier = 1;
        verticalMaxLimit = 89;
        verticalMinLimit = -89;
        moveSpeed = 3;
        sprintMoveRatio = 1.5f;
        forceJumpPower = 10;
        checkDistanceInAir = 0.07f;
    }
}

/// <summary>
/// 玩家角色控制器
/// </summary>
public class PlayerCharacterController : MonoBehaviour
{

    #region 属性

    private CharacterController cc;
    /// <summary>
    /// 角色控制器
    /// </summary>
    public CharacterController CC
    {
        get
        {
            if (cc == null)
            {
                cc = GetComponentInChildren<CharacterController>();
            }
            return cc;
        }
    }

    private Transform weaponSocketContainer;
    /// <summary>
    /// 武器位置点,容器
    /// </summary>
    public Transform WeaponSocketContainer
    {
        get
        {
            if (weaponSocketContainer == null)
            {
                weaponSocketContainer = transform.Find("WeaponSocketContainer");
            }
            return weaponSocketContainer;
        }
    }

    private Camera mainCamera;
    /// <summary>
    /// 主摄像机
    /// </summary>
    public Camera MainCamera
    {
        get
        {
            if (mainCamera == null)
            {
                mainCamera = transform.Find("CameraContainer/MainCamera").GetComponent<Camera>();
            }
            return mainCamera;
        }
    }

    private Camera firstPersonalWeaponCamera;
    /// <summary>
    /// 第一人称武器摄像机
    /// </summary>
    public Camera FirstPersonalWeaponCamera
    {
        get
        {
            if (firstPersonalWeaponCamera == null)
            {
                firstPersonalWeaponCamera = transform.Find("CameraContainer/MainCamera/FirstPersonalWeaponCamera").GetComponent<Camera>();
            }
            return firstPersonalWeaponCamera;
        }
    }


    //地面检测点(Sphere)
    private Transform checkGroundPoint;
    public Transform CheckGroundPoint
    {
        get
        {
            if (checkGroundPoint == null)
            {
                checkGroundPoint = transform.Find("CheckGroundPoint");
            }
            return checkGroundPoint;
        }
    }

    #endregion


    [Header("角色属性")]
    public CharacterPropertyStruct characterProperty;


    [Header("地面检测")]
    public GroundStruct groundStruct;



    //private
    private Vector3 characterVelocity;
    private float cameraVerticalAngle = 0;
    //跳跃
    float jumpHeight = 0;
    float jumpTimer = 0;
    bool isJump = false;
    //角色蹲伏
    float m_TargetCharacterHeight;
    float m_CharacterStadingHeight;//角色站立高度
    float m_CharacterCrouchHeight;//角色蹲伏高度
    bool isCrouching = false;       //是否下蹲
    //冲刺
    bool isSprinting = false;   //是否冲刺

    void Awake()
    {
        groundStruct.Init();
        characterProperty.Init();
    }

    void Start()
    {

    }


    void Update()
    {
        if (CheckIsGround())
        {
            PerformPlayerSquat();
        }

        PerformPlayerMove();
    }

    #region 地面检测

    /// <summary>
    /// 检测是否在地面
    /// </summary>
    bool CheckIsGround()
    {
        #region 方案1

        float maxDistance = groundStruct.isOnGround ? CC.skinWidth + groundStruct.groundCheckDistance : characterProperty.checkDistanceInAir;
        RaycastHit hit;
        groundStruct.groundNormal = Vector3.up;

        //这里,我们使用 
        bool isCastGround = Physics.CapsuleCast(GetCapsuleBottomCenter() , GetCapsuleTopCenter() , CC.radius , Vector3.down , out hit , maxDistance , groundStruct.onGroundLayer , QueryTriggerInteraction.Ignore);
        if (isCastGround)
        {
            groundStruct.isOnGround = true;
            groundStruct.groundNormal = hit.normal;

            //Move自身不带有物理属性.
            CC.Move(hit.distance * Vector3.down);
            //jumpHeight = 0;
        }
        else
        {
            groundStruct.isOnGround = false;

            //不在地面.角色自动下坠.
            CC.Move(Vector3.down * Time.deltaTime * groundStruct.dropSpeed);
        }

        #endregion

        #region 方案2

        //bool isOnGround = Physics.CheckSphere(CheckGroundPoint.position , 0.5f , onGroundLayer);

        #endregion

        return groundStruct.isOnGround;
    }
    /// <summary>
    /// 获得胶囊体底部中心点
    /// </summary>
    Vector3 GetCapsuleBottomCenter()
    {
        return transform.position + transform.up * CC.radius;
    }
    /// <summary>
    /// 获得胶囊体顶部中心点
    /// </summary>
    Vector3 GetCapsuleTopCenter()
    {
        return transform.position + transform.up * (CC.height - CC.radius);
    }



    #endregion

    #region 角色移动

    /// <summary>
    /// 执行角色移动
    /// </summary>
    void PerformPlayerMove()
    {
        //控制人物视野限制
        float view_X = InputHandleMgr.Instsance.GetLookInputsHorizontal() * characterProperty.rotationSpeed * characterProperty.RotationMultiplier;
        float view_Y = InputHandleMgr.Instsance.GetLookInputsVertical() * characterProperty.rotationSpeed * characterProperty.RotationMultiplier;

        if (Mathf.Abs(view_Y) >= 0.01f || Mathf.Abs(view_X) >= 0.01f)
        {
            //水平旋转
            transform.Rotate(0 , view_X , 0);
            //上下视野限制
            cameraVerticalAngle -= view_Y;
            cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle , characterProperty.verticalMinLimit , characterProperty.verticalMaxLimit);
            MainCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle , 0 , 0);
        }

        //是否冲刺
        if (InputHandleMgr.Instsance.GetSprintInputHeld())
        {
            SetCrouchingState(false , false);
            float sprintRatio = characterProperty.sprintMoveRatio;
            characterVelocity = InputManager.Instance.Movement * Time.deltaTime * characterProperty.moveSpeed * sprintRatio;
            characterVelocity = transform.TransformDirection(characterVelocity);
        }
        else
        {
            float sprintRatio = characterProperty.sprintMoveRatio;
            characterVelocity = InputManager.Instance.Movement * Time.deltaTime * characterProperty.moveSpeed * sprintRatio;
            characterVelocity = transform.TransformDirection(characterVelocity);
        }


        if (IsNormalUnderSlopeLimit())
        {

            PerformJump();
            //CC 移动
            CC.Move(characterVelocity);
        }
    }
    /// <summary>
    ///  如果给定法线表示的斜角低于角色控制器的斜角限制，则返回true
    /// </summary>
    bool IsNormalUnderSlopeLimit()
    {
        //是否 地面法线 和 transform.up 朝向一致.
        bool isPositive = Vector3.Dot(transform.up , groundStruct.groundNormal) > 0;
        // 用到初中知识.
        return Vector3.Angle(transform.up , groundStruct.groundNormal) <= CC.slopeLimit && isPositive;
    }

    #endregion

    #region 跳跃

    void PerformJump()
    {
        //跳跃
        if (InputHandleMgr.Instsance.GetJumpInputDown())
        {
            isJump = true;
            jumpHeight = 0;
            jumpHeight = characterProperty.forceJumpPower;
            Debug.LogError("跳跃高度: " + jumpHeight);
        }
        if (isJump)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= 1)
            {
                jumpTimer = 0;
                isJump = false;
            }
        }
        characterVelocity.y = Mathf.Lerp(characterVelocity.y , jumpHeight , characterProperty.jumpCurve.Evaluate(jumpTimer));
        //Debug.LogError(characterVelocity.y);
    }

    #endregion

    #region 下蹲

    /// <summary>
    /// 执行角色下蹲
    /// </summary>
    void PerformPlayerSquat()
    {
        if (InputHandleMgr.Instsance.GetCrouchInputDown())
        {
            isCrouching = true;
            //角色控制器CC
            DOTween.To(() => CC.center , (Vector3 c) => CC.center = c , new Vector3(0 , characterProperty.characterCrouchHeight_Y / 2 , 0) , 0.5f);
            DOTween.To(() => CC.height , (float c) => CC.height = c , characterProperty.characterHeight_Y / 2 , 0.5f);
            //武器位置点,容器
            WeaponSocketContainer.DOLocalMoveY(-0.5f , 0.5f);
            //摄像机
            MainCamera.transform.DOLocalMoveY(-0.5f , 0.5f);
        }
        if (InputHandleMgr.Instsance.GetCrouchInputReleased())
        {
            isCrouching = false;
            //角色控制器CC
            DOTween.To(() => CC.center , (Vector3 c) => CC.center = c , new Vector3(0 , characterProperty.characterCrouchHeight_Y , 0) , 0.5f);
            DOTween.To(() => CC.height , (float c) => CC.height = c , characterProperty.characterHeight_Y , 0.5f);
            //武器位置点,容器
            WeaponSocketContainer.DOLocalMoveY(0 , 0.5f);
            //摄像机
            MainCamera.transform.DOLocalMoveY(0 , 0.5f);
        }
    }

    /// <summary>
    /// 设置下蹲状态
    /// </summary>
    /// <param name="isCrouch">是否下蹲</param>
    /// <param name="isIgnoreCollider">是否忽略碰撞</param>
    bool SetCrouchingState(bool isCrouch , bool isIgnoreCollider)
    {
        //是否下蹲
        if (isCrouch)
        {
            m_TargetCharacterHeight = m_CharacterCrouchHeight;
        }

        if (!isIgnoreCollider)
        {
            Collider[] collectColliders = Physics.OverlapCapsule(GetCapsuleBottomCenter() , GetCapsuleTopCenter() , CC.radius , -1);
            for (int i = 0; i < collectColliders.Length; i++)
            {
                if (collectColliders[i] != CC)
                {
                    return false; //说明,有障碍物在头顶.
                }
            }

            m_TargetCharacterHeight = m_CharacterStadingHeight;
        }

        isCrouching = isCrouch;

        return true;
    }

    #endregion

}
