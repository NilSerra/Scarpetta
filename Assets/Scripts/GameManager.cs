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

    void Start()
    {
        score = 0;
        coins = 0;
        ammo = 0;
        gameOverText.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if(!gameOver && SceneManager.GetActiveScene().name == "MainScene"){
            scoreText.text = "Score: " + (int) score + " m";
            coinsText.text = "Coins:  " + coins;
            ammoText.text = "Bullets:  " + ammo;
        }
        // do the same for the help system scene
        if(!gameOver && SceneManager.GetActiveScene().name == "HelpSystem"){
            scoreText.text = "Score: " + (int) score + " m";
            coinsText.text = "Coins:  " + coins;
            ammoText.text = "Bullets:  " + ammo;
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
