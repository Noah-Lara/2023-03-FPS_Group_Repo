using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneExit : MonoBehaviour
{
    public string sceneToLoad;
    public loadLevel lvlLoader;

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.instance.enemiesRemaining <= 0)
        {
            if (other.CompareTag("Player"))
            {
                gameManager.instance.levelFinish();
                gameManager.instance.loadNextlevel = true;
                //SceneManager.LoadScene(sceneToLoad);
                lvlLoader.loadNextLevel();
            }
        }
    }

}