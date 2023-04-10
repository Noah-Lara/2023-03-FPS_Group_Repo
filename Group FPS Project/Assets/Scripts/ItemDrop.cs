using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    
    //[SerializeField] float health = 100f;
    
    // Start is called before the first frame update
    
    public void dropcoin(Vector3 destroyedObjectPosition, GameObject itemModel)
    {
        Vector3 position = destroyedObjectPosition;//position of the enemy or destroyed object 
        GameObject item = Instantiate(itemModel, position + new Vector3(0f,1f,0f), Quaternion.identity);// Item Drop
        item.SetActive(true);//set the coin object to active
        Destroy(item, 30f);//Destroy the item afte x amount of time
    }
}
