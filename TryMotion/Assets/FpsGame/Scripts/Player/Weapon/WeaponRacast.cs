using L_Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRacast : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initPos;
        public Vector3 initVel;
        public TrailRenderer tracer;//激光
    }

    //private
    ParticleSystem muzzleEfx;
    ParticleSystem hitEfx;
    Transform raycastOrigin;
    Transform aimLookAt;
    CrossHairTarget crossHair;


    [Header("射击")]
    [SerializeField] int fireRate = 25;
    [SerializeField] int bulletCount = 50;
    [SerializeField] float cartridgeInvertal = 3f;//换弹夹间隔
    [SerializeField] float bulletSpeed = 1000f;
    [SerializeField] float bulletDrop = 0.0f;
    [SerializeField] float maxLifeTime = 3;

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
        }

        //else
        aimLookAt = Camera.main.transform.Find("AimLookAt");
        crossHair = CrossHairTarget.Instance;
        //去获得.墙体弹痕特效
        GameObject metalEfx = AllPoolMgr.Instance.missilesPool.FindCorrespondingPoolByPrefName("BulletImpactMetalEffect").Recycle();
        metalEfx.SetActive(true);
        hitEfx = metalEfx.GetComponent<ParticleSystem>();
        
    }


    void Update()
    {

    }

    #region 射击

    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;//射击累计时间
    List<Bullet> bulletList = new List<Bullet>();

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

        //创建子弹
        Vector3 velocity = (crossHair.DetectCrossHairTarget() - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position , velocity);
        bulletList.Add(bullet);
    }

    #region Bullet!

    //计算子弹下坠
    Vector3 CalBulletDrop(Bullet bullet)
    {
        //pos + v*t + 0.5*g*t*t  //子弹计算公式
        Vector3 gravity = Vector3.down * bulletDrop;
        return bullet.initPos + (bullet.initVel * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    //创建子弹
    //赋值 位置、速度
    Bullet CreateBullet(Vector3 pos , Vector3 vel)
    {
        Bullet bullet = new Bullet();
        bullet.initPos = pos;
        bullet.initVel = vel;
        bullet.time = 0.0f;
        bullet.tracer = AllPoolMgr.Instance.missilesPool.FindCorrespondingPoolByPrefName("TrailEfx").Recycle().GetComponent<TrailRenderer>();
        bullet.tracer.AddPosition(pos);
        return bullet;
    }

    public void UpdateBullet(float deltaTime)
    {
        SimulateBullet(deltaTime);
        DestroyBullets();
    }
    void SimulateBullet(float deltaTime)
    {
        bulletList.ForEach(bullet =>
        {
            Vector3 p0 = CalBulletDrop(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = CalBulletDrop(bullet);
            RaycasstSegment(p0,p1,bullet);
        });
    }
    void DestroyBullets() 
    {
        bulletList.RemoveAll(bullet => bullet.time> maxLifeTime);
    }

    void RaycasstSegment(Vector3 startPos , Vector3 endPos , Bullet bullet)
    {
        var dir = endPos - startPos;
        float distance = dir.magnitude;
        ray.origin = startPos;
        ray.direction = dir;
        //射击检测
        if (Physics.Raycast(ray , out hitInfo , distance))
        {
            bullet.tracer.transform.position = hitInfo.point;   //激光线
            bullet.time = maxLifeTime;

            //射击碰撞火苗
            hitEfx.transform.position = hitInfo.point;
            hitEfx.transform.forward = hitInfo.normal;
            hitEfx.Emit(1);
        }
        else
        {
            bullet.tracer.transform.position = endPos;
        }
    }

    #endregion

    #endregion

}
