using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private float playerHP = 100f;
    private float playerDamage = 20f;
    private float playerSpeed = 5f;
    public  float maxPlayerHP { get; private set;} = 300f;
    public  float maxPlayerDamage { get; private set; } = 100f;
    public  float maxPlayerSpeed { get; private set; } = 10f;
    public float coin { get; private set; }
    private void Awake()
    {
        LoadPlayerData();
        if (Instance == null ) Instance = this;
    }
    //public Player(float playerHP, float playerDamage, float playerSpeed, float maxPlayerHP, float maxPlayerDamage, float maxPlayerSpeed, float coin)
    //{
    //    this.playerHP = playerHP;
    //    this.playerDamage = playerDamage;
    //    this.playerSpeed = playerSpeed;
    //    this.maxPlayerHP = maxPlayerHP;
    //    this.maxPlayerDamage = maxPlayerDamage;
    //    this.maxPlayerSpeed = maxPlayerSpeed;
    //    this.coin = coin;
    //}

    public void SetPlayerHP(float value = 30f)
    {
        playerHP = Mathf.Clamp(playerHP + value, 0 , maxPlayerHP);
        
        SavePlayerData();
    }

    public float GetPlayerHP()
    {
        return playerHP;
    }

    public void SetPlayerDamage(float value = 5)
    {
        playerDamage = Mathf.Clamp(playerDamage + value, 0, maxPlayerDamage); 
        SavePlayerData(); 
    }

    public float GetPlayerDamage()
    {
        return playerDamage;
    }
    public void SetPlayerSpeed(float value = 1)
    {
        playerSpeed = Mathf.Clamp(playerSpeed + value, 0, maxPlayerSpeed);
        SavePlayerData();
    }

    public float GetPlayerSpeed()
    {       
        return playerSpeed;
    }
    public void SetPlayerCoin(float value)
    {
        coin = Mathf.Clamp(coin + value, 0,Mathf.Infinity);
        SavePlayerData();
    }
    public float GetPlayerCoin()
    {
        return coin;
    }
    private void SavePlayerData()
    {
        PlayerPrefs.SetFloat("PlayerHP", playerHP);
        PlayerPrefs.SetFloat("PlayerDamage", playerDamage);
        PlayerPrefs.SetFloat("PlayerSpeed", playerSpeed);
        PlayerPrefs.SetFloat("PlayerCoin", coin);
        PlayerPrefs.Save();
    }

    private void LoadPlayerData()
    {
        playerHP = PlayerPrefs.GetFloat("PlayerHP",100f);
        playerDamage = PlayerPrefs.GetFloat("PlayerDamage",20f);
        playerSpeed = PlayerPrefs.GetFloat("PlayerSpeed",5f);
        coin = PlayerPrefs.GetFloat("PlayerCoin",150f);
    }
}
