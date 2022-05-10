using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Debugging;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D body;
    public AudioClip breakWall;
    public float projectileVelocity = 30f;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        body = GetComponent<Rigidbody2D>();
        body.transform.position += new Vector3(projectileVelocity, 0, 0) * Time.deltaTime;
        if(body.transform.position.x > 50){
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
            if(collision.gameObject.tag == "4BlockWall"||collision.gameObject.tag == "5BlockWall"||collision.gameObject.tag == "6BlockWall"||collision.gameObject.tag == "DoubleWall"||collision.gameObject.tag == "DoubleWall1"||collision.gameObject.tag == "DoubleWall2"){
                Debugging.DebugLog("Projectile hit block wall");
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
