using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Manager;

public class EnemySpawnpoint : MonoBehaviour
{
    [SerializeField]
    private AIController aiController;
    [SerializeField]
    private Transform[] patrolPoints;

    public void Start()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        if (aiController == null) return;
        GameObject inst = Instantiate(aiController.prefab);
        inst.name = aiController.name;
        inst.transform.position = transform.position;

        EnemyAI enemyAI = inst.GetComponent<EnemyAI>();

        enemyAI.aiController = aiController;

        for(int i = 0; i < patrolPoints.Length; i++)
        {
            enemyAI.path.Add(patrolPoints[i]);
        }

    }
}
