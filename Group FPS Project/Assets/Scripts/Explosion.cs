using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int explosionAmount;
    [SerializeField] bool Pulling;
    [SerializeField] Renderer model;
    [SerializeField] Collider col;
    bool isHit;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        model.enabled = false;
        col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isHit)
        {
            isHit = true;
            gameManager.instance.playerScript.takeDamage(damage);
            if (Pulling)
            {
                gameManager.instance.playerScript.pushBackInput((transform.position - gameManager.instance.player.transform.position) * explosionAmount);
            }
            else
            {
                gameManager.instance.playerScript.pushBackInput((gameManager.instance.player.transform.position - transform.position) * explosionAmount);
            }
        }
    }
}
