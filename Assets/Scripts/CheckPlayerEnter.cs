using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerEnter : MonoBehaviour
{
    private GameObject boss;
    private GameObject healthBar;

    private void Awake()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        healthBar = GameObject.FindGameObjectWithTag("Boss HealthBar");

        healthBar.SetActive(false);
    }
    void Start()
    {
        boss.GetComponent<StoneBoss>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            boss.GetComponent<StoneBoss>().enabled = true;
            healthBar.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (boss != null)
        {
            if (!boss.GetComponent<EnemyLife>().isDead)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    boss.GetComponent<StoneBoss>().enabled = false;
                    //boss.SetActive(false);                
                    if(healthBar != null)
                    {
                        healthBar.SetActive(false);
                    }
                }
            }
        }
    }
}
