/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-22 15:06:41
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 远程猴子(金) AIState
/// </summary>
public abstract class AIRemoteMonkeyGoldState : AIState
{
    //protected
    protected int _playerLayerMask = -1;
    protected int _bodyPartLayer = -1;
    protected int _visualLayerMask = -1;
    protected AIStateMachine_RemoteMonkey _enemyStateMachine = null;
    [SerializeField] protected bool isByDamage = false;


    public override void SetAIStateMachine(AIStateMachine aIStateMachine)
    {
        if (aIStateMachine.GetType() == typeof(AIStateMachine_RemoteMonkey))
        {
            _enemyStateMachine = (AIStateMachine_RemoteMonkey)aIStateMachine;
            TakeDamageDispatcher.Instance.AddEventListener(ConstKey_EnemyInfo.OnTakeDamage , OnTakeDamageEvent);
        }
        base.SetAIStateMachine(aIStateMachine);
    }

    protected abstract void ResetAIState();

    protected virtual void OnTakeDamageEvent(object[] objs)
    {
        DamageInfo info = (objs[0] as DamageInfo);
        _enemyStateMachine.attackerGo = info.atkerGo;
    }

    protected virtual void Awake()
    {
        //得到一个mask(蒙版层)="player"; (+1)意思是包含default默认层.
        _playerLayerMask = LayerMask.GetMask("Player" , "AI Body Part") + 1;
        _visualLayerMask = LayerMask.GetMask("Player" , "AI Body Part" , "Visual Aggravator") + 1;

        // Get the layer index of the AI Body Part layer
        _bodyPartLayer = LayerMask.GetMask("Player" , "AI Body Part");
    }

    public override void OnTriggerEvent(AITriggerEventType aITriggerEventType , Collider other)
    {
        base.OnTriggerEvent(aITriggerEventType , other);

        if (aITriggerEventType != AITriggerEventType.Exit)
        {
            //1.player进入触发区。 (visual_player)
            if (other.CompareTag(Tag.Player))
            {
                //威胁距离
                float distanceToThreat = Vector3.Distance(_aIStateMachine.sensorPosition , other.transform.position);

                //进行视觉威胁判断.
                //视觉威胁不是player 或者 (视觉威胁是player && 处在视野范围内)
                if (_aIStateMachine.VisualThreat.type != AITargetType.Visual_Player ||
                    _aIStateMachine.VisualThreat.type == AITargetType.Visual_Player && distanceToThreat < _aIStateMachine.VisualThreat.distance)
                {
                    RaycastHit hitInfo;
                    if (ColliderIsVisible(other , out hitInfo , _playerLayerMask))
                    {
                        Debug.LogError("看见主角!");//todo
                        _aIStateMachine.VisualThreat.Set(AITargetType.Visual_Player , other , other.transform.position , distanceToThreat);
                    }
                }
            }
            //2.声音传递
            if (other.CompareTag(Tag.FootSound))//这里可以多个Tag.
            {
                float distanceToSound = Vector3.Distance(_aIStateMachine.sensorPosition , other.transform.position);
                //依据音频源对象计算.
                //float distanceFactor = distanceToSound / soundRadius;   //由发出声音方计算
                float distanceFactor = distanceToSound / _enemyStateMachine.SensorRaduis;    //由接收声音方计算

                //再计算,基于Agent听力能力的因素偏倚。
                distanceFactor += distanceFactor * (1.0f - _enemyStateMachine.Hearing);
                //太远...

                if (distanceFactor > 1) { return; }

                if (distanceToSound <= _enemyStateMachine.AudioThreat.distance)
                {
                    //todo Debug.LogError("boss听到动静!");
                    _enemyStateMachine.AudioThreat.Set(AITargetType.Audio , other , other.transform.position , distanceToSound);
                }
            }
        }
    }

    /// <summary>
    /// 是否能检测到玩家的存在.(隔绝掉障碍物的阻挡)
    /// </summary>
    /// <returns></returns>
    public bool ColliderIsVisible(Collider other , out RaycastHit hitInfo , int layerMask = -1)
    {
        hitInfo = new RaycastHit();

        //1.先限制视野
        //判断是否处于FOV视野里面.
        Vector3 head = _aIStateMachine.sensorPosition;
        Vector3 direction = other.transform.position - head;
        float angle = Vector3.Angle(transform.forward.normalized , direction.normalized);
        _aIStateMachine = (AIStateMachine_MonkeyKing)_aIStateMachine;
        //Debug.LogError("视觉角度: " + angle);
        if (angle > _enemyStateMachine.Fov / 2)
        {
            return false;
        }

        float closestColliderDistance = float.MaxValue;
        Collider closestCollider = null;
        //2.再,剔除其他的物体(视觉)遮挡.
        RaycastHit[] raycastHits = Physics.RaycastAll(_aIStateMachine.sensorPosition , direction.normalized , _aIStateMachine.sensorRadius * _enemyStateMachine.Sight , _playerLayerMask);// 
        for (int i = 0; i < raycastHits.Length; i++)
        {
            if (raycastHits[i].distance < closestColliderDistance)
            {
                //ai_body层
                if (raycastHits[i].transform.gameObject.layer == _bodyPartLayer)
                {
                    //并且,不是自身.
                    if (_aIStateMachine != GameSceneManager.Instance.GetAiStateMachine(other.GetComponent<Rigidbody>().GetInstanceID()))
                    {
                        closestColliderDistance = raycastHits[i].distance;
                        closestCollider = raycastHits[i].collider;
                        hitInfo = raycastHits[i];
                    }
                }
                else
                {
                    closestColliderDistance = raycastHits[i].distance;
                    closestCollider = raycastHits[i].collider;
                    hitInfo = raycastHits[i];
                }
            }
        }
        //Debug.LogError("距离最近的敌人name: " + closestCollider.name);

        if (closestCollider == null) { return false; }
        if (other == null) { return false; }
        //满足:视野中存在player.
        if (closestCollider.gameObject == other.gameObject && closestCollider)
        {
            return true;
        }

        return false;
    }

}