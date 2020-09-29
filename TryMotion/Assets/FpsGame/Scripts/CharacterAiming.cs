using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fpsGame
{
    public class CharacterAiming : MonoBehaviour
    {
        Camera mainCam;
        [SerializeField] float turnSpeed = 15;


        void Start()
        {
            mainCam = Camera.main;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void FixedUpdate()
        {
            //float yawCam = mainCam.transform.rotation.y;
            //transform.rotation = Quaternion.Slerp(transform.rotation , Quaternion.Euler(new Vector3(0 , yawCam , 0)),Time.fixedDeltaTime * turnSpeed);
        }
    }

}