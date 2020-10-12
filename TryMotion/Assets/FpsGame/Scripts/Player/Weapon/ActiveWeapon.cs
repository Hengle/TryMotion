using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ActiveWeapon : MonoBehaviour
{

    #region 属性

    Transform weaponParent;
    Transform WeaponParent
    {
        get
        {
            if (weaponParent == null)
            {
                weaponParent = transform.Find("WeaponHolder/WeaponPivot");
            }
            return weaponParent;
        }
    }

    Rig handRig;
    Rig HandRig 
    {
        get 
        {
            if (handRig == null)
            {
                handRig = transform.Find("Rig Setup/RigLayer_HandIK").GetComponent<Rig>();
            }
            return handRig;
        }
    }

    #endregion

    WeaponRacast currentWeapon;

    [Header("武器测试")]
    public GameObject weapon_1;
    public GameObject weapon_2;

    WeaponRacast weaponRacast1;
    WeaponRacast weaponRacast2;

    void Start()
    {
        WeaponRacast weaponRacast = GetComponentInChildren<WeaponRacast>(true);
        if (weaponRacast)
        {
            EquipWeapon(weaponRacast);
        }

        weaponRacast1 = Instantiate(weapon_1).GetComponent<WeaponRacast>();
        weaponRacast2 = Instantiate(weapon_2).GetComponent<WeaponRacast>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponRacast1.gameObject.SetActive(true);
            weaponRacast2.gameObject.SetActive(false);
            EquipWeapon(weaponRacast1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponRacast1.gameObject.SetActive(false);
            weaponRacast2.gameObject.SetActive(true);
            EquipWeapon(weaponRacast2);
        }
    }

    public void EquipWeapon(WeaponRacast wr) 
    {
        currentWeapon = wr;
        currentWeapon.transform.SetParent(WeaponParent,false);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localEulerAngles = Vector3.zero;

        HandRig.weight = 1;
    }



}
