using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seekingProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    //[SerializeField] int Angle;
    [SerializeField] int sightAngle;
    Vector3 playerDir;
    float angleToTarget;
    
    [SerializeField] Transform headPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerDir = (gameManager.instance.player.transform.position - headPos.position);
        angleToTarget =  Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);
        if(angleToTarget <= sightAngle)
        {
            transform.position = Vector3.MoveTowards(transform.position, gameManager.instance.player.transform.position, speed * Time.deltaTime);
            transform.forward = gameManager.instance.player.transform.position - transform.position;
        }
        
    }
}
