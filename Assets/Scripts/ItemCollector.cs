using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private float HPHeal = 10f;
    private PlayerLife playerLife;
    void Start()
    {
        playerLife = GetComponent<PlayerLife>();
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Coin"))
        {
            CollectCoin(coll);

        };
        if (coll.gameObject.CompareTag("Fruit"))
        {
            playerLife.Heal(HPHeal);
            CollectItem("Collect Fruit", coll);
        }
        if (coll.gameObject.CompareTag("Heart"))
        {
            playerLife.CollectHeart(); 
            CollectItem("Collect Heart", coll);
        }
    }
    private void CollectItem(string name,Collider2D coll)
    {
        AudioManager.Instance.PlaySFX(name);
        coll.GetComponent<Animator>().SetBool("isCollected", true);
        coll.GetComponent<Collider2D>().enabled = false;
        float animationDuration = coll.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(DelayDestroyObj(coll.gameObject, animationDuration - 0.5f));
    }
    private void CollectCoin(Collider2D coll)
    {
        FinishPoint.coin++;
        AudioManager.Instance.PlaySFX("CollectCoin");
        Destroy(coll.gameObject);
    }
    private IEnumerator DelayDestroyObj(GameObject gameObject,float delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        Destroy(gameObject);
    }
}
