using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    [SerializeField] private Slider slider;

    private void Start()
    {
        
    }
    public void UpdateHealthBar(float currentHP, float maxHP)
    {
        slider.value = currentHP / maxHP;
    }
}
