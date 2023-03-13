using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("-----Enemy Stats-----")]
    [SerializeField] Transform headPos;
    [SerializeField] int HP;
    [SerializeField] int roamDist;
    [SerializeField] int sightAngle;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int waitTime;

    [Header("-----Gun Stats-----")]
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletSpeed;
    [SerializeField] Transform shootPos;

    bool isShooting;
    public bool playerInRange;
    Vector3 playerDir;
    float angleToPlayer;
    bool destinationChosen;
    float stoppingDistOrg;
    Vector3 startingPos;

    //Checks Game-manager to increase total number of enemies
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);
            if (!isShooting)
            {
                StartCoroutine(shoot());
            }
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        GameObject bulletClone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    //Damages the Enemy
    public void takeDamage(int dmg)
    {
        agent.SetDestination(gameManager.instance.player.transform.position);
        HP -= dmg;
        StartCoroutine(flashMat());
        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    //flashes a color on enemy to detect a hit
    IEnumerator flashMat()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
}
