using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gameManager.instance.unpauseState();
        gameManager.instance.isPaused = !gameManager.instance.isPaused;
    }

    public void restart()
    {
        gameManager.instance.unpauseState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void options()
    {
        gameManager.instance.activeMenu = gameManager.instance.optionMenu;
        gameManager.instance.activeMenu.SetActive(true);
        gameManager.instance.pauseMenu.SetActive(false);
    }
    public void respawnPlayer()
    {
        gameManager.instance.unpauseState();
        gameManager.instance.playerScript.respawnPlayer();
    }
    public void quit()
    {
        Application.Quit();
    }
}
