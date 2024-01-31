using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private int damage = 10;
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

}
