using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public bool testing = false;
    public float accelerationUp=50f;
    Rigidbody2D body;

    public ParticleSystem ps;
    private ParticleSystem.EmissionModule em;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        em = ps.emission;
        em.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetButton("Jump") && !gameManager.gameOver){
            body.AddForce(Vector2.up*accelerationUp, ForceMode2D.Force);
            em.enabled = true;
        }
        else if(Input.GetButtonUp("Jump")){
            body.AddForce(Vector2.up*0f, ForceMode2D.Force);
            em.enabled = false;
        }
        else{
            em.enabled = false;
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
}
