/* ========================================================
*      作 者：Lin
*      主 题：角色移动输入控制器
*      主要功能：

*      详细描述：

*      创建时间：2020-09-24 09:40:16
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using XInputDotNetPure;

public class PlayerMovementInputController : MonoBehaviour
{

    [System.Serializable]
    /// <summary>
    /// 角色动作参数
    /// </summary>
    public struct CharacterMotionParameter
    {

        [Header("移动")]
        public Vector3 moveDir;             //移动方向
        public float moveSpeed;             //移动速度
        public float sprintMoveRatio;       //加速参数

        [Header("视野旋转")]
        public float rotateAngle;          //角速度值

        [Header("跳跃")]
        public float jumpSpeed;
        public float G;                     //重力值 =-9.81f
        public float defaultJumpCenterY;    //跳跃默认-collider.center.y
        public float defaultJumpHeight;     //跳跃默认-collider.Height
        public float jumpCenterY;          //跳跃collider.center.y
        public float jumpHeight;            //跳跃collider.Height


        [Header("地面检测")]
        public LayerMask onGroundLayer;
        public Vector3 groundNormal;        //地面法线
        public float groundCheckDistance;   //地面检测(_cc.SkinnedRadius)
        public float checkDistanceInAir;    //空中检测
        public bool isOnGround;             //是否还在空中

        [Header("角色其他参数")]
        public float characterHeight_Y;         //Character高度参数
        public float characterCrouchHeight_Y;   //Character(匍匐)高度参数


        public void Init()
        {
            //移动
            moveDir = Vector3.zero;
            moveSpeed = 5;
            //视野
            rotateAngle = 120;
            //跳跃
            jumpSpeed = 8;
            G = -20f;//-9.81f
            defaultJumpCenterY = 1;
            defaultJumpHeight = 2;
            jumpCenterY = 1.2f;
            jumpHeight = 1.5f;
            //地面检测
            onGroundLayer = LayerMask.GetMask("Ground");
            groundNormal = Vector3.zero;
            groundCheckDistance = 0.03f;
            checkDistanceInAir = 0.2f;
            isOnGround = false;
            //角色其他参数
            characterCrouchHeight_Y = 2;
            characterCrouchHeight_Y = 1;
        }
    }

    //组件、脚本
    private PlayerAnimationController _playerAnimCtrl;
    private CharacterController _cc;
    private InputManager _inputManager;
    private Transform _followTarget;
    private Transform _entity;

    [Header("角色动作参数")]
    public CharacterMotionParameter parameter;

    void Awake()
    {
        Time.timeScale = 1;
        _playerAnimCtrl = GetComponent<PlayerAnimationController>();
        _cc = GetComponent<CharacterController>();
        _inputManager = InputManager.Instance;
        _followTarget = transform.Find("followTarget");
        _entity = transform.Find("entity");

        parameter.Init();
    }

    void Start()
    {
        //transform.position = new Vector3(0 , 3 , 0);
    }


    void Update()
    {
        if (CheckIsGround())
        {
            parameter.moveDir = new Vector3(_inputManager.Movement.x , 0 , _inputManager.Movement.y);
            parameter.moveDir = transform.TransformDirection(parameter.moveDir);
            parameter.moveDir *= parameter.moveSpeed;

            //播放跳跃动画
            if (_inputManager.IsJump)
            {
                _playerAnimCtrl.Jump = true;
                //moveDir.y = jumpSpeed; //修改为,由动画 JumpStart 参数控制起跳瞬间.
            }

            if (Mathf.Abs(_inputManager.Movement.x) >= 0.01f || Mathf.Abs(_inputManager.Movement.y) >= 0.01f)
            {
                Vector3 moveDir = parameter.moveDir.normalized;
                _playerAnimCtrl.Forward = Mathf.Round(Mathf.Max(Mathf.Abs(moveDir.x) , Mathf.Abs(moveDir.z))) ;
            }
            else
            {
                _playerAnimCtrl.Forward = 0;
            }

            Aim();
        }
    }

    private void LateUpdate()
    {
        //应用重力
        parameter.moveDir.y += parameter.G * Time.deltaTime;

        parameter.moveDir = _followTarget.TransformDirection(parameter.moveDir);

        //控制位移
        _cc.Move(parameter.moveDir * Time.deltaTime);

        //视野旋转
        RotateView();

        Vector3 lookDir = new Vector3(parameter.moveDir.x , 0 , parameter.moveDir.z);
        if (Mathf.Abs(lookDir.x) >= 0.1f || Mathf.Abs(lookDir.z) >= 0.1f)
        {
            _entity.rotation = Quaternion.Slerp(_entity.rotation , Quaternion.LookRotation(lookDir , Vector3.up) , Time.deltaTime * 5);
        }

    }

    #region 地面检测


    /// <summary>
    /// 检测是否在地面
    /// </summary>
    public bool CheckIsGround()
    {
        float maxDistance = parameter.isOnGround ? _cc.skinWidth + parameter.groundCheckDistance : parameter.checkDistanceInAir;
        RaycastHit hit;
        parameter.groundNormal = Vector3.up;

        //这里,我们使用 
        bool isCastGround = Physics.CapsuleCast(GetCapsuleBottomCenter() , GetCapsuleTopCenter() , _cc.radius , Vector3.down , out hit , maxDistance , parameter.onGroundLayer , QueryTriggerInteraction.Ignore);
        if (isCastGround)
        {
            parameter.groundNormal = hit.normal;

            ////Move自身不带有物理属性.
            //_cc.Move(hit.distance * Vector3.down);
        }

        _playerAnimCtrl.IsOnGround = isCastGround;

        parameter.isOnGround = isCastGround;

        return parameter.isOnGround;
    }
    /// <summary>
    /// 获得胶囊体底部中心点
    /// </summary>
    Vector3 GetCapsuleBottomCenter()
    {
        return transform.position + transform.up * _cc.radius;
    }
    /// <summary>
    /// 获得胶囊体顶部中心点
    /// </summary>
    Vector3 GetCapsuleTopCenter()
    {
        return transform.position + transform.up * (_cc.height - _cc.radius);
    }



    #endregion

    #region Animator 执行

    public void Jump()
    {
        parameter.moveDir.y = parameter.jumpSpeed;
    }

    public void Aim()
    {
        ////处于弓箭
        //if (_playerAnimCtrl.InArrowFight)
        //{
        //    if (_inputManager.IsAim)
        //    {
        //        _playerAnimCtrl.ArrowAttack = 1;
        //    }
        //    else
        //    {
        //        _playerAnimCtrl.ArrowAttack = 0;
        //    }
        //}

        //处于刀剑


    }

    #endregion

    #region 旋转视角

    float delta_x = 0;
    float delta_y = 0;
    [SerializeField] float look_x = 0;
    float look_y = 0;
    float screenThreshold = 5f;//屏幕阈值
    [SerializeField] Vector2 screenCenter = new Vector2(Screen.width / 2 , Screen.height / 2);
    void RotateView()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        look_x = x * parameter.rotateAngle * Time.deltaTime;
        look_y = y * parameter.rotateAngle * Time.deltaTime;

        //_followTarget.localEulerAngles = new Vector3(-look_y , look_x , 0);

        //水平旋转
        _followTarget.transform.rotation *= Quaternion.AngleAxis(look_x , Vector3.up);

        //垂直旋转
        _followTarget.rotation *= Quaternion.AngleAxis(-look_y , Vector3.right);

        var angles = _followTarget.transform.localEulerAngles;
        angles.z = 0;
        var angle = _followTarget.transform.localEulerAngles.x;
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        _followTarget.transform.localEulerAngles = angles;


        //transform.rotation = Quaternion.Euler(0 , _followTarget.transform.rotation.eulerAngles.y , 0);

        //_followTarget.transform.localEulerAngles = new Vector3(angles.x , 0 , 0);
    }

    #endregion
}
