using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(RequireComponent))]
public class PlayerTestNavAgent : MonoBehaviour
{

    [Header("区域目标:")]
    public Transform part1_pos;
    public Transform part2_pos;
    [Header("当前目标"),Space]
    public Transform currentTarget;

    #region 属性

    private NavMeshAgent agent;
    public NavMeshAgent Agent
    {
        get
        {
            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
            }
            return agent;
        }
    }

    #endregion

    void Start()
    {
        currentTarget = part1_pos;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget != null)
        {

            if (Vector3.Distance(transform.position , part1_pos.position) <= 0.5f)
            {
                Debug.LogError("切换目标！");
                currentTarget = part2_pos;
            }
            if (Vector3.Distance(transform.position , part2_pos.position) <= 0.5f)
            {
                Debug.LogError("切换目标！");
                currentTarget = part1_pos;
            }

            Agent.SetDestination(currentTarget.position);
        }
    }
}
