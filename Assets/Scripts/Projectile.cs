using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    public float projectileVelocity = 30f;
    private Rigidbody2D body;
    public AudioClip breakWall;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    
    }

    // Update is called once per frame
    void Update()
    {
        body = GetComponent<Rigidbody2D>();
        body.transform.position += new Vector3(projectileVelocity, 0, 0) * Time.deltaTime;
        if(body.transform.position.x > 50){
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
            if(collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "4BlockWall"||collision.gameObject.tag == "5BlockWall"||collision.gameObject.tag == "6BlockWall"||collision.gameObject.tag == "DoubleWall"||collision.gameObject.tag == "DoubleWall1"||collision.gameObject.tag == "DoubleWall2"){
                Animator[] animators = collision.gameObject.GetComponentsInChildren<Animator>();
                foreach(Animator anim in animators){
                    anim.Play("block_destroy");
                }
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(this.gameObject);
                AudioSource.PlayClipAtPoint(breakWall, transform.position);
            }
    }
}
