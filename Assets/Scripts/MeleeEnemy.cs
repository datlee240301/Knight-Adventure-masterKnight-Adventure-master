using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    private Animator anim;
    private PlayerLife playerLife;
    private EnemyMovement enemyMovement;
    private EnemyLife enemyLife;
    [SerializeField]private BoxCollider2D coll;
    [SerializeField] private float attackDuration = 1f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float attackRange;
    [SerializeField] private float findPlayerRange;
    [SerializeField] private int damage = 20;
    //[SerializeField] private float startPosition = 2f;

    [SerializeField] private bool hasShield = false;
    [SerializeField] private float shieldCooldown = 10f;
    //[SerializeField] private float shieldTime = 3f;
    private float coolDown = 0f;
    private bool isAttacking = false;
    private bool isSummoning = false;
    private bool isShielded = false;

    private float shieldDuration;
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        enemyMovement = GetComponentInParent<EnemyMovement>();
        enemyLife = GetComponentInParent<EnemyLife>();
        shieldDuration = shieldCooldown;
    }
    private void FixedUpdate()
    {     
        enemyLife.isAttacking = isAttacking;
        isSummoning = enemyLife.isSummoning;
        coolDown -= Time.deltaTime;
        if (hasShield)
        {
            shieldDuration -= Time.deltaTime;
            enemyLife.isShielded = isShielded;
        }
    }

    // Update is called once per frame
    void Update()
    {
            if (enemyMovement != null)
            {
                enemyMovement.enabled = !PlayerInsight();
                if (FindPlayer() && !isAttacking && !isSummoning && !PlayerInsight())
                {
                    enemyMovement.MoveToPlayer();
                }
                else
                {
                    enemyMovement.PlayerNotFound();
                }
                if (!isAttacking && !isSummoning)
                {
                    enemyMovement.enabled = true;
                }
                else
                {
                    enemyMovement.enabled = false;
                
                }
                if (enemyMovement.enabled == false)
                {
                    anim.SetInteger("state", 0);
                }

            }       

        if (PlayerInsight() && !enemyLife.IsDead() )
        {
            if(!isShielded && !isSummoning && !isAttacking)
            {
                if (coolDown < 0 )
                {
                    Attack();
                }
                else if(shieldDuration < 0 )
                {
                    StartCoroutine(Shield());
                }
            }
            anim.SetInteger("state", 0);
        }
        
      
    }
    private void Attack()
    {
        isAttacking = true;
        anim.SetTrigger("isAttack");
        coolDown = attackDuration;
    }
    private IEnumerator Shield()
    {
        anim.SetBool("isShield", true);
        isShielded = true;
        shieldDuration = shieldCooldown;
        yield return new WaitForSeconds(3f);
        isShielded = false;
        anim.SetBool("isShield", false);

    }
    public void EndAttack()
    {
        isAttacking = false;
    }

    private bool PlayerInsight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center + transform.right * attackRange * transform.localScale.x,
            new Vector3(coll.bounds.size.x* attackRange, coll.bounds.size.y, coll.bounds.size.z), 0.1f, Vector2.right, 0, playerLayer);
        if (hit.collider != null)
        {
            playerLife = hit.transform.GetComponent<PlayerLife>();
        }
        return hit.collider != null;
    }
    private bool FindPlayer()
    {
        Vector2 direction = transform.right.normalized * transform.localScale.x;
        Vector3 startLine = transform.position + (Vector3)direction * 2f;
        RaycastHit2D hitGround = Physics2D.Raycast(startLine, direction, findPlayerRange, groundLayer);
        RaycastHit2D hitPlayer;

        if (hitGround.collider != null)
        {
            Vector2 playerDirection = hitGround.point - (Vector2)transform.position;
            hitPlayer = Physics2D.Raycast(transform.position, playerDirection.normalized, playerDirection.magnitude, playerLayer);
        }
        else
        {
            hitPlayer = Physics2D.Raycast(startLine, direction, findPlayerRange, playerLayer);
        }

        return hitPlayer.collider != null;

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
    private void OnDrawGizmos()
    {
        //Attack Box
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(coll.bounds.center + transform.right * attackRange * transform.localScale.x,
        //   new Vector3(coll.bounds.size.x * attackRange, coll.bounds.size.y, coll.bounds.size.z));

        //Find Player Line
        //Gizmos.color = Color.yellow;
        //Vector2 direction = transform.right.normalized * transform.localScale.x;
        //Vector3 startLine = transform.position + (Vector3)direction * startPosition;
        //RaycastHit2D hitGround = Physics2D.Raycast(startLine, direction, findPlayerRange, groundLayer);
        //if (hitGround.collider != null)
        //{

        //    Gizmos.DrawLine(startLine, hitGround.point);
        //}
        //else
        //{
        //    Vector3 endPosition = transform.position + (Vector3)(direction * findPlayerRange);
        //    Gizmos.DrawLine(startLine, endPosition);
        //}


    }


}
