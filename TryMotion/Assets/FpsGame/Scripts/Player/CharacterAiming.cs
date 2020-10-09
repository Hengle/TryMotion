using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace fpsGame
{
    public class CharacterAiming : MonoBehaviour
    {
        #region 属性

        private Rig RigLayer_AimPose;
        private Rig RigLayer_noAimPose;
        private Rig RigLayer_BodyAimPose;
        private Rig RigLayer_HandIK;

        #endregion

        Camera mainCam;
        //人物位移
        [SerializeField] float turnSpeed = 15;
        //武器参数
        //瞄准
        [SerializeField] float turnThreshold = 60;//枪旋转阈值
        float aimDuration = 0.3f;
        bool isAim = false;
        //射击
        float shotDuration = 0.33f;
        WeaponRacast weaponRacast;


        void Awake()
        {
            //Rigs
            Rig[] rigs = GetComponentsInChildren<Rig>(true);
            for (int i = 0; i < rigs.Length; i++)
            {
                if (rigs[i].name == "RigLayer_AimPose")
                {
                    RigLayer_AimPose = rigs[i];
                }
                else if (rigs[i].name == "RigLayer_noAimPose")
                {
                    RigLayer_noAimPose = rigs[i];
                }
                else if (rigs[i].name == "RigLayer_BodyAimPose")
                {
                    RigLayer_BodyAimPose = rigs[i];
                }
                else if (rigs[i].name == "RigLayer_HandIK")
                {
                    RigLayer_HandIK = rigs[i];
                }
            }

            weaponRacast = GetComponentInChildren<WeaponRacast>(true);


        }

        void Start()
        {
            mainCam = Camera.main;
            //锁定鼠标
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void FixedUpdate()
        {
            FixedUpdate_Rotate();
        }

        void Update()
        {
            Update_HandleWeapon();
        }

        //更新旋转
        void FixedUpdate_Rotate() 
        {
            float yawCam = mainCam.transform.localEulerAngles.y;
            float yawPlayer = transform.localEulerAngles.y;
            float diffValue = Mathf.Abs(yawCam - yawPlayer);
            if (diffValue >= turnThreshold)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation , Quaternion.Euler(new Vector3(0 , yawCam , 0)) , Time.fixedDeltaTime * turnSpeed);
            }
        }

        //处理武器
        void Update_HandleWeapon() 
        {
            //瞄准
            if (Input.GetMouseButton(1))
            {
                isAim = true;
                RigLayer_AimPose.weight += Time.deltaTime / aimDuration;
            }
            else
            {
                isAim = false;
                RigLayer_AimPose.weight -= Time.deltaTime / aimDuration;
            }
            //射击
            if (Input.GetButtonDown("Fire1") && isAim)
            {
                weaponRacast.StartFire();
            }
            if (Input.GetButtonUp("Fire1"))
            {
                weaponRacast.StopFire();
            }
            //持续射击
            if (weaponRacast.isFire)
            {
                weaponRacast.UpdateFire(Time.deltaTime);
            }

        }

    }

}