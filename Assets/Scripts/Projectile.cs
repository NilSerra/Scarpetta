using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    public float projectileVelocity = 40f;
    private Rigidbody2D body;

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
        
        switch (collision.gameObject.tag)
        {
            case "Obstacle":
                Animator[] animators = collision.gameObject.GetComponentsInChildren<Animator>();
                foreach(Animator anim in animators){
                    anim.Play("block_destroy");
                }
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(this.gameObject);
                break;
            default:
                break;
        }
        
    }
}
