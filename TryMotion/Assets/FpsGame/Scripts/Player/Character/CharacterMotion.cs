using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fpsGame
{

    public class CharacterMotion : MonoBehaviour
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


        //animation
        private CharacterLocomotion characterLocomotion;
        public CharacterLocomotion CharacterLocomotion
        {
            get
            {
                if (characterLocomotion == null)
                {
                    characterLocomotion = GetComponent<CharacterLocomotion>();
                }
                return characterLocomotion;
            }
        }

        #endregion


        [Header("角色属性")]
        public CharacterPropertyStruct characterProperty;


        [Header("地面检测")]
        public GroundStruct groundStruct;


        //private
        public Vector3 characterVelocity;
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


        void FixedUpdate()
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
            float maxDistance = groundStruct.isOnGround ? CC.skinWidth + groundStruct.groundCheckDistance : characterProperty.checkDistanceInAir;
            RaycastHit hit;
            groundStruct.groundNormal = Vector3.up;

            //这里,我们使用 
            Collider[] colliders = Physics.OverlapCapsule(GetCapsuleBottomCenter() , GetCapsuleTopCenter() , CC.radius * 0.9f , groundStruct.onGroundLayer);
            if (colliders.Length != 0)
            {
                CharacterLocomotion.IsOnGround = true;//动画
                groundStruct.isOnGround = true;
                
                ////Move自身不带有物理属性.
                //CC.Move(hit.distance * Vector3.down);
            }
            else
            {
                CharacterLocomotion.IsOnGround = false;//动画
                groundStruct.isOnGround = false;
                //不在地面.角色自动下坠.
                CC.Move(Vector3.down * Time.deltaTime * groundStruct.dropSpeed);
            }
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
            Vector3 dir = new Vector3(NewInputManager.Instance.Movement.x , 0 , NewInputManager.Instance.Movement.y);
            float sprintRatio = 0;
            //是否冲刺
            if (NewInputManager.Instance.IsSprint)
            {
                SetCrouchingState(false , false);
                sprintRatio = characterProperty.sprintMoveRatio;
                characterVelocity = dir * Time.deltaTime * characterProperty.moveSpeed * sprintRatio;
                characterVelocity = transform.TransformDirection(characterVelocity);
            }
            else
            {
                sprintRatio = characterProperty.normalMoveRatio;
                characterVelocity = dir * Time.deltaTime * characterProperty.moveSpeed * sprintRatio;
                characterVelocity = transform.TransformDirection(characterVelocity);
            }


            if (IsNormalUnderSlopeLimit())
            {
                PerformJump();

                CharacterLocomotion.Forward = Mathf.Clamp(dir.magnitude , 0 , 1);
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
            if (NewInputManager.Instance.IsJump)
            {
                isJump = true;
                jumpHeight = 0;
                jumpHeight = characterProperty.forceJumpPower;
                //Debug.LogError("跳跃高度: " + jumpHeight);
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
            if (NewInputManager.Instance.IsCrouch)
            {
                isCrouching = true;
                //角色控制器CC
                DOTween.To(() => CC.center , (Vector3 c) => CC.center = c , new Vector3(0 , characterProperty.characterCrouchHeight_Y / 2 , 0) , 0.5f);
                DOTween.To(() => CC.height , (float c) => CC.height = c , characterProperty.characterHeight_Y / 2 , 0.5f);
            }
            else
            {
                isCrouching = false;
                //角色控制器CC
                DOTween.To(() => CC.center , (Vector3 c) => CC.center = c , new Vector3(0 , characterProperty.characterCrouchHeight_Y , 0) , 0.5f);
                DOTween.To(() => CC.height , (float c) => CC.height = c , characterProperty.characterHeight_Y , 0.5f);
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
}
