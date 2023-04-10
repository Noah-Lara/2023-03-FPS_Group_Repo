using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage, IPhysics
{
    [Header("-----Components-----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody rb;

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
    [SerializeField] float bulletSpeedY;
    [SerializeField] Transform shootPos;

    //[Header("-----Sword Stats-----")]
    //[SerializeField] Collider swordCol;

    //Variables
    ItemDrop drop;
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
        stoppingDistOrg = agent.stoppingDistance;
        startingPos = transform.position;
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
            agent.stoppingDistance = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);

            if (playerInRange)
            {
                if (canSeePlayer())
                {
                    StartCoroutine(roam());
                }
            }
            else if (agent.destination != gameManager.instance.player.transform.position)
            {
                StartCoroutine(roam());
            }
        }
    }

    //Fuction that Vector3 vs sightangle to determine if player is in range and facing player
    bool canSeePlayer()
    {
        playerDir = (gameManager.instance.player.transform.position - headPos.position);
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);
        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightAngle)
            {
                agent.stoppingDistance = stoppingDistOrg;
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    FacePlayer();
                }

                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
                return true;
            }
        }
        return false;
    }

    //Function that takes a random stop in the radius thats on the NavMesh and moves enemy.
    IEnumerator roam()
    {
        if (!destinationChosen && agent.remainingDistance < 0.05)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(waitTime);
            destinationChosen = false;

            Vector3 ranDir = Random.insideUnitSphere * roamDist;
            ranDir += startingPos;
            NavMeshHit hit;
            NavMesh.SamplePosition(ranDir, out hit, roamDist, 1);

            agent.SetDestination(hit.position);
        }
    }

    void FacePlayer()
    {
        playerDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    // Method call for the animator | Creates a bullet
    public void createBullet()
    {
        GameObject bulletClone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = (transform.forward + new Vector3(0, bulletSpeedY, 0)) * bulletSpeed;
    }

    //Damages the Enemy
    public void takeDamage(int dmg)
    {
        HP -= dmg;

        if (HP <= 0)
        {
            StopAllCoroutines();
            anim.SetBool("Dead", true);
            
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            agent.enabled = false;
            gameManager.instance.updateGameGoal(-1, gameObject);
        }
        else
        {
            anim.SetTrigger("Damage");
            //swordColOff();
            StartCoroutine(flashMat());
            agent.SetDestination(gameManager.instance.player.transform.position);
        }
    }

    //flashes a color on enemy to detect a hit
    IEnumerator flashMat()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    //public void swordColOn()
    //{
    //    swordCol.enabled = true;
    //}

    //public void swordColOff()
    //{
    //    swordCol.enabled = false;
    //}

    public void takeForce(Vector3 direction)
    {
        rb.velocity = direction * 0.3f;
    }
}
