using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public bool isPaused;
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame(){
        if (MapGenerator.scoreNumber > PlayerPrefs.GetInt("HighestScore", 0))
                PlayerPrefs.SetInt("HighestScore", (int)MapGenerator.scoreNumber);

        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
        isPaused=true;
    }

    public void ResumeGame(){
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
        isPaused=false;
    }

    public void GoToMainMenu(){
         Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScreen");
    }

    public void QuitGame(){
        Application.Quit();
    }
}
