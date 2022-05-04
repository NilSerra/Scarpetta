using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSkinManager : MonoBehaviour
{
    
    public SpriteRenderer headSprite;
    public SpriteRenderer bodySprite;
    public SpriteRenderer leg1Sprite;
    public SpriteRenderer leg2Sprite;
    public SpriteRenderer hand1Sprite;
    public SpriteRenderer hand2Sprite;
    public SpriteRenderer accessorySprite;
    public SpriteRenderer gunSprite;

    public List<Sprite> headSprites = new List<Sprite>();
    public List<Sprite> bodySprites = new List<Sprite>();
    public List<Sprite> legSprites = new List<Sprite>();    
    public List<Sprite> handSprites = new List<Sprite>();    
    public List<Sprite> accessorySprites = new List<Sprite>();    
    public List<Sprite> gunSprites = new List<Sprite>();    

    public List<int> headSpritesCost = new List<int>();
    public List<int> bodySpritesCost = new List<int>();
    public List<int> legSpritesCost = new List<int>();
    public List<int> handSpritesCost = new List<int>();
    public List<int> accessorySpritesCost = new List<int>();
    public List<int> gunSpritesCost = new List<int>();

    public static List<string> titles = new List<string>{"head", "body", "legs", "hands", "accessory", "gun"};

    public Text CoinsTextCustomizationScreen;
    public GameObject popupPanel;

    public AudioClip buySound;


    public int GetItemCost(string bodyPart, int index){
        if (titles.Contains(bodyPart)){
            switch (bodyPart){
                case "head":
                    return headSpritesCost[index];
                case "body":
                    return bodySpritesCost[index];
                case "legs":
                    return legSpritesCost[index];
                case "hands": 
                    return handSpritesCost[index];
                case "accessory":
                    return accessorySpritesCost[index];
                case "gun":
                    return gunSpritesCost[index];
                default:
                    break;
            }
        }
        Debug.Log("WHEN CALLING GetItemCost(): Error in body part name");
        return 0;
    }

    public Sprite GetSpriteItem(string bodyPart, int index){
        if (titles.Contains(bodyPart)){
            switch (bodyPart){
                case "head":
                    return headSprites[index];
                case "body":
                    return bodySprites[index];
                case "legs":
                    return legSprites[index];
                case "hands": 
                    return handSprites[index];
                case "accessory":
                    return accessorySprites[index];
                case "gun":
                    return gunSprites[index];
                default:
                    break;
            }
        }
        Debug.Log("WHEN CALLING GetSpriteItem(): Error in body part name");
        return null;
    }

    public int GetSpriteListCount(string bodyPart){
        if (titles.Contains(bodyPart)){
            switch (bodyPart){
                case "head":
                    return headSprites.Count;
                case "body":
                    return bodySprites.Count;
                case "legs":
                    return legSprites.Count;
                case "hands": 
                    return handSprites.Count;
                case "accessory":
                    return accessorySprites.Count;
                case "gun":
                    return gunSprites.Count;
                default:
                    break;
            }
        }
        Debug.Log("WHEN CALLING GetSpriteListCount(): Error in body part name");
        return 0;
    }

    public void ChangeEquipment(string title, int elementIndex, GameObject buttonText){
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
            SetPlayerEquipment(bodyPart, elementIndex);
        }
        else if (PlayerHasMoney(costList[elementIndex])){
            PlayerBoughtItem(GetPlayerPrefNameForItem(bodyPart, elementIndex), costList[elementIndex]);
            sprite.sprite = spriteList[elementIndex];
            if (sprite2 != null){
                sprite2.sprite = spriteList[elementIndex];
            }
            SetPlayerEquipment(bodyPart, elementIndex);

            buttonText.GetComponent<Text>().text = "set";
            Debug.Log("Item bought for " + costList[elementIndex] + " coins.");
        }
        else{
            StartCoroutine(ActivatePopupMenu());
            Debug.Log("Not enough coins.");
        }
    }

    public string GetPlayerPrefNameForItem(string bodyPart, int index){

        if (titles.Contains(bodyPart)){
            return bodyPart+"_"+index;
        }
        Debug.Log("WHEN CALLING GetPlayerPrefNameForItem(): Error in body part name");
        return "Error";
    }

    public string GetPlayerEquipmentPrefName(string bodyPart){
        if (titles.Contains(bodyPart)){
            return bodyPart+"_equiped";
        }
        Debug.Log("WHEN CALLING GetPlayerPrefNameForItem(): Error in body part name");
        return "Error";
    }

    public void SetPlayerEquipment(string bodyPart, int index){
        PlayerPrefs.SetInt(GetPlayerEquipmentPrefName(bodyPart), index);
    }

    public int GetPlayerEquipmentIndex(string bodyPart){
        int index = PlayerPrefs.GetInt(GetPlayerEquipmentPrefName(bodyPart), 0);
        return index;
    }

    public Sprite GetSpriteFromBodyPart(string bodyPart){
        if (titles.Contains(bodyPart)){
            int index = GetPlayerEquipmentIndex(bodyPart);
            switch (bodyPart){
                case "head":
                    return headSprites[index];
                case "body":
                    return bodySprites[index];
                case "legs":
                    return legSprites[index];
                case "hands": 
                    return handSprites[index];
                case "accessory":
                    return accessorySprites[index];
                case "gun":
                    return gunSprites[index];
                default:
                    break;
            }
        }
        Debug.Log("WHEN CALLING GetSpriteFromBodyPart(): Error in body part name");
        return null;
    }

    public bool PlayerOwnsItem(string preferencesText){
        return 1 == PlayerPrefs.GetInt(preferencesText, 0);
    }

    private void PlayerBoughtItem(string preferencesText, int cost){
        PlayerPrefs.SetInt(preferencesText, 1);
        SetCoinsPlayer(GetCoinsPlayer() - cost);
        CoinsTextCustomizationScreen.text = GetCoinsPlayer().ToString();
        AudioSource.PlayClipAtPoint(buySound, Camera.main.transform.position);
    }

    public bool PlayerHasMoney(int costItem){
        return GetCoinsPlayer() >= costItem;
    }
    public int GetCoinsPlayer(){
        return PlayerPrefs.GetInt("TotalCoins", 0);
    }

    public void SetCoinsPlayer(int coins){
        PlayerPrefs.SetInt("TotalCoins", coins);
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


}
