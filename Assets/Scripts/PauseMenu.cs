using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Debugging;

public class PauseMenu : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public bool isPaused;
    void Start()
    {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameManager.gameOver && Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame(){
        Debugging.DebugLog("Game Paused.");
        if(!gameManager.gameOver){
            if(pauseMenu){
                pauseMenu.SetActive(true);
                gameManager.audioSource.Pause();
            }
            if(pauseButton){
                pauseButton.SetActive(false);
            }
            Time.timeScale = 0f;
            isPaused=true;
            }
    }

    public void ResumeGame(){
        Debugging.DebugLog("Resuming Game.");
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
        isPaused=false;
        gameManager.audioSource.Play();
    }

    public void GoToMainMenu(){
        Debugging.DebugLog("Changing scene to Menu");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScreen");
    }
}
