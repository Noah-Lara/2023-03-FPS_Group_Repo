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

    Vector3 startPos;
    Vector3 dest;
    // Start is called before the first frame update
    void Start()
    {
       startPos = transform.position;
       dest = new Vector3(transform.position.x + MoveDist_X, transform.position.y + MoveDist_Y, transform.position.z + MoveDist_Z);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.instance.enemiesRemaining == 1)
        {
          StartCoroutine(RaiseGate());
        }
    }

    IEnumerator RaiseGate()
    {
        float timeCurr = 0;
        while (timeCurr < Duration)
        {
            transform.position = Vector3.Lerp(startPos, dest, timeCurr/Duration);
            timeCurr += Time.deltaTime;
            yield return new WaitForSeconds(0);
        }
        transform.position = dest;
    }
}
