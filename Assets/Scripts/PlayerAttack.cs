using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 3;
    [SerializeField] private LayerMask enemyLayer;
    private float damage;
    [SerializeField] private BoxCollider2D coll;

    void Start()
    {
        damage = Player.Instance.GetPlayerDamage();
    }

    private void DamageEnemy()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(coll.bounds.center + transform.right * attackRange * PlayerMovement.facingDirection,
        new Vector3(coll.bounds.size.x * attackRange, coll.bounds.size.y, coll.bounds.size.z), 0.1f, enemyLayer);

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            EnemyLife enemyLife = enemyCollider.GetComponent<EnemyLife>();
            if (enemyLife != null && !enemyLife.IsDead())
            {
                enemyLife.TakeDamage(damage);
            }
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(coll.bounds.center + transform.right * attackRange * PlayerMovement.facingDirection,
           new Vector3(coll.bounds.size.x * attackRange, coll.bounds.size.y, coll.bounds.size.z));

    }
}