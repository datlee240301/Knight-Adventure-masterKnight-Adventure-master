using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{

    public static float coin = 0;
    public TextMeshProUGUI coinText;
    private GameObject boss;
   // public Transform enemy; 
    private Animator anim;
    public static bool isFinish = false;
    public static bool isOpenGate;
    public static bool isDone = false;
    private bool isBossDead;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        coin = 0;
        isDone = false;
        isFinish = false;
        isOpenGate = false;
        boss = GameObject.FindGameObjectWithTag("Boss");
    }

    void Update()
    {
        coinText.text = coin.ToString();
        if (boss != null)
        {
             isBossDead = boss.GetComponent<EnemyLife>().isDead;
        }
        if (!isOpenGate )
        {
           
            if(EnemyCount.enemyCount == 0 || isBossDead)
            {
                isOpenGate = true;
                OpenGate();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isOpenGate)
        {
            isDone = true;
            AudioManager.Instance.musicSource.Stop();
            AudioManager.Instance.PlaySFX("Victory");
            float currentCoin = PlayerPrefs.GetFloat("PlayerCoin");
            float level = SceneManager.GetActiveScene().buildIndex;
            float highScore = PlayerPrefs.GetFloat("HighScore" + level);
            PlayerMovement playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            playerMovement.enabled = false;
            if(Score.score > highScore)
            {
                PlayerPrefs.SetFloat("HighScore" + level, Score.score);
            }
            PlayerPrefs.SetFloat("PlayerCoin", currentCoin + coin);
            PlayerPrefs.Save();
            CompleteLevel();
        }
    }
    public void OpenGate()
    {
        anim.SetTrigger("isFinish");
    }
    private void CompleteLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("UnlockedLevel"))
        {          
            PlayerPrefs.SetInt("UnlockedLevel", Mathf.Clamp(PlayerPrefs.GetInt("UnlockedLevel", 1) + 1,0,3));
            PlayerPrefs.Save();
        }
    }
   
}
