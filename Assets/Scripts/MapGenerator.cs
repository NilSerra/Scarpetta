using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Debugging;

public class MapGenerator : MonoBehaviour
{
    private GameObject floor;
    private GameObject ceiling;
    private GameObject floor_2;
    private GameObject ceiling_2;

    private GameObject[] entityBlock1;
    private GameObject[] entityBlock2;

    private Queue<GameObject> coinBallPrefabSet;
    private Queue<GameObject> coinLinePrefabSet;
    private Queue<GameObject> wall4BlocksPrefabSet;
    private Queue<GameObject> wall5BlocksPrefabSet;
    private Queue<GameObject> wall6BlocksPrefabSet;
    private Queue<GameObject> doubleWallPrefabSet;
    private Queue<GameObject> doubleWall1PrefabSet;
    private Queue<GameObject> doubleWall2PrefabSet;
    private Queue<GameObject> shieldPrefabSet;
    private Queue<GameObject> gunPowerUpPrefabSet;
    private Queue<GameObject> arrowUpPrefabSet;
    private Queue<GameObject> arrowDownPrefabSet;

    private float entityBlock1MinX;
    private float entityBlock2MinX;
    private float entityBlockSeparation;

    public GameObject player;
    public Animator playerAnimator;
    public GameManager gameManager;
    
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

    public int speedIncreaseFactor = 60;
    public float maxXspeed = 20;
    public float baseSpeed = 5f;


