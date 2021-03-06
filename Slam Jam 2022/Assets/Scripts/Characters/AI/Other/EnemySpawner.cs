using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Vector2Int enemiesToSpawnRange;

    [SerializeField] AIManager[] agentOptions;

    [SerializeField] float spawnAreaRange = 5;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        var rand = Random.Range(enemiesToSpawnRange.x, enemiesToSpawnRange.y + 1);

        for(int i = 0; i < rand; i++)
        {
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnAreaRange;

            if (NavMesh.SamplePosition(spawnPos, out var hit, spawnAreaRange, NavMesh.AllAreas))
            {
                var rot = Random.rotation;

                var go = Instantiate(agentOptions.ChooseRandomElementInArray(), hit.position, rot);

                GameManager._enemies.Add(go.gameObject);
            }
        }

        this.PerformAtEndOfFrame(GameManager.LevelStart);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spawnAreaRange);
    }
}
