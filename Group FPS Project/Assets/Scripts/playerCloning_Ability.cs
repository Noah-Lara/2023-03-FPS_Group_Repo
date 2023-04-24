using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerCloning_Ability : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] int travelDistance;
    [SerializeField] int Health;

    [SerializeField] Transform shootPos;
    [SerializeField] int faceSpeed;
    [SerializeField] int shootDist;
    [SerializeField] int waitTime;

    Vector3 dest;
    Vector3 enemyDir;
    bool isShooting;
    bool isMoving;
    bool enemyinRange;
    GameObject ene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) {
            enemyinRange = true;
            GameObject ene = other.gameObject;
        }
    }
    private void Update()
    {
        //Moves clone to target location where camera is facing else it follows the player
        if (Input.GetButton("Ab"))
        {
            StartCoroutine(Move());
        }
        else if (!isMoving)
        {
            Follow();
        }
    }

    public void Follow()
    {
        agent.SetDestination(gameManager.instance.player.transform.position);
    }

    //Moves clone anywhere that is hit 
    IEnumerator Move()
    {
        RaycastHit position;
        isMoving = true;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out position, travelDistance))
        {
            if (position.collider != null)
            {
                agent.SetDestination(position.point);
            }
            if (CanseeEnemy())
            {
                if (!isShooting && Input.GetButton("Shoot"))
                {
                    StartCoroutine(Shoot());
                }
            }
        }
        yield return new WaitForSeconds(waitTime);
        isMoving = false;
    }

    //Checks within Sphere
    bool CanseeEnemy()
    {
        if (enemyinRange)
        {
            enemyDir = (ene.transform.position - shootPos.position).normalized;

            RaycastHit hit;
            if (Physics.Raycast(shootPos.position, enemyDir, out hit))
            {
                if (hit.collider)
                {
                    if (agent.remainingDistance < agent.stoppingDistance)
                    {
                        faceEnemy();
                    }
                    return true;
                }
            }
        }
        return false;
    }

    //Turns to face enemy
    void faceEnemy()
    {
        enemyDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(enemyDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceSpeed);
    }

    //Shoots toward enemy
    IEnumerator Shoot()
    {
        isShooting = true;

        RaycastHit hit;
        Debug.DrawRay(shootPos.position, enemyDir);
        if (Physics.Raycast(shootPos.position, enemyDir, out hit))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().takeDamage(gameManager.instance.playerScript.spellShootDamage);
            }
            //Instantiate(bulletHitEffect, hit.point, bulletHitEffect.transform.rotation);
        }

        yield return new WaitForSeconds(1);
        isShooting = false;
    }

    public void takeDamage(int dmg)
    {
        Health -= dmg;
        if (Health <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            agent.enabled = false;
            Destroy(gameObject);
        }
    }
}
