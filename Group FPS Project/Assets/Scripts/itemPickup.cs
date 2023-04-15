using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickup : MonoBehaviour
{
    [SerializeField] float speed;
    public int ExpAmount;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, gameManager.instance.player.transform.position, speed * Time.deltaTime);
        transform.forward = gameManager.instance.player.transform.position - transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            gameManager.instance.updatePlayerExperience(ExpAmount);
            Destroy(gameObject);
        }

        if (other.name == "Player" && gameObject.name == "Power_Up")
        {
            gameManager.instance.playerScript.jumpSpeed = gameManager.instance.playerScript.jumpSpeed * 2;
        }
    }
    
}
   
