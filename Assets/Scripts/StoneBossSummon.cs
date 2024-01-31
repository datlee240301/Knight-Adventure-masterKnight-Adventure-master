using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBossSummon : MonoBehaviour
{
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    public float speed;
    private bool hasDisappeared = false;
    private float timeDisappear = 0.3f;
    private float fadeTime = 0f;
    private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 fallVelocity = new Vector2(0, -speed);
        if (rb.bodyType == RigidbodyType2D.Dynamic)
            rb.velocity = fallVelocity;
        if (hasDisappeared)
        {     
                fadeTime += Time.deltaTime; 
                float newAlpha = sprite.color.a * (1 - (fadeTime / timeDisappear));
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, newAlpha);

                if (fadeTime > timeDisappear)
                {
                    Destroy(gameObject);
                }                   

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground"))
        {
            hasDisappeared = true;
            rb.bodyType = RigidbodyType2D.Static;
            rb.simulated = false;
        }
    }


       
    
}
