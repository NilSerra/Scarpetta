using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Debugging;

public class MenuManager : MonoBehaviour
{
    public Text highestScoreText;
    public Text totalCoinsText;
    public GameObject helpMessagePanel;

    void Start() {
        Application.targetFrameRate = 60;
        float highestScore = PlayerPrefs.GetInt("HighestScore", 0);
        float totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        highestScoreText.text = "HIGHEST SCORE " + highestScore+" m";
        totalCoinsText.text = "Total Coins: " + totalCoins;
        ShowHelpMessage();
    }

    private void ShowHelpMessage(){
        if(SceneManager.GetActiveScene().name == "MenuScreen" && PlayerPrefs.GetInt("FirstTimeOpening", 1)==1){
            Debugging.DebugLog("Showing tutorial message");
            helpMessagePanel.SetActive(true);
        }
        else{
            helpMessagePanel.SetActive(false);
        }
    }
    public void PlayGame(){
        Debugging.DebugLog("Changing scene to MainScene");
        SceneManager.LoadScene("MainScene");
    }

    public void GoToShop(){
        Debugging.DebugLog("Changing scene to CharacterPersonalization");
        SceneManager.LoadScene("CharacterPersonalizationScreen");
    }

    public void PlayTutorial(){
        Debugging.DebugLog("Changing scene to Tutorial");
        PlayerPrefs.SetInt("FirstTimeOpening", 0);
        SceneManager.LoadScene("HelpSystem");
    }

    public void QuitGame(){
        Debugging.DebugLog("Quitting game.");
        Application.Quit();
    }
}
