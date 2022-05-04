using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject projectilePrefab;
    private GameObject shield;
    public bool testing = false;
    public bool hasShield = false;
    public int ammo = 0;
    public float accelerationUp=50f;
    Rigidbody2D body;
    // public ParticleSystem ps;
    // private ParticleSystem.EmissionModule em;
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


    //public AudioSource runningSound;
    public AudioClip coinPickup;
    public AudioClip breakWall;
    public AudioClip gameOver;
    public AudioClip powerUp;
    public AudioClip shoot;
    public AudioClip arrows;



    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        // em = ps.emission;
        // em.enabled = false;
        shield = this.transform.Find("ShieldPlayerPrefab").gameObject;

        LoadSkin();

        playerAnimator = GetComponentInChildren(typeof(Animator)) as Animator;
        playerAnimator.Play("Run");

        //runningSound = GetComponent<RunningSound>();

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
            // else{
            //     em.enabled = false;
            // }
        }
        else{ 
            // em.enabled = false;
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
                AudioSource.PlayClipAtPoint(coinPickup, transform.position);
                break;
            case "Obstacle":
                if(!testing){
                    if(hasShield){
                        hasShield = false;
                        shield.SetActive(false);
                        // Destroy block animation

                        // Transform[] allChildren = collision.gameObject.GetComponentsInChildren<Transform>();
                        // foreach (Transform child in allChildren)
                        // {
                        //     Animator blockAnimator = child.gameObject.GetComponent<Animator>();
                        //     blockAnimator.Play("block_destoy");
                        // }
                        
                        // collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                        collision.gameObject.SetActive(false);
                        AudioSource.PlayClipAtPoint(breakWall, transform.position);
                    }
                    else{
                        body.velocity = new Vector2(0f,0f);
                        playerAnimator.Play("Die");
                        gameManager.EndGame();
                        AudioSource.PlayClipAtPoint(gameOver, transform.position);
                    }
                    
                }
                break;
            case "Ground":
                if (!gameManager.gameOver && playerAnimator.GetBool("isFlying")){
                    playerAnimator.Play("Land");
                    playerAnimator.SetBool("isFlying", false);
                    // play the runningSound sound
                    //runningSound.PlayOneShot(runningSound, 0.5f);

                }
                break;
            case "Shield":
                hasShield = true;
                shield.SetActive(true);
                collision.gameObject.SetActive(false);
                AudioSource.PlayClipAtPoint(powerUp, transform.position);
                break;
            case "Gun":
                ammo += 3;
                gameManager.SetAmmo(ammo);
                collision.gameObject.SetActive(false);
                AudioSource.PlayClipAtPoint(powerUp, transform.position);
                break;
            case "ArrowUp":
                body.velocity = new Vector2(0f,12f);
                collision.gameObject.SetActive(false);
                AudioSource.PlayClipAtPoint(arrows, transform.position);
                break;
            case "ArrowDown":
                body.velocity = new Vector2(0f,-10f);
                collision.gameObject.SetActive(false);
                AudioSource.PlayClipAtPoint(arrows, transform.position);
                break;
            default:
                break;
        }
    }

    private void Fly(){
        if(!gameManager.gameOver){
            body.AddForce(Vector2.up*accelerationUp, ForceMode2D.Force);
            // em.enabled = true;
            // stop the runningSound sound
            //runningSound.Stop();
        }
        
        if(!playerAnimator.GetBool("isFlying")){
            playerAnimator.Play("Jump");
            playerAnimator.SetBool("isFlying", true);
            //runningSound.Stop();
        }
    }

    public void ShootProjectile(Vector3 position){
        if(ammo > 0 && !gameManager.gameOver && Time.time > nextFire){
            ammo -= 1;
            gameManager.SetAmmo(ammo);
            GameObject newProjectile = GameObject.Instantiate(projectilePrefab, new Vector3(position.x+0.6f, position.y+0.1f, position.z), Quaternion.identity);
            nextFire = Time.time + fireRate;
            AudioSource.PlayClipAtPoint(shoot, transform.position);
        }
        
    }
}
