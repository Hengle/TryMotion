using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThirdPersonalSimpleMove : MonoBehaviour
{
    private CharacterController cc;

    private Vector3 velocity = Vector3.zero;

    float speed = 6;
    [SerializeField] float h = 0;
    [SerializeField] float v = 0;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }


    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");


        if (Mathf.Abs(h) >= 0.01f || Mathf.Abs(v) >= 0.01f)
        {
            velocity.x = h * speed * Time.deltaTime;
            velocity.y = 0;
            velocity.z = v * speed * Time.deltaTime;
            velocity = transform.TransformDirection(velocity);

            cc.Move(velocity);
        }
    }
}
