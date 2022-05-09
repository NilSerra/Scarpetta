using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Debugging;

public class CharacterPersonalizatorManager : MonoBehaviour
{
    public SpriteRenderer headSprite;
    public List<Sprite> headSprites = new List<Sprite>();
    public List<int> headSpritesCost = new List<int>();
    private int currentOptionHead;

    public SpriteRenderer bodySprite;
    public SpriteRenderer leg1Sprite;
    public SpriteRenderer leg2Sprite;
    public SpriteRenderer hand1Sprite;
    public SpriteRenderer hand2Sprite;
    public SpriteRenderer accessorySprite;
    public SpriteRenderer gunSprite;

    static List<string> titles = new List<string>{"head", "body", "legs", "hands", "accessory", "gun"};
    public GameObject contentList;
    public GameObject listElementPrefab;
    public GameObject titlePrefab;

    public GameObject popupPanel;

    public Text CoinsText;
    public CharacterSkinManager csm;

    private void Start() {
        for(int index = 0; index < titles.Count; ++index){
            
            switch (titles[index]){
                case "head":
                    CreateElementsCharacterPart(titles[index]);
                    break;
                case "body":
                    CreateElementsCharacterPart(titles[index]);
                    break;
                case "legs": 
                    CreateElementsCharacterPart(titles[index]);
                    break;
                case "hands": 
                    CreateElementsCharacterPart(titles[index]);
                    break;
                case "accessory":
                    CreateElementsCharacterPart(titles[index]);
                    break;
                case "gun":
                    CreateElementsCharacterPart(titles[index]);
                    break;
                default:
                    break;
            }
        } 

        popupPanel.SetActive(false);

        CoinsText.text = csm.GetCoinsPlayer().ToString();

        headSprite.sprite = csm.GetSpriteFromBodyPart("head");

        bodySprite.sprite = csm.GetSpriteFromBodyPart("body");
        
        hand1Sprite.sprite = csm.GetSpriteFromBodyPart("hands");
        hand2Sprite.sprite = csm.GetSpriteFromBodyPart("hands");
        
        leg1Sprite.sprite = csm.GetSpriteFromBodyPart("legs");
        leg2Sprite.sprite = csm.GetSpriteFromBodyPart("legs");
        
        accessorySprite.sprite = csm.GetSpriteFromBodyPart("accessory");
        
        gunSprite.sprite = csm.GetSpriteFromBodyPart("gun");
    }

    private void CreateElementsCharacterPart(string title){
        Debugging.DebugLog("Creating Character Elements for part: " + title);
        GameObject textTest = GameObject.Instantiate(titlePrefab);
        textTest.GetComponent<UnityEngine.UI.Text>().text = title;
        textTest.transform.SetParent(contentList.transform, false);

        int count = csm.GetSpriteListCount(title);

        for(int i = 0; i < count; ++i){
            GameObject element = GameObject.Instantiate(listElementPrefab);
            GameObject imageObject = element.transform.Find("ImageItem").gameObject;
            GameObject textObject = element.transform.Find("ItemPrice").gameObject;
            GameObject buttonObject = element.transform.Find("BackButton").gameObject;

            imageObject.GetComponent<Image>().sprite = csm.GetSpriteItem(title, i);
            if (imageObject.GetComponent<Image>().sprite == null){
                imageObject.GetComponent<Image>().color = new Color32(255,255,225,0);
            }
            int price = i < count ? csm.GetItemCost(title, i) : 0;
            textObject.GetComponent<Text>().text = price.ToString();
            Button button = buttonObject.GetComponent<Button>();

            GameObject buttonText = buttonObject.transform.Find("Text").gameObject;
            buttonText.GetComponent<Text>().text = (price == 0 || csm.PlayerOwnsItem(csm.GetPlayerPrefNameForItem(title, i))) ? "set" : "buy";

            int tempIndex = i;
            button.onClick.AddListener(() => csm.ChangeEquipment(title, tempIndex, buttonText));

            element.transform.SetParent(contentList.transform, false);
        }
        Debugging.DebugLog("Objects for part: " + title + " loaded successfully");
    }

    public void GoBack(){
        Debugging.DebugLog("Changing scene to Menu");
        SceneManager.LoadScene("MenuScreen");
    }

    public void TestFunctionAddCoins(){
        PlayerPrefs.SetInt("TotalCoins", csm.GetCoinsPlayer() + 100);
        CoinsText.text = csm.GetCoinsPlayer().ToString();
    }

    public void okButton(){
        popupPanel.SetActive(false);  
    }
}
