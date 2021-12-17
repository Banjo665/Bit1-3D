using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSoundControl : MonoBehaviour
{
    AudioSource audioSource;

    public List<AudioClip> sounds = new List<AudioClip>();

    void Start()
    {
        audioSource = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<AudioSource>();
    }

    public void PlaySound(int i){
        if(audioSource.clip != sounds[i]){
            audioSource.clip = sounds[i];
        }
        if(!audioSource.isPlaying){
            audioSource.Play();
        }
    }
    public void Pause(){
        if(audioSource.isPlaying){
            audioSource.Pause();
        }
    }
}
