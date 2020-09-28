using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDrawGizmos_Test : MonoBehaviour
{

    [SerializeField] AIStateMachine_MonkeyKing _enemyStateMachine;

    [Header("绘制范围")]
    public bool enemyAtkRange;
    public bool patrolRange;
    public bool soundTriggerRange;

    #region OnDrawGizmos-绘制

    [Header("编辑器状态下-OnDrawGizmos")]
    [Header("视野参数")]
    [SerializeField] int fov_VertexCount = 25;//定义圆圈边定点数量，数值越高越平滑
    [SerializeField] float fov_Radius = 5;//圆形半径
    [Header("巡逻参数")]
    [SerializeField] int patrol_VertexCount = 50;//定义圆圈边定点数量，数值越高越平滑
    [SerializeField] float patrol_Radius = 5;//圆形半径
    [Header("听觉参数")]
    [SerializeField] int sound_VertexCount = 50;//定义圆圈边定点数量，数值越高越平滑
    [SerializeField] float sound_Radius = 10;//圆形半径
    //绘制Gizmo
    protected void OnDrawGizmos()
    {
        //敌人攻击范围 (红色)
        if (_enemyStateMachine != null && enemyAtkRange)
        {
            float deltaTheta = (_enemyStateMachine.Fov * (Mathf.PI / 180)) / fov_VertexCount;//进过顶点平分后的θ度数.
            float theta = 0;//起始点绘制
            theta = transform.localEulerAngles.y + 45;
            Vector3 oldPos = transform.position;
            for (int i = 0; i < fov_VertexCount + 1; i++)
            {
                Vector3 pos = new Vector3(fov_Radius * Mathf.Cos(theta), 0.25f, fov_Radius * Mathf.Sin(theta));
                Gizmos.color = Color.red;
                Gizmos.DrawLine(oldPos, transform.position + pos);
                oldPos = transform.position + pos;

                theta += deltaTheta;
            }
            Gizmos.DrawLine(oldPos, transform.position);
        }

        //敌人巡逻范围 (蓝色)
        if (_enemyStateMachine != null && patrolRange)
        {
            float deltaTheta = 2 * Mathf.PI  / patrol_VertexCount;//进过顶点平分后的θ度数.
            float theta = 0;//起始点绘制
            Vector3 oldPos = transform.position;
            for (int i = 0; i < patrol_VertexCount + 1; i++)
            {
                Vector3 pos = new Vector3(patrol_Radius * Mathf.Cos(theta), 0.25f, patrol_Radius * Mathf.Sin(theta));
                Gizmos.color = Color.blue;
                if (i != 0) { Gizmos.DrawLine(oldPos, transform.position + pos); }
                oldPos = transform.position + pos;

                theta += deltaTheta;
            }
        }

        //敌人听觉范围 (绿色)
        if (_enemyStateMachine != null && soundTriggerRange)
        {
            float deltaTheta = 2 * Mathf.PI / patrol_VertexCount;//进过顶点平分后的θ度数.
            float theta = 0;//起始点绘制
            Vector3 oldPos = transform.position;
            for (int i = 0; i < sound_VertexCount + 1; i++)
            {
                Vector3 pos = new Vector3(_enemyStateMachine.SensorRaduis * Mathf.Cos(theta), 0.25f, _enemyStateMachine.SensorRaduis * Mathf.Sin(theta)); //sound_Radius
                Gizmos.color = Color.green;
                if (i != 0) { Gizmos.DrawLine(oldPos, transform.position + pos); }
                oldPos = transform.position + pos;

                theta += deltaTheta;
            }
        }
    }

    #endregion

}
