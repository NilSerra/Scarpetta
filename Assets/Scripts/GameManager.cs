using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   
    //Game Over
    public Text gameOverText;
    public bool gameOver = false;
    //Score
    public float score;
    public int coins;
    public int ammo;
    public Text scoreText;
    public Text coinsText;
    public Text ammoText;

    public bool useTouch;

    void Start()
    {
        score = 0;
        coins = 0;
        ammo = 0;
        gameOverText.gameObject.SetActive(false);
        
        if(SystemInfo.deviceType == DeviceType.Desktop){
            useTouch = false;
        }
         else if(SystemInfo.deviceType == DeviceType.Handheld){
            useTouch = true;
        }
    }

    void FixedUpdate()
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
        gameOverText.gameObject.SetActive(true);
    }
}
