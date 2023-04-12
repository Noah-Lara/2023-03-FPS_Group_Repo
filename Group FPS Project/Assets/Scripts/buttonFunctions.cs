using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class buttonFunctions : MonoBehaviour
{
    GameObject returnMenu;
    GameObject returnToOptions;
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
    public void BackToOptions()
    {
        gameManager.instance.activeMenu.SetActive(false);
        gameManager.instance.activeMenu = returnToOptions;
        gameManager.instance.activeMenu.SetActive(true);
    }

    public void AudioMenu()
    {
        gameManager.instance.activeMenu.SetActive(false);
        returnToOptions = gameManager.instance.activeMenu;
        gameManager.instance.activeMenu = gameManager.instance.AudioMenu;
        gameManager.instance.activeMenu.SetActive(true);
    }
}
