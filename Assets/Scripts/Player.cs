using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject projectilePrefab;
    public bool testing = false;
    public float accelerationUp=50f;
    Rigidbody2D body;
    public ParticleSystem ps;
    private ParticleSystem.EmissionModule em;

    private float fireRate = 0.5f;
    private float nextFire = 0f;
    private bool useTouch;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        em = ps.emission;
        em.enabled = false;
        if(SystemInfo.deviceType == DeviceType.Desktop){
            useTouch = false;
        }
         else if(SystemInfo.deviceType == DeviceType.Handheld){
            useTouch = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!useTouch){
            if(Input.GetKeyDown(KeyCode.S) || (Input.GetMouseButtonDown(0) && Input.mousePosition.x < Screen.width / 2.0)){
                ShootProjectile(body.transform.position);
            }
        }
        else{ 
            var tapCount = Input.touchCount;
            for (var i = 0 ; i < tapCount ; i++) {
                var touch = Input.GetTouch(i);
                if(touch.position.x < Screen.width / 2.0){
                    ShootProjectile(body.transform.position);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if(!useTouch){
            if((Input.GetButton("Jump")) || (Input.GetMouseButton(0) && Input.mousePosition.x >= Screen.width / 2.0) && !gameManager.gameOver){
                Fly();
            }
            else{
                em.enabled = false;
            }
        }
        else{ 
            em.enabled = false;
            var tapCount = Input.touchCount;
            for (var i = 0 ; i < tapCount ; i++) {
                var touch = Input.GetTouch(i);
                if(touch.position.x >= Screen.width / 2.0 && !gameManager.gameOver){
                    Fly();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
        switch (collision.gameObject.tag)
        {
            case "Coin":
                // body.velocity = new Vector2(0f,10f);
                gameManager.incCoins();
                collision.gameObject.SetActive(false);
                break;
            case "Obstacle":
                if(!testing){
                    body.velocity = new Vector2(0f,0f);
                    gameManager.EndGame();
                }
                break;
            default:
                break;
        }
        
    }

    private void Fly(){
        body.AddForce(Vector2.up*accelerationUp, ForceMode2D.Force);
        em.enabled = true;
    }

    public void ShootProjectile(Vector3 position){
        if(!gameManager.gameOver && Time.time > nextFire){
            GameObject newProjectile = GameObject.Instantiate(projectilePrefab, new Vector3(position.x+0.6f, position.y+0.1f, position.z), Quaternion.identity);
            nextFire = Time.time + fireRate;
        }
        
    }
}
