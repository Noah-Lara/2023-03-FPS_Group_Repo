using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] float numToSpawn;
    [SerializeField] int timer;
    [SerializeField] Transform[] spawnpos;
    [SerializeField] public int totalWaves;
    
    bool isSpawning;
    public int wave;
    int numSpawned;    
    public bool playerInTrigger;
    
    private void Start()
    {
        playerInTrigger = false;
        wave = 1;
        //numToSpawn = numToSpawn * (wave * waveMultiplier);
        ////gameManager.instance.updateEnemyTotal(numToSpawn);
    }

    private void Update()
    {
        if (playerInTrigger)
        {
            
                if (!isSpawning && numSpawned < numToSpawn )
                    StartCoroutine(spawn());
            
        }
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            if (numSpawned >= numToSpawn && wave < totalWaves)
            {
                
                playerInTrigger = false;
                StartCoroutine(nextWave());
            }
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;
            GameObject enemyClone = Instantiate(enemy, spawnpos[Random.Range(0, spawnpos.Length)].transform.position, enemy.transform.rotation);
            //gameManager.instance.updateGameGoal(0, enemyClone);
            numSpawned++;
            yield return new WaitForSeconds(timer);
        isSpawning = false;
    }
    IEnumerator nextWave()
    {
        if(wave <= totalWaves)
        {
            numToSpawn = numToSpawn * 1.2f;
            yield return new WaitForSeconds(25);
            wave++;
            numSpawned = 0;
            playerInTrigger = true;
        }
        
    }
}
