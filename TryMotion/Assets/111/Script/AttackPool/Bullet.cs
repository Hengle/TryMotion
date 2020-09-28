using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int speed = 300;
    public GameObject atkerGo;

    void Start()
    {
        Destroy(this.gameObject,1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Enemy))
        {
            other.GetComponentInChildren<MonkeyKing_TakeDamage>().TakeDamage(atkerGo, 10);
        }
    }

}
