using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : MeleeEnemy
{
    private Animator anim;
    private EnemyLife enemyLife;
    private EnemyMovement enemyMovement;
    private float shieldDuration;
    private float timeToShield;
    [SerializeField]private float coolDown = 10f;
    [SerializeField] private float shieldTime = 3f;
    private bool isShielded = false;
    void Start()
    {
        anim = GetComponent<Animator>();
        enemyLife = GetComponent<EnemyLife>();
        enemyMovement = GetComponentInParent<EnemyMovement>();
        shieldDuration = coolDown;
        timeToShield = shieldTime;
    }

    // Update is called once per frame
    void Update()
    {
        enemyLife.isShielded = isShielded;
        coolDown -= Time.deltaTime;
        Debug.Log("find: "+enemyMovement.IsFindPlayer());
        Debug.Log("attack"+!enemyLife.isAttacking);
        Debug.Log("summon"+!enemyLife.isSummoning);

    }
   

}
