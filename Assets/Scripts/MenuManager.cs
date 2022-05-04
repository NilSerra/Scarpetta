using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Text highestScoreText;
    public Text totalCoinsText;

    

    public GameObject helpMessagePanel;
    void Start() {
        float highestScore = PlayerPrefs.GetInt("HighestScore", 0);
        float totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        highestScoreText.text = "HIGHEST SCORE " + highestScore+" m";
        totalCoinsText.text = "Total Coins: " + totalCoins;
        ShowHelpMessage();
    }

    private void ShowHelpMessage(){
        if(SceneManager.GetActiveScene().name == "MenuScreen" && PlayerPrefs.GetInt("FirstTimeOpening", 1)==1){
            helpMessagePanel.SetActive(true);
            PlayerPrefs.SetInt("FirstTimeOpening", 0);
        }
    }
    public void PlayGame(){
        SceneManager.LoadScene("MainScene");
    }

    public void GoToOptions(){
        SceneManager.LoadScene("CharacterPersonalizationScreen");
    }

    public void PlayTutorial(){
        SceneManager.LoadScene("HelpSystem");
    }


    public void QuitGame(){
        Application.Quit();
    }
}
