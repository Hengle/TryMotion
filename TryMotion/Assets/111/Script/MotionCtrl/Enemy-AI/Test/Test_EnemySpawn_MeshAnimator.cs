/* ========================================================
*      作 者：
*      主 题：
*      主要功能：

*      详细描述：

*      创建时间：2020-09-09 13:32:30
*      修改记录：
*      版 本：1.0
 ========================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_EnemySpawn_MeshAnimator : MonoBehaviour
{
    public GameObject monkeyKingPref;

    [SerializeField, Range(1, 300)] int spawnAmount = 10;
    [SerializeField] float spawnRange = 20;
    [SerializeField] float moveSpeed = 3;

    List<GameObject> monkeyList = new List<GameObject>();

    [SerializeField,Header("当前生成的个数:")]int currentAmount = 0;
    IEnumerator Start()
    {
        while (true)
        {
            if (currentAmount <= spawnAmount)
            {
                currentAmount++;
                Vector3 targetPos = UnityEngine.Random.insideUnitSphere * spawnRange;
                targetPos.y = 0;
                GameObject go = Instantiate(monkeyKingPref, targetPos,Quaternion.identity);
                monkeyList.Add(go);
                //Vector3 currentPos = go.transform.position;
                //while (currentPos != targetPos)
                //{
                //    go.transform.position = Vector3.MoveTowards(currentPos, targetPos, Time.deltaTime * moveSpeed);
                //}
            }
            yield return null;
        }
    }

    void Update()
    {

    }
}
