using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoneBoss : MonoBehaviour
{
    [SerializeField] private GameObject summon;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Collider2D coll;
    [SerializeField] private GameObject arm;
    [Header("Skills Cooldown")]
    [SerializeField] private float shieldCooldown = 15f;
    [SerializeField] private float healCooldown = 10f;
    [SerializeField] private float armAttackCooldown = 4f;
    [SerializeField] private float armShootCooldown = 10f;
    [SerializeField] private float summonCooldown = 1f;
    //[SerializeField] private float summonCooldownReduced = 1f;

    [Header("Skills Options")]
    [SerializeField] private float heightSummon = 20f;
    [SerializeField] private float attackRange = 2;
    [SerializeField] private float damage = 30;
    [SerializeField] private float healAmount = 300f;
    private Animator anim;
    private PlayerLife playerLife;
    public bool isDead = false;
    private float maxHP;
    private float currentHP;
    private bool isShielded = false;
    private bool isHealing = false;
    private bool isArmAttacking = false;
    private bool isArmShoot = false;
    private BossHealthBar healthBar;
    private Vector3 initialScale;
    private GameObject summonPosition;
    private GameObject armPosition;

    private GameObject player;
    private EnemyLife enemyLife;

    private float shieldDuration = 0f;
    private float healDuration = 0f;
    private float armAttackDuration = 0f;
    private float armShootDuration = 0f;
    private float summonDuration = 0f;
    //private float summonCoolDownReduced2;

    private bool isBossDead = false;

    private void Awake()
    {
        

        initialScale = transform.localScale;      
        //summonCoolDownReduced2 = summonCooldown;
        enemyLife = GetComponent<EnemyLife>();
    }
    void Start()
    {

        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        summonPosition = transform.Find("Summon Position").gameObject;
        armPosition = transform.Find("Arm Shoot Position").gameObject;
        player = GameObject.FindGameObjectWithTag("Player");       
        maxHP = enemyLife.currentHP;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isDead = enemyLife.isDead;
        shieldDuration -= Time.deltaTime;
        healDuration -= Time.deltaTime;
        armAttackDuration -= Time.deltaTime;
        armShootDuration -= Time.deltaTime;
        if (isShielded)
        {
            summonDuration -= Time.deltaTime;
        }
        else
        {
            summonDuration = summonCooldown;
        }
        enemyLife.isShielded = isShielded;
        currentHP = enemyLife.currentHP;
    }
    private void Update()
    {
        if (!isDead)
        {
            if (summonDuration < 0)
            {
                Summon();
            }
            if(!isShielded && !isHealing && !isArmShoot && !isArmAttacking)
            {
                if (armAttackDuration < 0 && PlayerInsight() )
                {
                    ArmAttack();
                }
                else if (armShootDuration < 0 )
                {
                    ArmShoot();
                }
                else if(shieldDuration < 0 && (currentHP < maxHP / 2))
                {
                  
                    StartCoroutine(Shield());
                
                }
                else if(healDuration < 0 && (currentHP < maxHP / 3))
                {
                     StartCoroutine(Heal());
                }          
            }
            UpdateSummonPosition();
            PlayerDirection();
        }
        else
        {
            anim.SetBool("isHealing", false);
            if (!isBossDead)
            {
                Score.score += 500;
                isBossDead = true;
            }
        }
        if(FinishPoint.isDone)
        {
            gameObject.SetActive(false);
        }
    }

    private void EndArmAttack()
    {
        isArmAttacking = false;
    }
    private void ArmShoot()
    {
        isArmShoot = true;
        anim.SetTrigger("isArmShoot");
        armShootDuration = armShootCooldown;
    }
    private void EndArmShoot()
    {
        isArmShoot = false;
    }
    private void ArmShootAttack()
    {
        Instantiate(arm, armPosition.transform.position, Quaternion.identity);
    }
    private void ArmAttack()
    {
        anim.SetTrigger("isArmAttack");
        armAttackDuration = armAttackCooldown;
        isArmAttacking = true;
    }
    
    private void Summon()
    {
        Instantiate(summon, summonPosition.transform.position, Quaternion.identity);
        summonDuration = summonCooldown;
    }
    //private void SetSpeedForStoneBossSummon(float value = 10f)
    //{
    //    StoneBossSummon[] stoneBossSummons = summon.GetComponentsInChildren<StoneBossSummon>();

    //    foreach (StoneBossSummon stoneBossSummon in stoneBossSummons)
    //    {
    //        stoneBossSummon.speed = value;
    //    }
    //}

    private IEnumerator Shield()
    {
        anim.SetBool("isShield", true);
        isShielded = true;
        shieldDuration = shieldCooldown;
        //summonCooldown = summonCooldownReduced;
        //SetSpeedForStoneBossSummon(15f);
        yield return new WaitForSeconds(5f);
        //SetSpeedForStoneBossSummon();
        //summonCooldown = summonCoolDownReduced2;
        isShielded = false;
        anim.SetBool("isShield", false);

            
    }   
    private IEnumerator Heal()
    {
        isHealing = true;
        healDuration = healCooldown;
        anim.SetBool("isHealing", true);
        yield return new WaitForSeconds(2f);
        enemyLife.currentHP = Mathf.Clamp(currentHP + healAmount, 0, maxHP);
        enemyLife.healthBar.UpdateHealthBar(currentHP + healAmount, maxHP);
        isHealing = false;
        anim.SetBool("isHealing", false);

    }
    private void UpdateSummonPosition()
    {
        Vector3 playerHeadPosition = player.transform.position + Vector3.up * heightSummon;
        summonPosition.transform.position = playerHeadPosition;

    }
    public void PlayerDirection()
    {       
            Vector3 targetPosition = player.transform.position;
            if (transform.position.x < targetPosition.x)
            {
                transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
            }        
    }
    private void DamagePlayer()
    {
        if (PlayerInsight())
        {
            if (transform.position.x < playerLife.transform.position.x)
            {
                PlayerLife.enemyPosition = 1;
            }
            else
            {
                PlayerLife.enemyPosition = -1;
            }
            StartCoroutine(playerLife.TakeDamage(damage));
        }

    }
    private bool PlayerInsight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center + transform.right * attackRange * transform.localScale.x,
            new Vector3(coll.bounds.size.x * attackRange, coll.bounds.size.y, coll.bounds.size.z), 0.1f, Vector2.right, 0, playerLayer);
        if (hit.collider != null)
        {
            playerLife = hit.transform.GetComponent<PlayerLife>();
        }
        return hit.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.position.x < collision.transform.position.x)
        {
            PlayerLife.enemyPosition = 1;
        }
        else
        {
            PlayerLife.enemyPosition = -1;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(collision.GetComponent<PlayerLife>().TakeDamage(damage));

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(coll.bounds.center + transform.right * attackRange * transform.localScale.x,
           new Vector3(coll.bounds.size.x * attackRange, coll.bounds.size.y, coll.bounds.size.z));
    }
}
