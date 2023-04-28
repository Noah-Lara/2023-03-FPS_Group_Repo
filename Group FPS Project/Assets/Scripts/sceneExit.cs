using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneExit : MonoBehaviour
{
    public string sceneToLoad;
    public loadLevel lvlLoader;
    public Spawner spawner;

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.instance.enemiesRemaining <= 0 && spawner.wave >= spawner.totalWaves)
        {
            if (other.CompareTag("Player"))
            {
                gameManager.instance.finished = true;
                gameManager.instance.levelFinish();
                gameManager.instance.loadNextlevel = true;
                //SceneManager.LoadScene(sceneToLoad);
                lvlLoader.loadNextLevel();
            }
        }
    }

}