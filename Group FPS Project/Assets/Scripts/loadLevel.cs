using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadLevel : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public void loadNextLevel()
    {
        StartCoroutine(levelLoad(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator levelLoad(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);

    }
}
