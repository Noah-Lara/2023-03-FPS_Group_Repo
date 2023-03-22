using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] int numToSpawn;
    [SerializeField] int timer;
    [SerializeField] Transform[] spawnpos;

    int numSpawned;
    bool isSpawning;
    bool playerInTrigger;

    private void Start()
    {
        gameManager.instance.updateEnemyTotal(numToSpawn);
    }

    private void Update()
    {
        if (playerInTrigger)
        {
            if (!isSpawning && numSpawned < numToSpawn)
            {
                StartCoroutine(spawn());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;

        GameObject enemyClone = Instantiate(enemy, spawnpos[Random.Range(0, spawnpos.Length)].transform.position, enemy.transform.rotation);
        gameManager.instance.updateGameGoal(1, enemyClone);
        numSpawned++;

        yield return new WaitForSeconds(0);
        isSpawning = false;
    }
}
