using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Text highestScoreText;
    void Start() {
        float highestScore = PlayerPrefs.GetInt("HighestScore", 0);
        highestScoreText.text = "HIGHEST SCORE "+highestScore+" m";
    }
    public void PlayGame(){
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame(){
        Application.Quit();
    }
}
