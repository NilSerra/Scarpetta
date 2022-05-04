using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   
    //Game Over
    public GameObject pauseButton;
    public GameObject gameOverMenu;
    public bool gameOver = false;
    //Score
    public float score;
    public int coins;
    public int ammo;
    public Text scoreText;
    public Text coinsText;
    public Text ammoText;
    public AudioSource audioSource;

    public bool useTouch;

    void Start()
    {
        score = 0;
        coins = 0;
        ammo = 0;
        gameOverMenu.SetActive(false);
        audioSource = this.GetComponent<AudioSource>();
        
        if(SystemInfo.deviceType == DeviceType.Desktop){
            useTouch = false;
        }
         else if(SystemInfo.deviceType == DeviceType.Handheld){
            useTouch = true;
        }
    }

    void Update()
    {
        if(!gameOver && (SceneManager.GetActiveScene().name == "MainScene" || SceneManager.GetActiveScene().name == "HelpSystem")){
            scoreText.text = ((int) score).ToString();
            coinsText.text = coins.ToString();
            ammoText.text = ammo.ToString();
        }
    }

    public void IncScore(float currentScore){
        score += currentScore;
    }

    public void IncCoins(){
        coins++;
    }

    public void SetAmmo(int currentAmmo){
        ammo = currentAmmo;
    }
    public void EndGame() {
        if (score > PlayerPrefs.GetInt("HighestScore", 0))
            PlayerPrefs.SetInt("HighestScore", (int) score);
            PlayerPrefs.SetInt("TotalCoins", coins + PlayerPrefs.GetInt("TotalCoins"));

        gameOver=true;
        gameOverMenu.SetActive(true);
        pauseButton.SetActive(false);
        audioSource.Stop();
    }

    public void GoToMainMenu(){
        SceneManager.LoadScene("MenuScreen");
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void PlayGame(){
        SceneManager.LoadScene("MainScene");
    }

    public void PlayTutorial(){
        SceneManager.LoadScene("HelpSystem");
    }
}
