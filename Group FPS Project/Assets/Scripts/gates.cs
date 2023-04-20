using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gates : MonoBehaviour
{
    [SerializeField] float Movespeed;
    [SerializeField] float Duration;
    [SerializeField] int MoveDist_X;
    [SerializeField] int MoveDist_Y;
    [SerializeField] int MoveDist_Z;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip clip;

    bool playerInArea;

    Vector3 startPos;
    Vector3 dest;
    // Start is called before the first frame update
    void Start()
    {
        playerInArea = false;
       startPos = transform.position;
       dest = new Vector3(transform.position.x + MoveDist_X, transform.position.y + MoveDist_Y, transform.position.z + MoveDist_Z);
    }

    // Update is called once per frame
    void Update()
    {

        StartCoroutine(RaiseGate());

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = true;
        }
    }
    IEnumerator RaiseGate()
    {
        
        yield return new WaitForSeconds(30);
        if (gameManager.instance.enemiesRemaining == 0 && playerInArea)
        {
            
            float timeCurr = 0;
            while (timeCurr < Duration)
            {
                transform.position = Vector3.Lerp(startPos, dest, timeCurr / Duration);
                timeCurr += Time.deltaTime;

            }
            transform.position = dest;
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }
        
    }
}
