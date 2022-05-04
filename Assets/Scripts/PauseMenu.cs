using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
        isPaused=false;
        gameManager.audioSource.Play();
    }

    public void GoToMainMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScreen");
    }

    public void QuitGame(){
        Application.Quit();
    }
}
