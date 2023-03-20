using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    private void LateUpdate()
    {
        //Follows the player position
        gameObject.transform.position = new Vector3(gameManager.instance.player.transform.position.x, transform.position.y, gameManager.instance.player.transform.position.z);
        //Rotates Camera with Player
        gameObject.transform.rotation = Quaternion.Euler(90f, gameManager.instance.player.transform.eulerAngles.y, 0f);
    }
}
