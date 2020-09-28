using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_EnemySpawn : MonoBehaviour
{
    #region 单例
    private static Test_EnemySpawn _instance;
    public static Test_EnemySpawn Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Test_EnemySpawn>();
            }
            return _instance;
        }
    }
    #endregion



    public GameObject monkeyKingPref;
    public GameObject monsterPref;

    public Transform[] spawnPos;


    [Header("生成的敌人Info:")]
    public List<EnemyInfo> currentEnemyList = new List<EnemyInfo>();


    void Start()
    {
        GameObject boss = Instantiate(monkeyKingPref, spawnPos[0]);
        GameObject enemy1 = Instantiate(monsterPref, spawnPos[1]);
        GameObject enemy2 = Instantiate(monsterPref, spawnPos[2]);
        currentEnemyList.Add(boss.GetComponentInChildren<AIStateMachine>()._enemyInfo);
        currentEnemyList.Add(enemy1.GetComponentInChildren<AIStateMachine>()._enemyInfo);
        currentEnemyList.Add(enemy2.GetComponentInChildren<AIStateMachine>()._enemyInfo);

        
    }


}
