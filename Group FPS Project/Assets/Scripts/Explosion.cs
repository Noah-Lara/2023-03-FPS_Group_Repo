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
        IPhysics physicable = other.GetComponent<IPhysics>();
        if (physicable != null && !isHit)
        {
            isHit = true;
            
            if (Pulling)
            {
               physicable.takeForce((transform.position - other.transform.position) * explosionAmount, damage);
            }
            else
            {
                physicable.takeForce((other.transform.position - transform.position) * explosionAmount, damage);
            }
        }
    }
}
