using System.Collections;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;
    [SerializeField]private GameObject healthBarGameObj;
    public FloatHealthBar healthBar;
    private SpriteRenderer sprite;
    [SerializeField] private float maxHP = 100;
    [SerializeField] private bool isEnemyFlight = false;

    public  bool isAttacking = false;
    public  bool isSummoning = false;
    public bool isShielded = false;
    public float currentHP;
    public float enemyCoin = 1f;
    public bool isDead = false;
    private bool hasDisappeared = false;
    private float timeDisappear = 0.3f;
    private float fadeTime = 0f;

    private void Awake()
    {
        if (healthBarGameObj == null)
        {
            healthBar = GetComponentInChildren<FloatHealthBar>();
        }
        else
        {
            healthBar = healthBarGameObj.GetComponent<FloatHealthBar>();
        }
    }
    void Start()
    {
       
        currentHP = maxHP;
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        healthBar.UpdateHealthBar(currentHP, maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        if(hasDisappeared)
        {
            if (isEnemyFlight)
            {
                    fadeTime += Time.deltaTime;
                    float newAlpha = sprite.color.a * (1 - (fadeTime / timeDisappear));
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, newAlpha);
                   
                    if (fadeTime > timeDisappear)
                    {
                        Destroy(transform.parent.gameObject);
                    }
                
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }

        }
    }
    private void Die()
    {
        FinishPoint.coin += enemyCoin;
        isDead = true;
        anim.SetTrigger("isDeath");
        coll.enabled = false;
        if (isEnemyFlight)
        {
            Rigidbody2D rb = GetComponentInChildren<Rigidbody2D>();
            rb.gravityScale = 2f;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
     
    }
    public void Disappear()
    {
       hasDisappeared = true;
    }
    public bool IsDead()
    {
        return isDead;
    }
    public void TakeDamage(float damage)
    {
        if (!isShielded)
        {
            currentHP = Mathf.Clamp(currentHP - damage, 0, maxHP);
            healthBar.UpdateHealthBar(currentHP,maxHP);
            if (currentHP > 1 && !isAttacking && !isSummoning)
            {
                    anim.SetTrigger("isTakeHit");     
            }
            else if(currentHP == 0)
            {
                Destroy(healthBar.gameObject);
                Die();
            }
        }
       
    }
}
