using UnityEngine;
using System.Collections;

public abstract class AIState : MonoBehaviour
{
    //protected
    protected AIStateMachine _aIStateMachine;

    //abstract
    public abstract AIStateType GetStateType();
    public abstract AIStateType OnUpdate();

    //private 
    private AITargetType _curType;

    //public Porperty
    public AITargetType curType { get { return _curType; } set { _curType = value; } }

    //Default Handlers 
    public virtual void OnEnterState() { }
    public virtual void OnExitState() { }
    public virtual void OnAnimatorUpdated()
    {
        //Debug.LogError("动画deltaPosition:" +  _aIStateMachine.Anim.deltaPosition);
        //Debug.LogError("动画rootRotation:" + _aIStateMachine.Anim.rootRotation);
        //得到根运动为这个更新所更新的米的数量，并除以deltaTime得到米每秒。  (速度=路程/时间)
        //然后我们把这个分配给nav代理的速度。
        if (_aIStateMachine.useRootPosition)
            _aIStateMachine.Agent.velocity = _aIStateMachine.Anim.deltaPosition / Time.deltaTime;

        // 从animator中获取根旋转并赋值为transfrom的旋转。
        if (_aIStateMachine.useRootRotation)
            _aIStateMachine.transform.rotation = _aIStateMachine.Anim.rootRotation;
    }
    public virtual void OnAnimatorIKUpdated() { }
    public virtual void OnTriggerEvent(AITriggerEventType aITriggerEventType, Collider other) { }
    public virtual void OnDestinationReached(bool isReached) { }


    //public
    public virtual void SetAIStateMachine(AIStateMachine aIStateMachine)
    {
        _aIStateMachine = aIStateMachine;
    }

    #region >>>>>>>供外部[计算/调用]
    /// <summary>
    /// 转换球形碰撞器到世界坐标轴
    /// </summary>
    public static void ConvertSphereColliderToWorldSpace(SphereCollider sphere , out Vector3 soundPos , out float soundRadius)
    {
        float x = sphere.center.x * sphere.transform.lossyScale.x;
        float y = sphere.center.y * sphere.transform.lossyScale.y;
        float z = sphere.center.z * sphere.transform.lossyScale.z;

        soundPos = new Vector3(x , y , z);
        Debug.LogError(soundPos);

        float temp1 = Mathf.Max(sphere.radius * sphere.transform.lossyScale.x , sphere.radius * sphere.transform.lossyScale.y);
        float temp2 = Mathf.Max(temp1 , sphere.radius * sphere.transform.lossyScale.z);
        soundRadius = temp2;
    }

    /// <summary>
    /// 返回,带符号的角度.
    /// </summary>
    public static float FindSingleAngle(Vector3 from , Vector3 to)
    {
        float angle = Vector3.Angle(from.normalized , to.normalized);
        Vector3 temp = Vector3.Cross(from.normalized , to.normalized);
        angle = temp.y > 0 ? angle : -angle;
        return angle;
    }

    /// <summary>
    /// 计算概率,通过传递一个Vector2区间. 按照%计算.
    /// <para>flase: 返回小概率区间 (x代表小区间)</para>
    /// <para>true:  返回大概率区间 (y代表大区间)</para>
    /// </summary>
    public static bool CalcProbability(Vector2 probability)
    {
        int value = 0;
        int min = System.Convert.ToInt32(probability.x * 100);
        int max = System.Convert.ToInt32(probability.y * 100);
        value = UnityEngine.Random.Range(0 , 100);
        return value >= min? true:false;
    }
    #endregion


}

///// <summary>
///// AI状态重置接口
///// </summary>
//public interface IAIStateRest 
//{
//    void ResetAIState();
//}
