using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollow_OffMeshLink : MonoBehaviour
{

    public Transform target;
    private Vector3 offset;


    void Start()
    {
        offset = transform.position - target.position;
    }

    
    void Update()
    {
        transform.position = offset + target.position;
    }
}
