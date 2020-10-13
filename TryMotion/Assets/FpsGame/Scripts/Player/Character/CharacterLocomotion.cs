using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace fpsGame
{
    public class CharacterLocomotion : MonoBehaviour
    {
        #region 属性

        private RigBuilder rigBuilder;
        public RigBuilder RigBuilder 
        {
            get 
            {
                if (rigBuilder == null)
                {
                    rigBuilder = GetComponent<RigBuilder>();
                }
                return rigBuilder;
            }
        }

        #endregion

        private Animator _anim;
        //具体数值
        public float Forward { get; set; }
        public float Turn { get; set; }
        public float Jump { get; set; }
        public float IsJumpLeg { get; set; }
        public bool IsOnGround { get; set; }
        public bool IsCrouch { set; get; }

        //animator-hash
        //common
        private int forward = Animator.StringToHash("Forward");
        private int turn = Animator.StringToHash("Turn");
        private int jump = Animator.StringToHash("Jump");
        private int isOnGround = Animator.StringToHash("OnGround");
        private int isCrouch = Animator.StringToHash("Crouch");
        private int isJumpLeg = Animator.StringToHash("JumpLeg");


        private void Awake()
        {
            _anim = GetComponent<Animator>();

            RigBuilder.Build();
        }

        void Start()
        {

        }


        void Update()
        {
            _anim.SetFloat(forward , Forward , 0.3f , 0.1f);
            _anim.SetFloat(turn , Turn , 0.5f , 0.02f);
            _anim.SetFloat(jump , Jump , 0.5f , 0.02f);
            _anim.SetFloat(isJumpLeg , IsJumpLeg , 0.5f , 0.02f);
            //_anim.SetBool(death , Earth);
            _anim.SetBool(isOnGround , IsOnGround);
            _anim.SetBool(isCrouch , IsCrouch);
        }
    }
}
