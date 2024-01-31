using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static float score = 1000;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] private float scoreDuration;
    [SerializeField]private float timeDuration = 1f;
    private float time;
    private void Start()
    {
        score = 1000;
        time = timeDuration;
    }
    void Update()
    {
        timeDuration -= Time.deltaTime;
        if(timeDuration <= 0f && !FinishPoint.isDone)
        {
            score = Mathf.Clamp(score - scoreDuration,0,1500);
            scoreText.text = score.ToString();
            timeDuration = time;
        }
    }
}
