using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerLife : MonoBehaviour
{
    [SerializeField] private float invicibilityTime = 0.35f;
    [SerializeField] private float bounceBack = 5f;
    [SerializeField] private float respawnTime = 2f;
    private PlayerMovement playerMovement;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector2 respawnPosition;

    private int life = 3;
    private float playerHP;
    public float currentHP { get; private set; }
    public int currentLife { get; private set; }
    private bool isDead = false;
    private float flashDuration;
    public  static int enemyPosition;
    public static bool isFail = false;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        sprite = GetComponent<SpriteRenderer>();
        playerHP = Player.Instance.GetPlayerHP();
        respawnPosition = transform.position;
        currentHP = playerHP;
        flashDuration = invicibilityTime;
        currentLife = life;
        isFail = false;
    }
    private void Update()
    {
        invicibilityTime = Mathf.Clamp(invicibilityTime - Time.deltaTime, 0, flashDuration);
       
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Death Zone"))
        {
            rb.bodyType = RigidbodyType2D.Static;
            StartCoroutine( Die());
        }

    }

    private IEnumerator Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        AudioManager.Instance.PlaySFX("Death");
        anim.SetTrigger("isDeath");             
        isDead = true;
        currentLife = currentLife - 1;
        if (currentLife == 0)
        {
            yield return new WaitForSeconds(1f);
            isFail = true;

        }
        else
        {
            yield return new WaitForSeconds(respawnTime);
            RespawnPlayer();
        }
    }

    public IEnumerator TakeDamage(float damage)
    {
        if (invicibilityTime == 0)
        {
            
            currentHP = Mathf.Clamp(currentHP - damage, 0, playerHP);
            PlayerMovement.isAttacking = false;
            if (currentHP > 0 && !isDead)
            {
                AudioManager.Instance.PlaySFX("Hurt");
                anim.SetTrigger("isTakeHit");
                StartCoroutine(FlashCharacter());
                rb.velocity = new Vector2(enemyPosition * bounceBack, bounceBack * 1.7f);
                invicibilityTime = flashDuration;
                yield return null;
            }
            else
            {
                yield return null;
                if(!isDead)
                StartCoroutine(Die());
            }
        }
    }

    private IEnumerator FlashCharacter()
    {
       
        float flashInterval = 0.1f;
        float timer = 0f;

        while (timer < flashDuration)
        {            
            sprite.enabled = !sprite.enabled;        
            yield return new WaitForSeconds(flashInterval);
            timer += flashInterval;
        }
        sprite.enabled = true;
        
    }
    public bool IsDead()
    {
        return isDead;
    }
    private void RespawnPlayer()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        anim.SetTrigger("respawn");
        transform.position = respawnPosition;
        currentHP = playerHP;
        playerMovement.currentMP = playerMovement.playerMP; 
        anim.SetInteger("state", 0);
        isDead = false;


    }
    public int GetLife()
    {
        return life;
    }
    public void UpdateCheckpoint(Vector2 checkpointPosition)
    {
        respawnPosition = checkpointPosition;
    }
    public void Heal(float value = 20f)
    {
        currentHP = Mathf.Clamp(currentHP + value, 0, playerHP);
    }
    public void CollectHeart()
    {
        currentLife = Mathf.Clamp(currentLife + 1, 0, 3);
    }
}
