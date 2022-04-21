using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    public GameManager gameManager;
    private GameObject floor;
    private GameObject ceiling;
    private GameObject floor_2;
    private GameObject ceiling_2;

    public float baseSpeed = 5f;

    private GameObject obstacle0;
    private GameObject obstacle1;
    private GameObject obstacle2;
    private GameObject obstacle3;
    private GameObject obstacle4;
    private GameObject coin;
    private GameObject[] entityBlock1;
    private GameObject[] entityBlock2;
    private float entityBlock1MinX;
    private float entityBlock2MinX;
    private float entityBlockSeparation;

    public GameObject obstaclePrefab;
    public GameObject coinPrefab;

    private float minXseparation = 5;
    private float maxXseparation = 7;

    public float minYpositionObstacle = -5;
    public float maxYpositionObstacle = 5;

    public float minYpositionCoin = -4;
    public float maxYpositionCoin = 4;

    public float minYscaleObstacle = 2;
    public float maxYscaleObstacle = 6;

    public float maxXspeed = 20;

    public int speedIncreaseFactor = 60;

    // Start is called before the first frame update
    void Start()
    {
        floor = GameObject.Find("/MapGenerator/Floor");
        ceiling = GameObject.Find("/MapGenerator/Ceiling");
        floor_2 = GameObject.Find("/MapGenerator/Floor_2");
        ceiling_2 = GameObject.Find("/MapGenerator/Ceiling_2");


        // obstacle0 = createObstacle(10f);
        // obstacle1 = createObstacle(obstacle0.transform.position.x);
        // obstacle2 = createObstacle(obstacle1.transform.position.x);
        // obstacle3 = createObstacle(obstacle2.transform.position.x);
        // obstacle4 = createObstacle(obstacle3.transform.position.x);

        coin = createCoin(20f); 

        entityBlockSeparation = 30;
        entityBlock1 = new GameObject[5];
        entityBlock2 = new GameObject[5];
        entityBlock1MinX = 15;
        entityBlock2MinX = 15 + entityBlockSeparation;
        generateEntityBlock(entityBlock1, entityBlock1MinX);
        generateEntityBlock(entityBlock2, entityBlock2MinX);

    }

    // Update is called once per frame
    void Update()
    {
        
        if(!gameManager.gameOver){
            baseSpeed = Mathf.Min(maxXspeed, baseSpeed+Time.deltaTime/speedIncreaseFactor);
            gameManager.incScore(baseSpeed*baseSpeed*Time.deltaTime);
            minXseparation = baseSpeed*0.9f;
            maxXseparation = baseSpeed*1.3f;

            moveBackground(floor);
            moveBackground(floor_2);
            moveBackground(ceiling);
            moveBackground(ceiling_2);

            moveCoin(coin);

            entityBlock1MinX = moveEntityBlock(entityBlock1, entityBlock1MinX);
            entityBlock2MinX = moveEntityBlock(entityBlock2, entityBlock2MinX);

            if(entityBlock1MinX < -45){
                entityBlock1MinX += 2 * entityBlockSeparation;
                destroyEntityBlock(entityBlock1);
                generateEntityBlock(entityBlock1, entityBlock1MinX);
            }
            else if(entityBlock2MinX < -45){
                entityBlock2MinX += 2 * entityBlockSeparation;
                destroyEntityBlock(entityBlock2);
                generateEntityBlock(entityBlock2, entityBlock2MinX);
            }

        }
        else{
            baseSpeed=0;
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")){
                SceneManager.LoadScene("MainScene");
                baseSpeed = 5;
            }
        }
    }

    private GameObject createCoin(float position){
        GameObject obst = GameObject.Instantiate(coinPrefab);
        obst.transform.position = new Vector3(position+Random.Range(minXseparation, maxXseparation), Random.Range(minYpositionObstacle, maxYpositionObstacle), 0);
        return obst;
    }

    private float moveEntityBlock(GameObject[] entityBlock, float entityBlockMinX){
        for(int i=0; i < entityBlock.Length; i++){
            entityBlock[i].transform.position -= new Vector3(baseSpeed, 0, 0) * Time.deltaTime;
        }
        return entityBlockMinX - (baseSpeed * Time.deltaTime);
    }

    private void generateEntityBlock(GameObject[] entityBlock, float entityBlockMinX){
        for(int i=0; i < entityBlock.Length; i++){
            entityBlock[i] = GameObject.Instantiate(obstaclePrefab);
            if(i == 0){
                entityBlock[i].transform.position = new Vector3(entityBlockMinX+ Random.Range(minXseparation, maxXseparation), Random.Range(minYpositionObstacle, maxYpositionObstacle), 0);
            }
            else{
                entityBlock[i].transform.position = new Vector3(entityBlock[i-1].transform.position.x + Random.Range(minXseparation, maxXseparation), Random.Range(minYpositionObstacle, maxYpositionObstacle), 0);
                if(entityBlock[i].transform.position.x > entityBlockMinX + entityBlockSeparation){
                    entityBlock[i].SetActive(false);
                }
            }
        }
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

    private void moveCoin(GameObject coin){
        
        coin.transform.position -= new Vector3(baseSpeed, 0, 0) * Time.deltaTime;
        if (coin.transform.position.x <= -15)
        {
            coin.transform.position = new Vector3(coin.transform.position.x+20+Random.Range(minXseparation, maxXseparation), Random.Range(minYpositionCoin, maxYpositionCoin), 0);
            coin.SetActive(true);
        }
    }
}
