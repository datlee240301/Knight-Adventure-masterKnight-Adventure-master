using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        PlayMusic("Theme0");
    }
   
    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, sound => sound.name == name);
        if (sound != null)
        {          
            musicSource.clip = sound.clip;                
            musicSource.Play();
        }
        else
        {
            Debug.Log(name + " Not Found");
        }
    }
    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, sound => sound.name == name);
        if (sound != null)
        {           
            sfxSource.PlayOneShot(sound.clip);
        }
        else
        {
            Debug.Log(name + " Not Found");
        }
    }
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
