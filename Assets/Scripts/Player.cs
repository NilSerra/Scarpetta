using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float accelerationUp=50f;
    public bool gameOver = false;
    Rigidbody2D body;

    public ParticleSystem ps;
    private ParticleSystem.EmissionModule em;

    public Text gameOverText;


    // Start is called before the first frame update
    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        body = GetComponent<Rigidbody2D>();
        em = ps.emission;
        em.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetButton("Jump") && !gameOver){
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

    private void OnTriggerEnter2D(Collider2D other) {

        if (MapGenerator.scoreNumber > PlayerPrefs.GetInt("HighestScore", 0))
                PlayerPrefs.SetInt("HighestScore", (int)MapGenerator.scoreNumber);

        gameOver=true;
        body.velocity= new Vector2(0f,0f);
        gameOverText.gameObject.SetActive(true);
    }
}
