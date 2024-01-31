using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;
    public Sprite locked;
    public Sprite unlocked;
    private void Start()
    {
        int unlockLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        Debug.Log(unlockLevel);
        for (int i = 0; i < buttons.Length; i++)
        {
            Text buttonText = buttons[i].GetComponentInChildren<Text>();
            if (i < unlockLevel)
            {
                buttons[i].interactable = true;
                buttons[i].image.sprite = unlocked;
                if (buttonText != null)
                {
                    buttonText.gameObject.SetActive(true);
                float score = PlayerPrefs.GetFloat("HighScore" + (i + 1));
                buttonText.text = "High Score\n" + score.ToString();
                }
            }
            else
            {
                buttons[i].interactable = false;
                buttons[i].image.sprite = locked;
                if (buttonText != null)
                {
                    buttonText.gameObject.SetActive(false);
                }
            }
        }
    }
    public void OpenLevel(int levelId)
    {
        string levelName = "Level " + levelId;
        SceneManager.LoadScene(levelName);
        if (Time.timeScale == 0f) Time.timeScale = 1f;
        //Scene scene = SceneManager.GetSceneByName(levelName);
        //if (scene.IsValid())
        //{
        //    SceneManager.LoadScene(levelName);
        //}
        //else
        //{
        //    //SceneManager.LoadScene(sceneName: "Start Menu");
        //    Debug.LogWarning(levelName + " does not exist");
        //}
    }

}
