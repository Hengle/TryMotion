using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffect : MonoBehaviour
{

    TrailRenderer trailRenderer;


    void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void OnEnable()
    {
        isTrunOff = false;
    }


    bool isTrunOff = false;
    float timer = 0;

    void Update()
    {
        if (!isTrunOff)
        {
            timer += Time.deltaTime;
            if (timer >= trailRenderer.time * 2)
            {
                timer = 0;
                isTrunOff = true;
                this.gameObject.SetActive(false);
            }
        }
    }
    
}
