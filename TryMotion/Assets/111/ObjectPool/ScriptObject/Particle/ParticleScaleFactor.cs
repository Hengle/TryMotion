using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 粒子缩放因子
/// </summary>
public class ParticleScaleFactor : MonoBehaviour
{

    [SerializeField] List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    public GameObject _Target;

    void Awake()
    {
        ParticleSystem[] psList = GetComponentsInChildren<ParticleSystem>(true);
        for (int i = 0; i < psList.Length; i++)
        {
            particleSystems.Add(psList[i]);
        }
    }

    private void Update()
    {
        if (_Target == null)
        {
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 放大
    /// </summary>
    public void Scale2Amp(float scaleFactor)
    {
        for (int i = 0; i < particleSystems.Count; i++)
        {
            particleSystems[i].transform.localScale = Vector3.one * scaleFactor;
        }
    }



}
