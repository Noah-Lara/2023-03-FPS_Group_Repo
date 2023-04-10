using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    GameObject returnMenu;
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
        returnMenu = gameManager.instance.activeMenu;
        gameManager.instance.activeMenu = gameManager.instance.optionMenu;
        gameManager.instance.activeMenu.SetActive(true);
        returnMenu.SetActive(false);
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
    public void returnToPause()
    {
        gameManager.instance.activeMenu.SetActive(false);
        gameManager.instance.activeMenu = returnMenu;
        gameManager.instance.activeMenu.SetActive(true);        
    }
}
