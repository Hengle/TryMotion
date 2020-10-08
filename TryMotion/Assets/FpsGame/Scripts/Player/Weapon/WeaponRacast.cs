using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRacast : MonoBehaviour
{

    //private
    ParticleSystem muzzleEfx;
    ParticleSystem hitEfx;
    Transform raycastOrigin;
    Transform aimLookAt;
    Transform crossHairTarget;

    [Header("特效")]
    [SerializeField] TrailRenderer trailRenderer;

    [Header("射击")]
    int fireRate = 25;

    void Awake()
    {
        //Transform
        Transform[] trans = GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < trans.Length; i++)
        {
            if (trans[i].name == "raycastOrigin")
            {
                raycastOrigin = trans[i];
            }
            else if (trans[i].name == "MuzzleFlashEffect")
            {
                muzzleEfx = trans[i].GetComponent<ParticleSystem>();
            }
            else if (trans[i].name == "BulletImpactMetalEffect")
            {
                hitEfx = trans[i].GetComponent<ParticleSystem>();
            }
        }

        //else
        aimLookAt = Camera.main.transform.Find("AimLookAt");
        crossHairTarget = Camera.main.transform.Find("CrossHairTarget");
    }


    void Update()
    {

    }

    public void FirstFire() 
    {
    
    }

    public void UpdateFire(float DeltaTime) 
    {
    
    }

    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;//射击累计时间
    float fireDuration = 1;

    /// <summary>
    /// 射击
    /// </summary>
    public void MuzzleShotting()
    {
        muzzleEfx.Emit(1);//枪焰

        ray.origin = raycastOrigin.position;
        ray.direction = aimLookAt.position - ray.origin;
        var tracer = Instantiate(trailRenderer , ray.origin , Quaternion.identity);
        tracer.AddPosition(ray.origin);

        if (Physics.Raycast(ray , out hitInfo))
        {
            crossHairTarget.position = hitInfo.normal;
            tracer.transform.position = hitInfo.point;

            //射击碰撞火苗
            //hitEfx.transform.position = hit
            hitEfx.Emit(1);
        }
    }

}
