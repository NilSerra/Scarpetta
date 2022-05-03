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
    public float maxXspeed = 20;
    public int speedIncreaseFactor = 60;

    // Tutoriale entities
    public float entityWallX;
    public float entityWallY;
    public GameObject wall;
    public GameObject wall2;
    public float entityCoinLineX;
    public float entityCoinLineY;
    public GameObject coinLine;

    public GameObject popupPanel;
    public GameObject popupPanel2;
    public bool hint1shown = false;
    public bool hint2shown = false;


    // Start is called before the first frame update
    void Start()
    {
        popupPanel.SetActive(false);
        popupPanel2.SetActive(false);

        floor = GameObject.Find("/TutorialGenerator/Floor");
        ceiling = GameObject.Find("/TutorialGenerator/Ceiling");
        floor_2 = GameObject.Find("/TutorialGenerator/Floor_2");
        ceiling_2 = GameObject.Find("/TutorialGenerator/Ceiling_2");

        generateTutorialeEntities();

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameOver){

            baseSpeed = Mathf.Min(maxXspeed, baseSpeed+Time.deltaTime/speedIncreaseFactor);
            gameManager.incScore(baseSpeed*baseSpeed*Time.deltaTime);

            moveBackground(floor);
            moveBackground(floor_2);
            moveBackground(ceiling);
            moveBackground(ceiling_2);

            moveTutorialEntities();

            pauseForTutorial();
            // if (wall.GetComponent<SpriteRenderer>().isVisible) {
            //     Debug.Log("Wall is visible");
            // }

        }
        else{
            baseSpeed=0;
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")){
                SceneManager.LoadScene("HelpSystem");
                baseSpeed = 5;
            }
        }
    }

    private void moveTutorialEntities(){
        wall.transform.Translate(new Vector3(-baseSpeed*Time.deltaTime, 0, 0));
        wall2.transform.Translate(new Vector3(-baseSpeed*Time.deltaTime, 0, 0));
        coinLine.transform.Translate(new Vector3(-baseSpeed*Time.deltaTime, 0, 0));   
    }

    private void generateTutorialeEntities(){ 
        entityWallX = 15;
        entityWallY = -2.5f;
        wall = GameObject.Instantiate(wall5BlocksPrefab);
        wall.transform.position = new Vector3(entityWallX, entityWallY, 5);

        entityWallX += 7;
        entityWallY = 2.1f;
        wall2 = GameObject.Instantiate(wall6BlocksPrefab);
        wall2.transform.position = new Vector3(entityWallX, entityWallY, 5);

        entityCoinLineX = 40;
        entityCoinLineY = -2;
        coinLine = GameObject.Instantiate(coinLinePrefab);
        coinLine.transform.position = new Vector3(entityCoinLineX, entityCoinLineY, 5);

    }

    private void destroyEntityBlock(GameObject[] entityBlock){
        for(int i=0; i < entityBlock.Length; i++){
            Destroy(entityBlock[i]);
        }
    }

    private void moveBackground(GameObject background_element){
        
        background_element.transform.position -= new Vector3(baseSpeed, 0, 0) * Time.deltaTime;
        if (background_element.transform.position.x <= -20)
        {
            background_element.transform.position = new Vector3(20, background_element.transform.position.y, background_element.transform.position.z);
        }
    }

    private void pauseForTutorial(){

        if (wall.transform.position.x <= 6 && !hint1shown){
            Time.timeScale = 0;
            popupPanel.SetActive(true);
            popupPanel.GetComponentInChildren<Text>().text = "Press space to fly and avoid walls";

            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")){
                popupPanel.SetActive(false);
                Time.timeScale = 1;
                hint1shown = true;
            }
        } 
        if (coinLine.transform.position.x <= 6 && !hint2shown){
            Time.timeScale = 0;
            popupPanel.SetActive(true);
            popupPanel.GetComponentInChildren<Text>().text = "Collect coins by flying over them";

            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")){
                popupPanel.SetActive(false);
                Time.timeScale = 1;
                hint2shown = true;
            }
        }
        if (coinLine.transform.position.x <= -18) {
            Time.timeScale = 0;
            popupPanel.SetActive(true);
            popupPanel.GetComponentInChildren<Text>().text = "You have completed the tutorial";
            popupPanel2.SetActive(true);
            

            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")){
                Time.timeScale = 1;
                SceneManager.LoadScene("MenuScreen");
            }
            
        }
    }
}