using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerLife playerLife;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;
    [SerializeField] private Image lifeBar;
    private PlayerMovement playerMovement;
    //private Player player;

    void Start()
    {
        playerMovement = playerLife.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = playerLife.currentHP / Player.Instance.GetPlayerHP();
        staminaBar.fillAmount = playerMovement.currentMP / playerMovement.playerMP;
        lifeBar.fillAmount = (float)playerLife.currentLife / (float)playerLife.GetLife();
    }


}
