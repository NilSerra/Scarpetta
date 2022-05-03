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

    private GameObject[] entityBlock1;
    private GameObject[] entityBlock2;
    private float entityBlock1MinX;
    private float entityBlock2MinX;
    private float entityBlockSeparation;

    public GameObject coinBallPrefab;
    public GameObject coinLinePrefab;
    public GameObject wall4BlocksPrefab;
    public GameObject wall5BlocksPrefab;
    public GameObject wall6BlocksPrefab;
    public GameObject shieldPrefab;
    public GameObject gunPowerUpPrefab;

    private float minXseparation = 5;
    private float maxXseparation = 7;
    
    private float minYpositionWall4 = -2.9f;
    private float maxYpositionWall4 = 2.9f;
    
    private float minYpositionWall5 = -2.47f;
    private float maxYpositionWall5 = 2.44f;
    
    private float minYpositionWall6 = -2.07f;
    private float maxYpositionWall6 = 2.06f;

    private float minYpositionCoinLine = -4;
    private float maxYpositionCoinLine = 4;
    
    private float minYpositionCoinBall = -3;
    private float maxYpositionCoinBall = 3;

    public float maxXspeed = 20;

    public int speedIncreaseFactor = 60;

    // Start is called before the first frame update
    void Start()
    {
        floor = GameObject.Find("/MapGenerator/Floor");
        ceiling = GameObject.Find("/MapGenerator/Ceiling");
        floor_2 = GameObject.Find("/MapGenerator/Floor_2");
        ceiling_2 = GameObject.Find("/MapGenerator/Ceiling_2");

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

    private float moveEntityBlock(GameObject[] entityBlock, float entityBlockMinX){
        for(int i=0; i < entityBlock.Length; i++){
            if(entityBlock[i]){
                entityBlock[i].transform.position -= new Vector3(baseSpeed, 0, 0) * Time.deltaTime;
            }
        }
        return entityBlockMinX - (baseSpeed * Time.deltaTime);
    }

    private void generateEntityBlock(GameObject[] entityBlock, float entityBlockMinX){
        for(int i=0; i < entityBlock.Length; i++){
            float minY, maxY;

            //Randomizing entities to spawn
            float random = Random.Range(0, 100);
            if(random > 90){
                entityBlock[i] = GameObject.Instantiate(coinLinePrefab);
                minY = minYpositionCoinLine;
                maxY = maxYpositionCoinLine;
            }
            else if(random > 80){
                entityBlock[i] = GameObject.Instantiate(coinBallPrefab);
                minY = minYpositionCoinBall;
                maxY = maxYpositionCoinBall;
            }
            else if(random > 50){
                entityBlock[i] = GameObject.Instantiate(wall4BlocksPrefab);
                minY = minYpositionWall4;
                maxY = maxYpositionWall4;
            }
            else if(random > 20){
                entityBlock[i] = GameObject.Instantiate(wall5BlocksPrefab);
                minY = minYpositionWall5;
                maxY = maxYpositionWall5;
            }
            else if(random > 5){
                entityBlock[i] = GameObject.Instantiate(wall6BlocksPrefab);
                minY = minYpositionWall6;
                maxY = maxYpositionWall6;
            }
            else if(random > 2){
                entityBlock[i] = GameObject.Instantiate(shieldPrefab);
                minY = minYpositionCoinLine;
                maxY = maxYpositionCoinLine;
            }
            else if(random >= 0){
                entityBlock[i] = GameObject.Instantiate(gunPowerUpPrefab);
                minY = minYpositionCoinLine;
                maxY = maxYpositionCoinLine;
            }
            else{
                minY = 0;
                maxY = 0 ;
            }

            //Placing the entities in the next entityBlock
            if(i == 0){
                entityBlock[i].transform.position = new Vector3(entityBlockMinX+ Random.Range(minXseparation*100, maxXseparation*100)/100, Random.Range(minY*100, maxY*100)/100, 0);
            }
            else{
                entityBlock[i].transform.position = new Vector3(entityBlock[i-1].transform.position.x + Random.Range(minXseparation*100, maxXseparation*100)/100, Random.Range(minY*100, maxY*100)/100, 0);
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
}