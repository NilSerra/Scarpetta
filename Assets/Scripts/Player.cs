using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject projectilePrefab;
    public bool testing = false;
    public bool hasShield = false;
    public int ammo = 0;
    public float accelerationUp=50f;
    Rigidbody2D body;
    public ParticleSystem ps;
    private ParticleSystem.EmissionModule em;
    public Animator playerAnimator;
    private float fireRate = 0.5f;
    private float nextFire = 0f;

    public CharacterSkinManager csm;
    public SpriteRenderer headSprite;
    public SpriteRenderer bodySprite;
    public SpriteRenderer hand1Sprite;
    public SpriteRenderer hand2Sprite;
    public SpriteRenderer leg1Sprite;
    public SpriteRenderer leg2Sprite;
    public SpriteRenderer accessorySprite;
    public SpriteRenderer gunSprite;



    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        em = ps.emission;
        em.enabled = false;

        LoadSkin();

        playerAnimator = GetComponentInChildren(typeof(Animator)) as Animator;
        playerAnimator.Play("Run");

    }

    private void LoadSkin(){
        headSprite.sprite = csm.GetSpriteFromBodyPart("head");

        bodySprite.sprite = csm.GetSpriteFromBodyPart("body");
        
        hand1Sprite.sprite = csm.GetSpriteFromBodyPart("hands");
        hand2Sprite.sprite = csm.GetSpriteFromBodyPart("hands");
        
        leg1Sprite.sprite = csm.GetSpriteFromBodyPart("legs");
        leg2Sprite.sprite = csm.GetSpriteFromBodyPart("legs");
        
        accessorySprite.sprite = csm.GetSpriteFromBodyPart("accessory");
        
        gunSprite.sprite = csm.GetSpriteFromBodyPart("gun");
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameManager.useTouch){
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
        if(!gameManager.useTouch){
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
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
        switch (collision.gameObject.tag)
        {
            case "Coin":
                gameManager.IncCoins();
                collision.gameObject.SetActive(false);
                break;
            case "Obstacle":
                if(!testing){
                    if(hasShield){
                        hasShield = false;
                        // Destroy block animation
                        collision.gameObject.SetActive(false);
                    }
                    else{
                        body.velocity = new Vector2(0f,0f);
                        playerAnimator.Play("Die");
                        gameManager.EndGame();
                    }
                    
                }
                break;
            case "Ground":
                if (!gameManager.gameOver && playerAnimator.GetBool("isFlying")){
                    playerAnimator.Play("Land");
                    playerAnimator.SetBool("isFlying", false);
                }
                //this if to solve the problem of first time playing there is no animation playing
                if (!gameManager.gameOver){
                    playerAnimator.Play("Run");
                    playerAnimator.SetBool("isFlying", false);
                }
                break;
            case "Shield":
                hasShield = true;
                collision.gameObject.SetActive(false);
                break;
            case "Gun":
                ammo += 3;
                gameManager.SetAmmo(ammo);
                collision.gameObject.SetActive(false);
                break;
            case "ArrowUp":
                body.velocity = new Vector2(0f,12f);
                collision.gameObject.SetActive(false);
                break;
            case "ArrowDown":
                body.velocity = new Vector2(0f,-10f);
                collision.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    private void Fly(){
        if(!gameManager.gameOver){
            body.AddForce(Vector2.up*accelerationUp, ForceMode2D.Force);
            em.enabled = true;
        }
        
        if(!playerAnimator.GetBool("isFlying")){
            playerAnimator.Play("Jump");
            playerAnimator.SetBool("isFlying", true);
        }
    }

    public void ShootProjectile(Vector3 position){
        if(ammo > 0 && !gameManager.gameOver && Time.time > nextFire){
            ammo -= 1;
            gameManager.SetAmmo(ammo);
            GameObject newProjectile = GameObject.Instantiate(projectilePrefab, new Vector3(position.x+0.6f, position.y+0.1f, position.z), Quaternion.identity);
            nextFire = Time.time + fireRate;
        }
        
    }
}
