using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ArmBoss : MonoBehaviour
{

    private GameObject player;
    private Rigidbody2D rb;
    public float speed;
    private bool hasDisappeared = false;
    private float timeDisappear = 0.3f;
    private float fadeTime = 0f;
    private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x,direction.y).normalized * speed;
        float rot = Mathf.Atan2(-direction.y,-direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rot+180);
    }

    // Update is called once per frame
    void Update()
    {
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
