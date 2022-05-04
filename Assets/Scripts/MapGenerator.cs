using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{

    public GameObject player;
    public Animator playerAnimator;
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
    public GameObject doubleWallPrefab;
    public GameObject doubleWall1Prefab;
    public GameObject doubleWall2Prefab;
    public GameObject shieldPrefab;
    public GameObject gunPowerUpPrefab;
    public GameObject arrowUpPrefab;
    public GameObject arrowDownPrefab;
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
        entityBlock1 = new GameObject[10];
        entityBlock2 = new GameObject[10];
        entityBlock1MinX = 15;
        entityBlock2MinX = 15 + entityBlockSeparation;
        GenerateEntityBlock(entityBlock1, entityBlock1MinX);
        GenerateEntityBlock(entityBlock2, entityBlock2MinX);

        playerAnimator = player.GetComponentInChildren(typeof(Animator)) as Animator;
        playerAnimator.SetFloat("runningSpeed", 1);

    }

    // Update is called once per frame
    void Update()
    {
        if(!gameManager.gameOver){
            baseSpeed = Mathf.Min(maxXspeed, baseSpeed+Time.deltaTime/speedIncreaseFactor);
            gameManager.IncScore(baseSpeed*baseSpeed*Time.deltaTime);

            MoveBackground(floor);
            MoveBackground(floor_2);
            MoveBackground(ceiling);
            MoveBackground(ceiling_2);

            entityBlock1MinX = MoveEntityBlock(entityBlock1, entityBlock1MinX);
            entityBlock2MinX = MoveEntityBlock(entityBlock2, entityBlock2MinX);

            if(entityBlock1MinX < -45){
                entityBlock1MinX += 2 * entityBlockSeparation;
                DestroyEntityBlock(entityBlock1);
                GenerateEntityBlock(entityBlock1, entityBlock1MinX);
            }
            else if(entityBlock2MinX < -45){
                entityBlock2MinX += 2 * entityBlockSeparation;
                DestroyEntityBlock(entityBlock2);
                GenerateEntityBlock(entityBlock2, entityBlock2MinX);
            }
        }
        else{
            baseSpeed=0;
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")){
                SceneManager.LoadScene("MainScene");
                baseSpeed = 5;
            }
        }
        playerAnimator.SetFloat("runningSpeed", Mathf.Clamp(baseSpeed/5.0f, 1, 4));
    }

    private float MoveEntityBlock(GameObject[] entityBlock, float entityBlockMinX){
        for(int i=0; i < entityBlock.Length; i++){
            if(entityBlock[i]){
                entityBlock[i].transform.position -= new Vector3(baseSpeed, 0, 0) * Time.deltaTime;
            }
        }
        return entityBlockMinX - (baseSpeed * Time.deltaTime);
    }

    private void GenerateEntityBlock(GameObject[] entityBlock, float entityBlockMinX){
        for(int i=0; i < entityBlock.Length; i++){
            float maxY, minXseparation, maxXseparation;
            //Randomizing entities to spawn
            float random = Random.Range(0, 100);
            if(random > 93){
                // Coin Line
                entityBlock[i] = GameObject.Instantiate(coinLinePrefab);
                maxY = 4;
                minXseparation = baseSpeed*0.65f;
                maxXseparation = baseSpeed*1.1f;
            }
            else if(random > 86){
                // Coin Ball
                entityBlock[i] = GameObject.Instantiate(coinBallPrefab);
                maxY = 3;
                minXseparation = baseSpeed*0.4f;
                maxXseparation = baseSpeed*0.9f;
            }
            else if(random > 83){
                // Arrow Up
                entityBlock[i] = GameObject.Instantiate(arrowUpPrefab);
                maxY = 2.5f;
                minXseparation = baseSpeed*0.75f;
                maxXseparation = baseSpeed*1.1f;
            }
            else if(random > 80){
                // Arrow Down
                entityBlock[i] = GameObject.Instantiate(arrowDownPrefab);
                maxY = 2.5f;
                minXseparation = baseSpeed*0.75f;
                maxXseparation = baseSpeed*1.1f;
            }
            else if(random > 60){
                // 4 Blocks Wall
                entityBlock[i] = GameObject.Instantiate(wall4BlocksPrefab);
                maxY = 3.35f;
                minXseparation = baseSpeed*0.95f;
                maxXseparation = baseSpeed*1.05f;
            }
            else if(random > 30){
                // 5 Blocks Wall
                entityBlock[i] = GameObject.Instantiate(wall5BlocksPrefab);
                maxY = 2.9f;
                minXseparation = baseSpeed*1.05f;
                maxXseparation = baseSpeed*1.15f;
            }
            else if(random > 15){
                // 6 Blocks Wall
                entityBlock[i] = GameObject.Instantiate(wall6BlocksPrefab);
                maxY = 2.55f;
                minXseparation = baseSpeed*1.10f;
                maxXseparation = baseSpeed*1.2f;
            }
            else if(random > 13){
                // Double Wall
                entityBlock[i] = GameObject.Instantiate(doubleWallPrefab);
                maxY = 0f;
                minXseparation = baseSpeed*1.4f;
                maxXseparation = baseSpeed*1.6f;
            }
            else if(random > 9){
                // Double Wall 1
                entityBlock[i] = GameObject.Instantiate(doubleWall1Prefab);
                maxY = 0f;
                minXseparation = baseSpeed*1.3f;
                maxXseparation = baseSpeed*1.5f;
            }
            else if(random > 5){
                // Double Wall 2
                entityBlock[i] = GameObject.Instantiate(doubleWall2Prefab);
                maxY = 0f;
                minXseparation = baseSpeed*1.3f;
                maxXseparation = baseSpeed*1.5f;
            }
            else if(random > 2){
                // Shield PowerUp
                entityBlock[i] = GameObject.Instantiate(shieldPrefab);
                maxY = 4;
                minXseparation = baseSpeed*0.4f;
                maxXseparation = baseSpeed*0.9f;
            }
            else if(random >= 0){
                // Bullets PowerUp
                entityBlock[i] = GameObject.Instantiate(gunPowerUpPrefab);
                maxY = 4;
                minXseparation = baseSpeed*0.4f;
                maxXseparation = baseSpeed*0.9f;
            }
            else{
                maxY = minXseparation = maxXseparation = 0f;
            }
            //Placing the entities in the next entityBlock
            if(i == 0){
                entityBlock[i].transform.position = new Vector3(entityBlockMinX+ Random.Range(minXseparation*100, maxXseparation*100)/100, RandomMoreWeightExtremes(maxY), 0);
            }
            else{
                entityBlock[i].transform.position = new Vector3(entityBlock[i-1].transform.position.x + Random.Range(minXseparation*100, maxXseparation*100)/100, RandomMoreWeightExtremes(maxY), 0);
                if(entityBlock[i].transform.position.x > entityBlockMinX + entityBlockSeparation){
                    entityBlock[i].SetActive(false);
                }
            }
        }
    }
    private float RandomMoreWeightExtremes(float x){
        float random = ((Random.Range(0, 200) - 100)/100f) * x * 1.5f;
        if (random < 0){
            return Mathf.Max(random, -x);
        }
        else return Mathf.Min(random, x);
    }
    private void DestroyEntityBlock(GameObject[] entityBlock){
        for(int i=0; i < entityBlock.Length; i++){
            Destroy(entityBlock[i]);
        }
    }

    private void MoveBackground(GameObject background_element){
        background_element.transform.position -= new Vector3(baseSpeed, 0, 0) * Time.deltaTime;
        if (background_element.transform.position.x <= -20)
        {
            background_element.transform.position = new Vector3(20, background_element.transform.position.y, background_element.transform.position.z);
        }
    }
}