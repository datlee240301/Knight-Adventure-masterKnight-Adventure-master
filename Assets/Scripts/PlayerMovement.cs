using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;
    private TrailRenderer tr;




    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AudioSource jumpSound;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private float dashSpeed = 6f;
    [SerializeField] private float dashDuration = 0.5f;
    public float playerMP { get; private set; } = 100f;
    [SerializeField] private float MPRecoverySpeed = 5;

    private MovementState state;
    private bool isGrounded;

    private bool isFacingRight;
    private float directX;  

    private bool isJumping;
    public static bool isAttacking = false;
    public static bool isDashing = false;
    private float attack = 0f;
    private float dash = 0f;
    public static int facingDirection = 1;
    public float currentMP;
    private enum MovementState
    {
        idle, running, jumping, falling
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        tr = GetComponent<TrailRenderer>();
        moveSpeed = GetComponent<Player>().GetPlayerSpeed();
        attack = attackDuration;
        dash = dashDuration;
        currentMP = playerMP;
        attackDuration = 0;
        dashDuration = 0;
        isAttacking = false;
        isDashing = false;
        facingDirection = 1;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {      
        if (!GetComponent<PlayerLife>().IsDead())
        {
            if(!isDashing && !isAttacking)
            {

                if (directX > 0f)
                {
                    isFacingRight = true;
                    facingDirection = 1;
                }
                else if (directX < 0f)
                {
                    isFacingRight = false;
                    facingDirection = -1;

                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        
        directX = Input.GetAxisRaw("Horizontal");
        isGrounded = IsGrounded();

        attackDuration = Mathf.Clamp(attackDuration - Time.deltaTime, 0f,attack);      
        dashDuration = Mathf.Clamp(dashDuration - Time.deltaTime, 0f,dash);
 

        if (!isDashing)
        {
            currentMP = Mathf.Clamp(currentMP + Time.deltaTime * MPRecoverySpeed, 0, playerMP);
        }
        if (!isAttacking && !isDashing)
        {
        rb.velocity = new Vector2(directX * moveSpeed, rb.velocity.y);
        }
    }
    void Update()
    {     
            if (Input.GetButtonDown("Jump"))
            {   
                    Jump();                
            }else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
            {
                Attack();
            }else if(Input.GetKeyDown(KeyCode.LeftShift) && currentMP >= 20)
            {
                StartCoroutine(Dash());
            }    
        UpdateAnimationMove();

    }
    private void Attack()
    {
        if(attackDuration <= 0f)
        {
            AudioManager.Instance.PlaySFX("Attack");
            isAttacking = true;
            if(isGrounded && !isDashing)
            {
                rb.velocity = new Vector2(0,rb.velocity.y);
            }
            anim.SetTrigger("isAttack");
            attackDuration = attack;      
        }
    
    }
    private void EndAttack()
    {
        isAttacking = false;
    }
    void Jump()
    {
        if (isGrounded)
        {
            AudioManager.Instance.PlaySFX("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
        }
        else if (isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = false;
        }
      

    }
    private IEnumerator Dash()
    {      
        if(dashDuration <= 0f)
        {
            AudioManager.Instance.PlaySFX("Dash");
            currentMP =  Mathf.Clamp(currentMP - 20, 0, playerMP);           
            isDashing = true;            
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;
            float dashVelocity = dashSpeed * facingDirection;
            rb.velocity = new Vector2(dashVelocity,0f);
            anim.SetTrigger("isDash");
            dashDuration = dash;
            tr.emitting = true;
            yield return new WaitForSeconds(0.3f);
            tr.emitting = false;
            rb.gravityScale = originalGravity;
            isDashing = false;
            
        }
       
    }

    private void UpdateAnimationMove()

    {
        if (directX != 0f)
        {
            if (!AudioManager.Instance.sfxSource.isPlaying && isGrounded)
            {
                AudioManager.Instance.PlaySFX("Run");
            }
            state = MovementState.running;
            sprite.flipX = !isFacingRight;
        }
        else
        {
            state = MovementState.idle;

        }
        if (rb.velocity.y > .1f)
        {

            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;

        }
        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayer);
    }  

}