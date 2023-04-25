using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public loadLevel lvlLoader;
    public Animator slide;

    public void PlayGame()
    {
        lvlLoader.loadNextLevel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
