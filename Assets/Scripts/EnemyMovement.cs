using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    [SerializeField] private float speed;
    [SerializeField] private float idle = 1.5f;

    private Animator anim;
    private Vector3 initialScale;
    private EnemyLife enemyLife;
    private MovementState state;
    private bool findPlayer = false;
    private float initialSpeed;
    private bool isMove = true;

    [SerializeField] private GameObject[] waypoints;
    private int currentWaypoint = 0;
    private enum MovementState
    {
        idle, running
    }
    void Start()
    {
        initialScale = enemy.localScale;
        anim = enemy.GetComponent<Animator>();
        enemyLife = enemy.GetComponent<EnemyLife>();       
        initialSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.GetComponent<EnemyLife>().IsDead() || enemyLife.isAttacking || enemyLife.isSummoning)
        {
            speed = 0;
        }
        else if (findPlayer)
        {
            if (!enemyLife.isShielded)
            {
                speed = initialSpeed + 2;
            }
            else
            {
                speed = initialSpeed - 1;
            }
        }
        else
        {
            speed = initialSpeed;
          
            MoveToPoint();
        }

        anim.SetInteger("state", (int)state);
    }
    private IEnumerator ChangeDirect()
    {
        isMove = false;
        state = MovementState.idle;
        yield return new WaitForSeconds(idle);
        currentWaypoint++;
        isMove = true;
    }
    private void MoveToPoint()
    {
        
        if (currentWaypoint >= waypoints.Length)
        {
            currentWaypoint = 0;
        }
        if (enemy.position.x < waypoints[currentWaypoint].transform.position.x)
        {
            enemy.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z); ;
        }
        else
        {
            enemy.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z); ;
        }

        if (Vector2.Distance(waypoints[currentWaypoint].transform.position, enemy.position) < .1f)
        {
            if (isMove)
            {
               StartCoroutine(ChangeDirect());                
            }
        }      
        if (isMove)
        {
            state = MovementState.running;
            enemy.position = Vector2.MoveTowards(enemy.position, waypoints[currentWaypoint].transform.position, Time.deltaTime * speed);
        }
    }
    public void PlayerNotFound()
    {
        findPlayer = false;
    }
    public bool IsFindPlayer()
    {
        return findPlayer;
    }
    public void MoveToPlayer()
    {
        state = MovementState.running;
        findPlayer = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 targetPosition = player.transform.position;

            Vector3 targetPositionHorizontal = new Vector3(targetPosition.x - 2f * enemy.localScale.x, enemy.position.y, enemy.position.z);

            float distanceToTarget = Vector3.Distance(enemy.position, targetPositionHorizontal);

            if (distanceToTarget > 0.1f)
            {
                enemy.position = Vector3.MoveTowards(enemy.position, targetPositionHorizontal, speed * Time.deltaTime);

                if (enemy.position.x < targetPositionHorizontal.x)
                {
                    enemy.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
                }
                else
                {
                    enemy.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
                }
            }
        }
    }



    private void OnDisable()
    {
        state = MovementState.idle;
    }
}
