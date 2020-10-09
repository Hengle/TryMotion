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
    [SerializeField] int fireRate = 25;
    [SerializeField] int bulletCount = 50;
    [SerializeField] float cartridgeInvertal = 3f;//换弹夹间隔

    //public
    public bool isFire = false;

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

    #region 射击

    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;//射击累计时间

    //射击
    int currentBulletNum = 0;
    float cartridgeTime = 0;


    public void StartFire()
    {
        isFire = true;
        accumulatedTime = 0;
        FireBullet();
    }

    public void UpdateFire(float DeltaTime)
    {
        if (currentBulletNum <= bulletCount)
        {
            accumulatedTime += DeltaTime;
            float fireInvertal = 1.0f / fireRate;//发射速率
            while (accumulatedTime >= fireInvertal)
            {
                currentBulletNum++;
                FireBullet();
                accumulatedTime -= fireInvertal;
            }
        }
        else
        {
            //换弹夹
            cartridgeTime += DeltaTime;
            if (cartridgeTime >= cartridgeInvertal)
            {
                currentBulletNum = 0;
            }
        }
    }

    public void StopFire()
    {
        isFire = false;
    }


    void FireBullet()
    {
        //枪焰
        muzzleEfx.Emit(1);
        //发射Ray
        ray.origin = raycastOrigin.position;
        ray.direction = crossHairTarget.position - raycastOrigin.position;

        //生成激光线
        var tracer = Instantiate(trailRenderer , ray.origin , Quaternion.identity);
        tracer.AddPosition(ray.origin);

        //射击检测
        if (Physics.Raycast(ray , out hitInfo))
        {
            crossHairTarget.position = hitInfo.point;  //十字瞄准目标
            tracer.transform.position = hitInfo.point;  //激光线

            ///射击碰撞火苗
            hitEfx.transform.position = hitInfo.point;
            hitEfx.transform.forward = hitInfo.normal;
            hitEfx.Emit(1);
        }
    }

    #endregion



}
