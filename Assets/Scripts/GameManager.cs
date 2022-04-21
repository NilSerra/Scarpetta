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
    public Text scoreText;
    public Text coinsText;

    //Coins


    void Start()
    {
        score = 0;
        coins = 0;
        gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if(!gameOver){
            scoreText.text = "Score: " + (int) score + " m";
            coinsText.text = "Coins:  " + coins;
        }
    }

    public void incScore(float currentScore){
        score += currentScore;
    }

    public void incCoins(){
        coins++;
    }

    public void EndGame() {
        if (score > PlayerPrefs.GetInt("HighestScore", 0))
            PlayerPrefs.SetInt("HighestScore", (int) score);
            PlayerPrefs.SetInt("TotalCoins", coins + PlayerPrefs.GetInt("TotalCoins"));

        gameOver=true;
        gameOverText.gameObject.SetActive(true);
    }
}
