using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 伤害Info类
/// </summary>
public class DamageInfo
{
    public int damge;
    public GameObject atkerGo;
}


public class MonkeyKing_TakeDamage : MonoBehaviour
{
    AIStateMachine_MonkeyKing _enemyAIStateMachine;

    void Start()
    {
        _enemyAIStateMachine = GetComponentInChildren<AIStateMachine_MonkeyKing>();
    }

    public void TakeDamage(GameObject atkerGo,int damage)
    {
        _enemyAIStateMachine.Health -= damage;

        DamageInfo info = new DamageInfo()
        {
            damge = damage,
            atkerGo = atkerGo
        };

        TakeDamageDispatcher.Instance.Dispatcher(ConstKey_EnemyInfo.OnTakeDamage, new object[1] { info });
    }

}