    // Start is called before the first frame update
    void Start()
    {
        floor = GameObject.Find("/MapGenerator/Floor");
        ceiling = GameObject.Find("/MapGenerator/Ceiling");
        floor_2 = GameObject.Find("/MapGenerator/Floor_2");
        ceiling_2 = GameObject.Find("/MapGenerator/Ceiling_2");

        coinBallPrefabSet = new Queue<GameObject>();
        coinLinePrefabSet = new Queue<GameObject>();
        wall4BlocksPrefabSet = new Queue<GameObject>();
        wall5BlocksPrefabSet = new Queue<GameObject>();
        wall6BlocksPrefabSet = new Queue<GameObject>();
        doubleWallPrefabSet = new Queue<GameObject>();
        doubleWall1PrefabSet = new Queue<GameObject>();
        doubleWall2PrefabSet = new Queue<GameObject>();
        shieldPrefabSet = new Queue<GameObject>();
        gunPowerUpPrefabSet = new Queue<GameObject>();
        arrowUpPrefabSet = new Queue<GameObject>();
        arrowDownPrefabSet = new Queue<GameObject>();
        
        GenerateQueue(coinBallPrefabSet, coinBallPrefab);
        GenerateQueue(coinLinePrefabSet, coinLinePrefab);
        GenerateQueue(wall4BlocksPrefabSet, wall4BlocksPrefab);
        GenerateQueue(wall5BlocksPrefabSet, wall5BlocksPrefab);
        GenerateQueue(wall6BlocksPrefabSet, wall6BlocksPrefab);
        GenerateQueue(doubleWallPrefabSet, doubleWallPrefab);
        GenerateQueue(doubleWall1PrefabSet, doubleWall1Prefab);
        GenerateQueue(doubleWall2PrefabSet, doubleWall2Prefab);
        GenerateQueue(shieldPrefabSet, shieldPrefab);
        GenerateQueue(gunPowerUpPrefabSet, gunPowerUpPrefab);
        GenerateQueue(arrowUpPrefabSet, arrowUpPrefab);
        GenerateQueue(arrowDownPrefabSet, arrowDownPrefab);

        entityBlockSeparation = 30;
        entityBlock1 = new GameObject[7];
        entityBlock2 = new GameObject[7];
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
        Debugging.DebugLog("Generating new Entity Block ");
        for(int i=0; i < entityBlock.Length; i++){
            float maxY, minXseparation, maxXseparation;
            //Randomizing entities to spawn
            float random = Random.Range(0, 100);
            if(random > 93){
                // Coin Line
                // entityBlock[i] = GameObject.Instantiate(coinLinePrefab);
                entityBlock[i] = DequeueOrInstantiate(coinLinePrefabSet, coinLinePrefab);
                maxY = 4;
                minXseparation = baseSpeed*1f;
                maxXseparation = baseSpeed*1.2f;
                entityBlock[i].SetActive(true);
            }
            else if(random > 86){
                // Coin Ball
                // entityBlock[i] = GameObject.Instantiate(coinBallPrefab);
                entityBlock[i] = DequeueOrInstantiate(coinBallPrefabSet, coinBallPrefab);
                maxY = 3;
                minXseparation = baseSpeed*0.5f;
                maxXseparation = baseSpeed*0.9f;
                entityBlock[i].SetActive(true);
            }
            else if(random > 83){
                // Arrow Up
                // entityBlock[i] = GameObject.Instantiate(arrowUpPrefab);
                entityBlock[i] = DequeueOrInstantiate(arrowUpPrefabSet, arrowUpPrefab);
                maxY = 3f;
                minXseparation = baseSpeed*0.75f;
                maxXseparation = baseSpeed*1.1f;
                entityBlock[i].SetActive(true);
            }
            else if(random > 80){
                // Arrow Down
                // entityBlock[i] = GameObject.Instantiate(arrowDownPrefab);
                entityBlock[i] = DequeueOrInstantiate(arrowDownPrefabSet, arrowDownPrefab);
                maxY = 3f;
                minXseparation = baseSpeed*0.75f;
                maxXseparation = baseSpeed*1.1f;
                entityBlock[i].SetActive(true);
            }
            else if(random > 60){
                // 4 Blocks Wall
                // entityBlock[i] = GameObject.Instantiate(wall4BlocksPrefab);
                entityBlock[i] = DequeueOrInstantiate(wall4BlocksPrefabSet, wall4BlocksPrefab);
                RestartAnimation(entityBlock[i]);
                maxY = 3.35f;
                minXseparation = baseSpeed*0.95f;
                maxXseparation = baseSpeed*1.05f;
            }
            else if(random > 30){
                // 5 Blocks Wall
                // entityBlock[i] = GameObject.Instantiate(wall5BlocksPrefab);
                entityBlock[i] = DequeueOrInstantiate(wall5BlocksPrefabSet, wall5BlocksPrefab);
                RestartAnimation(entityBlock[i]);
                maxY = 2.9f;
                minXseparation = baseSpeed*1.05f;
                maxXseparation = baseSpeed*1.15f;
            }
            else if(random > 15){
                // 6 Blocks Wall
                // entityBlock[i] = GameObject.Instantiate(wall6BlocksPrefab);
                entityBlock[i] = DequeueOrInstantiate(wall6BlocksPrefabSet, wall6BlocksPrefab);
                RestartAnimation(entityBlock[i]);
                maxY = 2.55f;
                minXseparation = baseSpeed*1.10f;
                maxXseparation = baseSpeed*1.2f;
            }
            else if(random > 13){
                // Double Wall
                // entityBlock[i] = GameObject.Instantiate(doubleWallPrefab);
                entityBlock[i] = DequeueOrInstantiate(doubleWallPrefabSet, doubleWallPrefab);
                RestartAnimation(entityBlock[i]);
                maxY = 0f;
                minXseparation = baseSpeed*1.4f;
                maxXseparation = baseSpeed*1.6f;
            }
            else if(random > 9){
                // Double Wall 1
                // entityBlock[i] = GameObject.Instantiate(doubleWall1Prefab);
                entityBlock[i] = DequeueOrInstantiate(doubleWall1PrefabSet, doubleWall1Prefab);
                RestartAnimation(entityBlock[i]);
                maxY = 0f;
                minXseparation = baseSpeed*1.3f;
                maxXseparation = baseSpeed*1.5f;
            }
            else if(random > 5){
                // Double Wall 2
                // entityBlock[i] = GameObject.Instantiate(doubleWall2Prefab);
                entityBlock[i] = DequeueOrInstantiate(doubleWall2PrefabSet, doubleWall2Prefab);
                RestartAnimation(entityBlock[i]);
                maxY = 0f;
                minXseparation = baseSpeed*1.3f;
                maxXseparation = baseSpeed*1.5f;
            }
            else if(random > 2){
                // Shield PowerUp
                // entityBlock[i] = GameObject.Instantiate(shieldPrefab);
                entityBlock[i] = DequeueOrInstantiate(shieldPrefabSet, shieldPrefab);
                maxY = 4;
                minXseparation = baseSpeed*0.5f;
                maxXseparation = baseSpeed*0.9f;
                entityBlock[i].SetActive(true);
            }
            else if(random >= 0){
                // Bullets PowerUp
                // entityBlock[i] = GameObject.Instantiate(gunPowerUpPrefab);
                entityBlock[i] = DequeueOrInstantiate(gunPowerUpPrefabSet, gunPowerUpPrefab);
                maxY = 4;
                minXseparation = baseSpeed*0.5f;
                maxXseparation = baseSpeed*0.9f;
                entityBlock[i].SetActive(true);
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
            Debugging.DebugLog("Generated" + entityBlock[i].tag + " in: " + entityBlock[i].transform.position.x + " , " + entityBlock[i].transform.position.y);
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
            Debugging.DebugLog("Enqueuing " + entityBlock[i].tag);
            switch(entityBlock[i].tag){
                case "CoinBall":
                    for (int j = 0; j < entityBlock[i].transform.childCount; j++){
                        entityBlock[i].transform.GetChild(j).gameObject.SetActive(true);
                    }
                    entityBlock[i].SetActive(false);
                    coinBallPrefabSet.Enqueue(entityBlock[i]);
                    break;
                case "CoinLine":
                    for (int j = 0; j < entityBlock[i].transform.childCount; j++){
                        entityBlock[i].transform.GetChild(j).gameObject.SetActive(true);
                    }
                    entityBlock[i].SetActive(false);
                    coinLinePrefabSet.Enqueue(entityBlock[i]);
                    break;
                case "ArrowUp":
                    entityBlock[i].SetActive(false);
                    arrowUpPrefabSet.Enqueue(entityBlock[i]);
                    break;
                case "ArrowDown":
                    entityBlock[i].SetActive(false);
                    arrowDownPrefabSet.Enqueue(entityBlock[i]);
                    break;
                case "Shield":
                    entityBlock[i].SetActive(false);
                    shieldPrefabSet.Enqueue(entityBlock[i]);
                    break;
                case "Gun":
                    entityBlock[i].SetActive(false);
                    gunPowerUpPrefabSet.Enqueue(entityBlock[i]);
                    break;
                case "4BlockWall":
                    for (int j = 0; j < entityBlock[i].transform.childCount; j++){
                        entityBlock[i].transform.GetChild(j).gameObject.SetActive(true);
                    }
                    entityBlock[i].SetActive(false);
                    wall4BlocksPrefabSet.Enqueue(entityBlock[i]);
                    break;
                case "5BlockWall":
                    for (int j = 0; j < entityBlock[i].transform.childCount; j++){
                        entityBlock[i].transform.GetChild(j).gameObject.SetActive(true);
                    }
                    entityBlock[i].SetActive(false);
                    wall5BlocksPrefabSet.Enqueue(entityBlock[i]);
                    break;
                case "6BlockWall":
                    for (int j = 0; j < entityBlock[i].transform.childCount; j++){
                        entityBlock[i].transform.GetChild(j).gameObject.SetActive(true);
                    }
                    entityBlock[i].SetActive(false);
                    wall6BlocksPrefabSet.Enqueue(entityBlock[i]);
                    break;
                case "DoubleWall":
                    for (int j = 0; j < entityBlock[i].transform.childCount; j++){
                        entityBlock[i].transform.GetChild(j).gameObject.SetActive(true);
                    }
                    entityBlock[i].SetActive(false);
                    doubleWallPrefabSet.Enqueue(entityBlock[i]);
                    break;
                case "DoubleWall1":
                    for (int j = 0; j < entityBlock[i].transform.childCount; j++){
                        entityBlock[i].transform.GetChild(j).gameObject.SetActive(true);
                    }
                    entityBlock[i].SetActive(false);
                    doubleWall1PrefabSet.Enqueue(entityBlock[i]);
                    break;
                case "DoubleWall2":
                    for (int j = 0; j < entityBlock[i].transform.childCount; j++){
                        entityBlock[i].transform.GetChild(j).gameObject.SetActive(true);
                    }
                    entityBlock[i].SetActive(false);
                    doubleWall2PrefabSet.Enqueue(entityBlock[i]);
                    break;
                default:
                    Destroy(entityBlock[i]);
                    break;
            }
        }
    }

    private void MoveBackground(GameObject background_element){
        background_element.transform.position -= new Vector3(baseSpeed, 0, 0) * Time.deltaTime;
        if (background_element.transform.position.x <= -20)
        {
            background_element.transform.position = new Vector3(20, background_element.transform.position.y, background_element.transform.position.z);
        }
    }

    private void GenerateQueue(Queue<GameObject> queue, GameObject prefab){
        Debugging.DebugLog("Generating pool of " + prefab.tag);
        for(int i=0; i < 5; i++){
            GameObject obj = GameObject.Instantiate(prefab);
            obj.SetActive(false);
            queue.Enqueue(obj);
        }
    }

    private GameObject DequeueOrInstantiate(Queue<GameObject> queue, GameObject prefab){
        if(queue.Count == 0){
            Debugging.DebugLog("Instantiating new " + prefab.tag);
            return GameObject.Instantiate(prefab);
        }
        Debugging.DebugLog("Dequeuing " + prefab.tag);
        return queue.Dequeue();
    }

    private void RestartAnimation(GameObject gameObject){
        Debugging.DebugLog("Restarting Animation for " + gameObject.tag);
        gameObject.SetActive(true);
        if(gameObject.tag == "DoubleWall" || gameObject.tag == "DoubleWall1" || gameObject.tag == "DoubleWall2"){
            for (int j = 0; j < gameObject.transform.childCount; j++){
                Animator[] animators = gameObject.transform.GetChild(j).gameObject.GetComponentsInChildren<Animator>();
                foreach(Animator anim in animators){
                    anim.Play(0);
                }
                gameObject.transform.GetChild(j).gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        else{
            Animator[] animators = gameObject.GetComponentsInChildren<Animator>();
            foreach(Animator anim in animators){
                anim.Play(0);
            }
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}