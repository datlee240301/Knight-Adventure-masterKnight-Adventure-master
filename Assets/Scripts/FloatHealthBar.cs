using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatHealthBar : MonoBehaviour
{
    [SerializeField]private Transform enemy;
    [SerializeField]private Slider slider;
    [SerializeField] private Vector3 height = Vector3.up;
    private void Start()
    {       

    }
    public void UpdateHealthBar(float currentHP,float maxHP)
    {
        slider.value = currentHP / maxHP;
    }
    // Update is called once per frame when
    void Update()
    {       
        if(enemy.GetComponent<StoneBoss>() == null)
        {
            if(enemy != null)
            {      
                transform.position = enemy.position + height * 2f;
            }
            else
            {
                gameObject.SetActive(false);
            }
            transform.rotation = Camera.main.transform.rotation;
            Vector3 healthBarLocalScale = new Vector3(enemy.localScale.x,1,1);
            transform.localScale = healthBarLocalScale;  
        }
        else
        {
            if (FinishPoint.isDone)
            {
                gameObject.SetActive(false);
            }
        }
       
    }
}
