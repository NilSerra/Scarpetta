using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterPersonalizatorManager : MonoBehaviour
{
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

    public Text CoinsText;

    private void Start() {

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
        headSprite.sprite = headSprites[GetPlayerEquipmentForItem("head")];
        bodySprite.sprite = bodySprites[GetPlayerEquipmentForItem("body")];
        leg1Sprite.sprite = legSprites[GetPlayerEquipmentForItem("legs")];
        leg2Sprite.sprite = legSprites[GetPlayerEquipmentForItem("legs")];
        hand1Sprite.sprite = handSprites[GetPlayerEquipmentForItem("hands")];
        hand2Sprite.sprite = handSprites[GetPlayerEquipmentForItem("hands")];
        accessorySprite.sprite = accessorySprites[GetPlayerEquipmentForItem("accessory")];
        gunSprite.sprite = gunSprites[GetPlayerEquipmentForItem("gun")];


        
    }

    private void CreateElementscharacterPart(string title, List<Sprite> spriteList, List<int> spriteCost){
        GameObject texttest = GameObject.Instantiate(titlePrefab);
        texttest.GetComponent<UnityEngine.UI.Text>().text = title;
        texttest.transform.SetParent(contentList.transform, false);

        for(int i = 0; i < spriteList.Count; ++i){
            GameObject element = GameObject.Instantiate(listElementPrefab);
            GameObject imageObject = element.transform.GetChild(0).gameObject;
            GameObject textObject = element.transform.GetChild(1).gameObject;
            GameObject buttonObject = element.transform.GetChild(2).gameObject;

            imageObject.GetComponent<Image>().sprite = spriteList[i];
            if (imageObject.GetComponent<Image>().sprite == null){
                imageObject.GetComponent<Image>().color = new Color32(255,255,225,0);
            }
            int price = i < spriteList.Count ? spriteCost[i] : 0;
            textObject.GetComponent<Text>().text = price + " coins";
            Button button = buttonObject.GetComponent<Button>();
            int tempIndex = i; //needs to be like this
            button.onClick.AddListener(() => ChangeEquipment(title, tempIndex));

            element.transform.SetParent(contentList.transform, false);
        }

    }

    private void ChangeEquipment(string title, int elementIndex){
        Debug.Log(title + " pressed, item " + elementIndex);
        switch (title){
                case "head":
                    if (PlayerOwnsItem(GetPlayerPrefNameForItem("head", elementIndex))){
                        headSprite.sprite = headSprites[elementIndex];
                        SetPlayerEquipmentForItem("head", elementIndex);

                    }
                    else if (PlayerHasMoney(headSpritesCost[elementIndex])){
                        PlayerBoughtItem(GetPlayerPrefNameForItem("head", elementIndex), headSpritesCost[elementIndex]);
                        headSprite.sprite = headSprites[elementIndex];
                        SetPlayerEquipmentForItem("head", elementIndex);

                        Debug.Log("Item bought for " + headSpritesCost[elementIndex] + " coins.");
                    }
                    else{
                        Debug.Log("Not enough coins.");
                    }
                    break;
                case "body":
                    if (PlayerOwnsItem(GetPlayerPrefNameForItem("body", elementIndex))){
                        bodySprite.sprite = bodySprites[elementIndex];
                        SetPlayerEquipmentForItem("body", elementIndex);

                    }
                    else if (PlayerHasMoney(bodySpritesCost[elementIndex])){
                        PlayerBoughtItem(GetPlayerPrefNameForItem("body", elementIndex), bodySpritesCost[elementIndex]);
                        bodySprite.sprite = bodySprites[elementIndex];
                        SetPlayerEquipmentForItem("body", elementIndex);

                        Debug.Log("Item bought for " + bodySpritesCost[elementIndex] + " coins.");
                    }
                    else{
                        Debug.Log("Not enough coins.");
                    }
                    break;
                case "legs":
                    if (PlayerOwnsItem(GetPlayerPrefNameForItem("legs", elementIndex))){
                        leg1Sprite.sprite = legSprites[elementIndex];
                        leg2Sprite.sprite = legSprites[elementIndex];
                        SetPlayerEquipmentForItem("legs", elementIndex);

                    }
                    else if (PlayerHasMoney(legSpritesCost[elementIndex])){
                        PlayerBoughtItem(GetPlayerPrefNameForItem("legs", elementIndex), legSpritesCost[elementIndex]);
                        leg1Sprite.sprite = legSprites[elementIndex];
                        leg2Sprite.sprite = legSprites[elementIndex];
                        SetPlayerEquipmentForItem("legs", elementIndex);

                        Debug.Log("Item bought for " + legSpritesCost[elementIndex] + " coins.");
                    }
                    else{
                        Debug.Log("Not enough coins.");
                    }
                    break;
                case "hands": 
                    if (PlayerOwnsItem(GetPlayerPrefNameForItem("hands", elementIndex))){
                        hand1Sprite.sprite = handSprites[elementIndex];
                        hand2Sprite.sprite = handSprites[elementIndex];
                        SetPlayerEquipmentForItem("hands", elementIndex);

                    }
                    else if (PlayerHasMoney(handSpritesCost[elementIndex])){
                        PlayerBoughtItem(GetPlayerPrefNameForItem("hands", elementIndex), handSpritesCost[elementIndex]);
                        hand1Sprite.sprite = handSprites[elementIndex];
                        hand2Sprite.sprite = handSprites[elementIndex];
                        SetPlayerEquipmentForItem("hands", elementIndex);

                        Debug.Log("Item bought for " + handSpritesCost[elementIndex] + " coins.");
                    }
                    else{
                        Debug.Log("Not enough coins.");
                    }
                    break;
                case "accessory":
                    if (PlayerOwnsItem(GetPlayerPrefNameForItem("accessory", elementIndex))){
                        accessorySprite.sprite = accessorySprites[elementIndex];
                        SetPlayerEquipmentForItem("accessory", elementIndex);

                    }
                    else if (PlayerHasMoney(accessorySpritesCost[elementIndex])){
                        PlayerBoughtItem(GetPlayerPrefNameForItem("accessory", elementIndex), accessorySpritesCost[elementIndex]);
                        accessorySprite.sprite = accessorySprites[elementIndex];
                        SetPlayerEquipmentForItem("accessory", elementIndex);

                        Debug.Log("Item bought for " + accessorySpritesCost[elementIndex] + " coins.");
                    }
                    else{
                        Debug.Log("Not enough coins.");
                    }
                    break;
                case "gun":
                    if (PlayerOwnsItem(GetPlayerPrefNameForItem("gun", elementIndex))){
                        gunSprite.sprite = gunSprites[elementIndex];
                        SetPlayerEquipmentForItem("gun", elementIndex);
                    }
                    else if (PlayerHasMoney(gunSpritesCost[elementIndex])){
                        PlayerBoughtItem(GetPlayerPrefNameForItem("gun", elementIndex), gunSpritesCost[elementIndex]);
                        gunSprite.sprite = gunSprites[elementIndex];
                        SetPlayerEquipmentForItem("gun", elementIndex);
                        Debug.Log("Item bought for " + gunSpritesCost[elementIndex] + " coins.");
                    }
                    else{
                        Debug.Log("Not enough coins.");
                    }
                    break;
                default:
                    break;
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

    private void SetPlayerEquipmentForItem(string bodyPart, int index){
        PlayerPrefs.SetInt(GetPlayerEquipmentPrefName(bodyPart), index);
    }

    private int GetPlayerEquipmentForItem(string bodyPart){
        Debug.Log(bodyPart + "->" + PlayerPrefs.GetInt(GetPlayerEquipmentPrefName(bodyPart), 0));
        return PlayerPrefs.GetInt(GetPlayerEquipmentPrefName(bodyPart), 0);
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
        SceneManager.LoadScene("MenuScreen");
    }

    public void TestFunctionAddCoins(){
        PlayerPrefs.SetInt("TotalCoins", GetCoinsPlayer() + 100);
        CoinsText.text = GetCoinsPlayer().ToString();
    }
}
