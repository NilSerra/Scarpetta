using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera cam;
    public Vector2 widthThresold = new Vector2(-40,0);
    public GameObject layer0;
    public float layer0Parallax = 0.9f;
    public GameObject layer1;
    public float layer1Parallax = 0.7f;
    public GameObject layer2;
    public float layer2Parallax = 0.4f;
    public GameObject layer3;
    public float layer3Parallax = 0.2f;

    public GameObject floor;
    public float floorParallax = 1f;
    private GameObject layer0_copy;
    private GameObject layer1_copy;
    private GameObject layer2_copy;
    private GameObject layer3_copy;

    private GameObject floor_copy;
    public GameObject background;

    public MapGenerator map;
    public TutorialGenerator tutorial;

    public bool isTutorial = false;

    void Start()
    {
        
        layer0_copy = cloneBackgroundElement(layer0);
        layer1_copy = cloneBackgroundElement(layer1);
        layer2_copy = cloneBackgroundElement(layer2);
        layer3_copy = cloneBackgroundElement(layer3);
        floor_copy = cloneBackgroundElement(floor);
    }

    private GameObject cloneBackgroundElement(GameObject layer){
        GameObject go = GameObject.Instantiate(layer);
        go.transform.position = new Vector3(go.GetComponent<SpriteRenderer>().bounds.size.x,go.transform.position.y,go.transform.position.z);
        return go;
    }

    void Update() {
        float speed = map == null ? 5 : map.baseSpeed;
        if (isTutorial)
        {
            speed = tutorial.baseSpeed;
        }

        moveBackground(layer0, layer0_copy, speed, layer0Parallax);
        moveBackground(layer1, layer1_copy, speed, layer1Parallax);
        moveBackground(layer2, layer2_copy, speed, layer2Parallax);
        moveBackground(layer3, layer3_copy, speed, layer3Parallax);
        moveBackground(floor, floor_copy, speed, floorParallax);

        moveBackground(layer0_copy, layer0, speed, layer0Parallax);
        moveBackground(layer1_copy, layer1, speed, layer1Parallax);
        moveBackground(layer2_copy, layer2, speed, layer2Parallax);
        moveBackground(layer3_copy, layer3, speed, layer3Parallax);
        moveBackground(floor_copy, floor, speed, floorParallax);
    }

    private void moveBackground(GameObject b1, GameObject b2, float speed, float parallaxFactor){

        b1.transform.position -= new Vector3(Time.deltaTime * speed * parallaxFactor, 0, 0) ;

        if (b1.transform.position.x <= widthThresold.x)
        {
            b1.transform.position = new Vector3(b2.transform.position.x + b2.GetComponent<SpriteRenderer>().bounds.size.x-1, b1.transform.position.y, b1.transform.position.z);
        }
    
    }
}
