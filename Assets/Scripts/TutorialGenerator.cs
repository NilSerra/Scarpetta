using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialGenerator : MonoBehaviour
{
    public GameManager gameManager;
    private GameObject floor;
    private GameObject ceiling;
    private GameObject floor_2;
    private GameObject ceiling_2;
    public float baseSpeed = 5f;
    public GameObject coinLinePrefab;
    public GameObject wall5BlocksPrefab;
    public GameObject wall6BlocksPrefab;
    public GameObject shieldPrefab;
    public GameObject gunPowerUpPrefab;
    public GameObject arrowUpPrefab;
    public GameObject arrowDownPrefab;
    public float maxXspeed = 20;
    public int speedIncreaseFactor = 60;

    public GameObject[] entityBlock;
    private float entityBlockMinX;    
    public GameObject popupPanel;
    public GameObject popupPanel2;
    public bool hint1shown = false;
    public bool hint2shown = false;
    public bool hint3shown = false;
    public bool hint4shown = false;
    public bool hint5shown = false;
    public bool hint6shown = false;
    public bool hint7shown = false;


    // Start is called before the first frame update
    void Start()
    {
        popupPanel.SetActive(false);
        popupPanel2.SetActive(false);

        floor = GameObject.Find("/TutorialGenerator/Floor");
        ceiling = GameObject.Find("/TutorialGenerator/Ceiling");
        floor_2 = GameObject.Find("/TutorialGenerator/Floor_2");
        ceiling_2 = GameObject.Find("/TutorialGenerator/Ceiling_2");

        entityBlock = new GameObject[9];
        entityBlockMinX = 15;

        generateEntityBlock(entityBlock, entityBlockMinX);

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameOver){

            baseSpeed = Mathf.Min(maxXspeed, baseSpeed+Time.deltaTime/speedIncreaseFactor);
            gameManager.IncScore(baseSpeed*baseSpeed*Time.deltaTime);

            moveBackground(floor);
            moveBackground(floor_2);
            moveBackground(ceiling);
            moveBackground(ceiling_2);

            entityBlockMinX = moveEntityBlock(entityBlock, entityBlockMinX);

            pauseForTutorial(entityBlock);

        }
        else{
            baseSpeed=0;
        }
    }

    private void generateEntityBlock(GameObject[] entityBlock, float entityBlockMinX){
        for(int i=0; i < entityBlock.Length; i++){
            
            // put wall, wall, coinLine, shield, wall, gunPowerUp, wall, arrow up, arrow down
            if(i==0){
                entityBlock[i] = GameObject.Instantiate(wall5BlocksPrefab);
            }
            else if(i==1){
                entityBlock[i] = GameObject.Instantiate(wall6BlocksPrefab);
            }
            else if(i==2){
                entityBlock[i] = GameObject.Instantiate(coinLinePrefab);
            }
            else if(i==3){
                entityBlock[i] = GameObject.Instantiate(shieldPrefab);
            }
            else if(i==4){
                entityBlock[i] = GameObject.Instantiate(wall6BlocksPrefab);
            }
            else if(i==5){
                entityBlock[i] = GameObject.Instantiate(gunPowerUpPrefab);
            }
            else if(i==6){
                entityBlock[i] = GameObject.Instantiate(wall6BlocksPrefab);
            }
            else if(i==7){
                entityBlock[i] = GameObject.Instantiate(arrowUpPrefab);
            }
            else if(i==8){
                entityBlock[i] = GameObject.Instantiate(arrowDownPrefab);
            }

            //Placing the entities in the next entityBlock
            if(i == 0){
                entityBlock[i].transform.position = new Vector3(entityBlockMinX, -2.5f, 0);
            }
            else if(i == 1){
                entityBlock[i].transform.position = new Vector3(entityBlock[i-1].transform.position.x + 6, 2.1f, 0);
            }
            else if(i == 2){
                entityBlock[i].transform.position = new Vector3(entityBlock[i-1].transform.position.x + 18, -2, 0);
            }
            else if(i == 3){
                entityBlock[i].transform.position = new Vector3(entityBlock[i-1].transform.position.x + 18, -3.5f, 0);
            }
            else if(i == 4){
                entityBlock[i].transform.position = new Vector3(entityBlock[i-1].transform.position.x + 18, -2.1f, 0);
            }
            else if(i == 5){
                entityBlock[i].transform.position = new Vector3(entityBlock[i-1].transform.position.x + 18, -3.5f, 0);
            }
            else if(i == 6){
                entityBlock[i].transform.position = new Vector3(entityBlock[i-1].transform.position.x + 18, -2.1f, 0);
            }
            else if(i == 7){
                entityBlock[i].transform.position = new Vector3(entityBlock[i-1].transform.position.x + 16, 0, 0);
            }
            else if(i == 8){
                entityBlock[i].transform.position = new Vector3(entityBlock[i-1].transform.position.x + 16, 0, 0);
            }
        }
    }

    private float moveEntityBlock(GameObject[] entityBlock, float entityBlockMinX){
        for(int i=0; i < entityBlock.Length; i++){
            if(entityBlock[i]){
                entityBlock[i].transform.position -= new Vector3(baseSpeed, 0, 0) * Time.deltaTime;
            }
        }
        return entityBlockMinX - (baseSpeed * Time.deltaTime);
    }

    private void moveBackground(GameObject background_element){
        
        background_element.transform.position -= new Vector3(baseSpeed, 0, 0) * Time.deltaTime;
        if (background_element.transform.position.x <= -20)
        {
            background_element.transform.position = new Vector3(20, background_element.transform.position.y, background_element.transform.position.z);
        }
    }

    private void pauseForTutorial(GameObject[] entityBlock){

        if (entityBlock[0].transform.position.x <= 6 && !hint1shown){
            Time.timeScale = 0;
            popupPanel.SetActive(true);
            if (gameManager.useTouch){
                popupPanel.GetComponentInChildren<Text>().text = "Tap on the right to fly and avoid walls";
            }
            else{
                popupPanel.GetComponentInChildren<Text>().text = "Press space to fly and avoid walls";
            }
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")){
                popupPanel.SetActive(false);
                Time.timeScale = 1;
                hint1shown = true;
            }
        }

        if (entityBlock[2].transform.position.x <= 6 && !hint2shown){
            Time.timeScale = 0;
            popupPanel.SetActive(true);
            popupPanel.GetComponentInChildren<Text>().text = "Collect coins by flying over them";
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")){
                popupPanel.SetActive(false);
                Time.timeScale = 1;
                hint2shown = true;
            }
        }

        if (entityBlock[3].transform.position.x <= 6 && !hint3shown){
            Time.timeScale = 0;
            popupPanel.SetActive(true);
            popupPanel.GetComponentInChildren<Text>().text = "Collect shield by flying over it";
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")){
                popupPanel.SetActive(false);
                Time.timeScale = 1;
                hint3shown = true;
            }
        }

        if (entityBlock[4].transform.position.x <= 6 && !hint4shown){
            Time.timeScale = 0;
            popupPanel.SetActive(true);
            popupPanel.GetComponentInChildren<Text>().text = "With a shield you can fly through a wall";
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")){
                popupPanel.SetActive(false);
                Time.timeScale = 1;
                hint4shown = true;
            }
        }

        if (entityBlock[5].transform.position.x <= 6 && !hint5shown){
            Time.timeScale = 0;
            popupPanel.SetActive(true);
            popupPanel.GetComponentInChildren<Text>().text = "Collect gun to shoot";
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")){
                popupPanel.SetActive(false);
                Time.timeScale = 1;
                hint5shown = true;
            }
        }

        if (entityBlock[6].transform.position.x <= 6 && !hint6shown){
            Time.timeScale = 0;
            popupPanel.SetActive(true);
            if (gameManager.useTouch){
                popupPanel.GetComponentInChildren<Text>().text = "Tap on the left to shoot and destroy walls";
            }
            else{
                popupPanel.GetComponentInChildren<Text>().text = "Press s to shoot and destroy walls";
            }
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.S)){
                popupPanel.SetActive(false);
                Time.timeScale = 1;
                hint6shown = true;
            }
        }

        if (entityBlock[7].transform.position.x <= 6 && !hint7shown){
            Time.timeScale = 0;
            popupPanel.SetActive(true);
            popupPanel.GetComponentInChildren<Text>().text = "Be careful with arrow up and down";
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")){
                popupPanel.SetActive(false);
                Time.timeScale = 1;
                hint7shown = true;
            }
        }

        if (entityBlock[8].transform.position.x <= -14){
            Time.timeScale = 0;
            popupPanel.SetActive(true);
            popupPanel.GetComponentInChildren<Text>().text = "You have completed the tutorial";
            popupPanel2.SetActive(true);
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")){
                popupPanel.SetActive(false);
                popupPanel2.SetActive(false);
                Time.timeScale = 1;
                SceneManager.LoadScene("MenuScreen");
            }
        }
    }
}