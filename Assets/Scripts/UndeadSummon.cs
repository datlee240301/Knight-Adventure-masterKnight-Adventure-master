using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadSummon : MonoBehaviour
{

    private Animator anim;
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    public float speed;

    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        rb =   GetComponent<Rigidbody2D>();     
    }
    void Update()
    {
        Vector2 fallVelocity = new Vector2(0, -speed); 
        if(rb.bodyType == RigidbodyType2D.Dynamic)
            rb.velocity = fallVelocity;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("Idle",false );
            rb.bodyType = RigidbodyType2D.Static;
            anim.SetTrigger("isHit");
            
        }
    }

    public void DestroySummon()
    {
        Destroy(gameObject);
    }
    public void Appear()
    {
        anim.SetBool("Idle", true);
    }
}
