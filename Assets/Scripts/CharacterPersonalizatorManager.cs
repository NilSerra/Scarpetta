using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterPersonalizatorManager : MonoBehaviour
{
    [Header("Player in game. Dragged from the screen")]
    public GameObject characterInGame;
    
    [Header("Prefab of the player. Dragged from the prefabs folder.")]
    public GameObject mainPlayerPrefab;

    private string mainPlayerPrefabPath;
    
    public SpriteRenderer headSprite;
    public List<Sprite> headSprites = new List<Sprite>();
    public List<int> headSpritesCost = new List<int>();
    private int currentOptionHead;


    public SpriteRenderer bodySprite;
    public List<Sprite> bodySprites = new List<Sprite>();
    public List<int> bodySpritesCost = new List<int>();
    private int currentOptionBody;


    public SpriteRenderer leg1Sprite;
    public SpriteRenderer leg2Sprite;
    public List<Sprite> legSprites = new List<Sprite>();    
    public List<int> legSpritesCost = new List<int>();
    private int currentOptionLegs;


    public SpriteRenderer hand1Sprite;
    public SpriteRenderer hand2Sprite;
    public List<Sprite> handSprites = new List<Sprite>();    
    public List<int> handSpritesCost = new List<int>();
    private int currentOptionHand;


    public SpriteRenderer accessorySprite;
    public List<Sprite> accessorySprites = new List<Sprite>();    
    public List<int> accessorySpritesCost = new List<int>();
    private int currentOptionAccessory;


    public SpriteRenderer gunSprite;
    public List<Sprite> gunSprites = new List<Sprite>();    
    public List<int> gunSpritesCost = new List<int>();
    private int currentOptionGun;

    static List<string> titles = new List<string>{"head", "body", "legs", "hands", "accessory", "gun"};
    public GameObject contentList;
    public GameObject listElementPrefab;
    public GameObject titlePrefab;

    public GameObject popupPanel;

    public Text CoinsText;

    private void Start() {

        popupPanel.SetActive(false);

        CoinsText.text = GetCoinsPlayer().ToString();

        for(int index = 0; index < titles.Count; ++index){
            
            switch (titles[index]){
                case "head":
                    CreateElementscharacterPart(titles[index], headSprites, headSpritesCost);
                    
                    break;
                case "body":
                    CreateElementscharacterPart(titles[index], bodySprites, bodySpritesCost);
                    break;
                case "legs": 
                    CreateElementscharacterPart(titles[index], legSprites, legSpritesCost);
                    
                    break;
                case "hands": 
                    CreateElementscharacterPart(titles[index], handSprites, handSpritesCost);
                   
                    break;
                case "accessory":
                    CreateElementscharacterPart(titles[index], accessorySprites, accessorySpritesCost);
                    
                    break;
                case "gun":
                    CreateElementscharacterPart(titles[index], gunSprites, gunSpritesCost);
                    
                    break;
                default:
                    break;
            }
        } 

        mainPlayerPrefabPath = AssetDatabase.GetAssetPath( mainPlayerPrefab );       
    }

    private void CreateElementscharacterPart(string title, List<Sprite> spriteList, List<int> spriteCost){
        GameObject texttest = GameObject.Instantiate(titlePrefab);
        texttest.GetComponent<UnityEngine.UI.Text>().text = title;
        texttest.transform.SetParent(contentList.transform, false);

        for(int i = 0; i < spriteList.Count; ++i){
            GameObject element = GameObject.Instantiate(listElementPrefab);
            GameObject imageObject = element.transform.Find("ImageItem").gameObject;
            GameObject textObject = element.transform.Find("ItemPrice").gameObject;
            GameObject buttonObject = element.transform.Find("BackButton").gameObject;

            imageObject.GetComponent<Image>().sprite = spriteList[i];
            if (imageObject.GetComponent<Image>().sprite == null){
                imageObject.GetComponent<Image>().color = new Color32(255,255,225,0);
            }
            int price = i < spriteList.Count ? spriteCost[i] : 0;
            textObject.GetComponent<Text>().text = price.ToString();
            Button button = buttonObject.GetComponent<Button>();

            GameObject buttonText = buttonObject.transform.Find("Text").gameObject;
            buttonText.GetComponent<Text>().text = PlayerOwnsItem(GetPlayerPrefNameForItem(title, i)) ? "set" : "buy";

            int tempIndex = i; //needs to be like this
            button.onClick.AddListener(() => ChangeEquipment(title, tempIndex, buttonText));

            element.transform.SetParent(contentList.transform, false);
        }

    }

    private void ChangeEquipment(string title, int elementIndex, GameObject buttonText){
        Debug.Log(title + " pressed, item " + elementIndex);
        switch (title){
                case "head":
                    ActionButtonPressed("head", elementIndex, headSprite, null, headSprites, headSpritesCost, buttonText);
                    break;
                case "body":
                    ActionButtonPressed("body", elementIndex, bodySprite, null, bodySprites, bodySpritesCost, buttonText);
                    break;
                case "legs":
                    ActionButtonPressed("legs", elementIndex, leg1Sprite, leg2Sprite, legSprites, legSpritesCost, buttonText);
                    break;
                case "hands": 
                    ActionButtonPressed("hands", elementIndex, hand1Sprite, hand2Sprite, handSprites, handSpritesCost, buttonText);
                    break;
                case "accessory":
                    ActionButtonPressed("accessory", elementIndex, accessorySprite, null, accessorySprites, accessorySpritesCost, buttonText);
                    break;
                case "gun":
                    ActionButtonPressed("gun", elementIndex, gunSprite, null, gunSprites, gunSpritesCost, buttonText);
                    break;
                default:
                    break;
            }
            
    }

    private void ActionButtonPressed(string bodyPart, int elementIndex, SpriteRenderer sprite, SpriteRenderer sprite2, List<Sprite> spriteList, List<int> costList, GameObject buttonText){
        if (PlayerOwnsItem(GetPlayerPrefNameForItem(bodyPart, elementIndex))){
            sprite.sprite = spriteList[elementIndex];
            if (sprite2 != null){
                sprite2.sprite = spriteList[elementIndex];
            }
        }
        else if (PlayerHasMoney(costList[elementIndex])){
            PlayerBoughtItem(GetPlayerPrefNameForItem(bodyPart, elementIndex), costList[elementIndex]);
            sprite.sprite = spriteList[elementIndex];
            if (sprite2 != null){
                sprite2.sprite = spriteList[elementIndex];
            }
            buttonText.GetComponent<Text>().text = "set";
            Debug.Log("Item bought for " + costList[elementIndex] + " coins.");
        }
        else{
            StartCoroutine(ActivatePopupMenu());
            Debug.Log("Not enough coins.");
        }
    }

    public static string GetPlayerPrefNameForItem(string bodyPart, int index){

        if (titles.Contains(bodyPart)){
            return bodyPart+"_"+index;
        }
        Debug.Log("WHEN CALLING GetPlayerPrefNameForItem(): Error in body part name");
        return "Error";
    }

    public static string GetPlayerEquipmentPrefName(string bodyPart){
        if (titles.Contains(bodyPart)){
            return bodyPart+"_equiped";
        }
        Debug.Log("WHEN CALLING GetPlayerPrefNameForItem(): Error in body part name");
        return "Error";
    }

    private bool PlayerOwnsItem(string preferencesText){
        return 1 == PlayerPrefs.GetInt(preferencesText, 0);
    }

    private void PlayerBoughtItem(string preferencesText, int cost){
        PlayerPrefs.SetInt(preferencesText, 1);
        SetCoinsPlayer(GetCoinsPlayer() - cost);
        CoinsText.text = GetCoinsPlayer().ToString();
    }

    private bool PlayerHasMoney(int costItem){
        return GetCoinsPlayer() >= costItem;
    }
    private int GetCoinsPlayer(){
        return PlayerPrefs.GetInt("TotalCoins", 0);
    }

    private void SetCoinsPlayer(int coins){
        PlayerPrefs.SetInt("TotalCoins", coins);
    }
    public void GoBack(){
        PrefabUtility.SaveAsPrefabAsset(characterInGame, mainPlayerPrefabPath);
        SceneManager.LoadScene("MenuScreen");
    }

    public void TestFunctionAddCoins(){
        PlayerPrefs.SetInt("TotalCoins", GetCoinsPlayer() + 100);
        CoinsText.text = GetCoinsPlayer().ToString();
    }

    IEnumerator ActivatePopupMenu()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        popupPanel.SetActive(true);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        popupPanel.SetActive(false);
    }

    public void okButton(){
        popupPanel.SetActive(false);
    }

}
