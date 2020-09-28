using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 非网格链接移动方法
/// </summary>
public enum OffMeshLinkMoveMethod
{
    /// <summary>
    /// 传送
    /// </summary>
    Teleport,
    /// <summary>
    /// 正常速度
    /// </summary>
    NormalSpeed,
    /// <summary>
    /// 抛物线
    /// </summary>
    Parabola,
    /// <summary>
    /// 曲线
    /// </summary>
    Curve
}

public class AgentLinkMover : MonoBehaviour
{

    public OffMeshLinkMoveMethod method = OffMeshLinkMoveMethod.Parabola;
    public AnimationCurve curve = new AnimationCurve();
    public float CurveTime = .5f;
    public float FaceTime = .1f;

    public delegate void OnStartEvent();
    public event OnStartEvent OnStart;
    public delegate void OnCompleteEvent();
    public event OnCompleteEvent OnComplete;

    IEnumerator Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        while (true)
        {
            //当处于,非网格连接中...
            if (agent.isOnOffMeshLink)
            {
                //先面向目标
                yield return StartCoroutine(FaceToTarget(agent , FaceTime));

                if (method == OffMeshLinkMoveMethod.NormalSpeed)
                    yield return StartCoroutine(NormalSpeed(agent));
                else if (method == OffMeshLinkMoveMethod.Parabola)
                    yield return StartCoroutine(Parabola(agent , 2.0f , 0.5f));
                else if (method == OffMeshLinkMoveMethod.Curve)
                    yield return StartCoroutine(Curve(agent , CurveTime));

                //触发 CompleteOffMesh 事件
                agent.CompleteOffMeshLink();
                agent.updateRotation = true;
                //自身定义的事件.
                //OnComplete?.Invoke();
            }
            yield return null;
        }
    }

    /// <summary>
    /// 朝向目标
    /// </summary>
    IEnumerator FaceToTarget(NavMeshAgent agent , float duration)
    {
        agent.updateRotation = false;
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Quaternion startRotation = agent.transform.rotation;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        var endRotation = Quaternion.LookRotation(new Vector3(endPos.x - agent.transform.position.x , 0 , endPos.z - transform.position.z));
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            agent.transform.rotation = Quaternion.Slerp(startRotation , endRotation , normalizedTime);
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        //OnStart?.Invoke();
    }

    /// <summary>
    /// 正常移动
    /// </summary>
    IEnumerator NormalSpeed(NavMeshAgent agent)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        while (agent.transform.position != endPos)
        {
            agent.transform.position = Vector3.MoveTowards(agent.transform.position , endPos , agent.speed * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// 抛物线
    /// </summary>
    IEnumerator Parabola(NavMeshAgent agent , float height , float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos , endPos , normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }
    /// <summary>
    /// 曲线
    /// </summary>
    IEnumerator Curve(NavMeshAgent agent , float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            float yOffset = curve.Evaluate(normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos , endPos , normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }

}
