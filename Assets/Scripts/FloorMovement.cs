using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FloorMovement : MonoBehaviour
{

    public Player player;
    private GameObject floor;
    private GameObject ceiling;
    private GameObject floor_2;
    private GameObject ceiling_2;

    private GameObject box0;
    private GameObject box1;
    private GameObject box2;
    private GameObject box3;
    private GameObject big_box;

    public float baseSpeed = 5f;

    private GameObject obstacle0;
    private GameObject obstacle1;
    private GameObject obstacle2;
    private GameObject obstacle3;
    private GameObject obstacle4;

    public GameObject obstaclePrefab;

    private float minXseparation = 5;
    private float maxXseparation = 7;

    public float minYpositionObstacle = -5;
    public float maxYpositionObstacle = 5;

    public float minYscaleObstacle = 2;
    public float maxYscaleObstacle = 6;

    public float maxXspeed = 20;

    public int speedIncreaseFactor = 60;

    public float scoreNumber=0;

    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        floor = GameObject.Find("/MapGenerator/Floor");
        ceiling = GameObject.Find("/MapGenerator/Ceiling");
        floor_2 = GameObject.Find("/MapGenerator/Floor_2");
        ceiling_2 = GameObject.Find("/MapGenerator/Ceiling_2");

        box0 = GameObject.Find("/MapGenerator/box0");
        box1 = GameObject.Find("/MapGenerator/box1");
        box2 = GameObject.Find("/MapGenerator/box2");
        box3 = GameObject.Find("/MapGenerator/box3");
        big_box = GameObject.Find("/MapGenerator/big_box"); 

        obstacle0 = createObstacle(10f);
        obstacle1 = createObstacle(obstacle0.transform.position.x);
        obstacle2 = createObstacle(obstacle1.transform.position.x);
        obstacle3 = createObstacle(obstacle2.transform.position.x);
        obstacle4 = createObstacle(obstacle3.transform.position.x);

    }

    private GameObject createObstacle(float position){
        GameObject obst = GameObject.Instantiate(obstaclePrefab);
        obst.transform.position = new Vector3(position+Random.Range(minXseparation, maxXseparation), Random.Range(minYpositionObstacle, maxYpositionObstacle), 0);
        return obst;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!player.gameOver){
            baseSpeed = Mathf.Min(maxXspeed, baseSpeed+Time.deltaTime/speedIncreaseFactor);

            minXseparation = baseSpeed*0.8f;
            maxXseparation = baseSpeed*1.2f;

            scoreNumber += baseSpeed*Time.deltaTime;
            scoreText.text = "Score: "+(int)scoreNumber+" m";

            moveBackground(floor);
            moveBackground(floor_2);
            moveBackground(ceiling);
            moveBackground(ceiling_2);
            moveBackground(box0);
            moveBackground(box1);
            moveBackground(box2);
            moveBackground(box3);
            moveBackground(big_box);

            moveObstacle(obstacle0, obstacle4);
            moveObstacle(obstacle1, obstacle0);
            moveObstacle(obstacle2, obstacle1);
            moveObstacle(obstacle3, obstacle2);
            moveObstacle(obstacle4, obstacle3);
        }
        else{
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")){
                SceneManager.LoadScene("DemoScreen");
            }
        }
    }

    private void moveBackground(GameObject background_element){
        
        background_element.transform.position -= new Vector3(baseSpeed*(1/(1+Mathf.Abs(background_element.transform.position.z))), 0, 0) * Time.deltaTime;
        if (background_element.transform.position.x <= -20)
        {
            background_element.transform.position = new Vector3(20, background_element.transform.position.y, background_element.transform.position.z);
        }
    }

    private void moveObstacle(GameObject obst, GameObject prevObst){
        
        obst.transform.position -= new Vector3(baseSpeed, 0, 0) * Time.deltaTime;
        if (obst.transform.position.x <= -15)
        {
            obst.transform.position = new Vector3(prevObst.transform.position.x+Random.Range(minXseparation, maxXseparation), Random.Range(minYpositionObstacle, maxYpositionObstacle), 0);
            obst.transform.localScale = new Vector3(obst.transform.localScale.x, Random.Range(minYscaleObstacle, maxYscaleObstacle),0);
        }
    }

    private void FixedUpdate() {
        
    }
}
