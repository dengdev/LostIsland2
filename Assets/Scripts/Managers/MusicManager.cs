using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    public AudioClip openRoadClip;
    public AudioClip paperWingsClip;

    private AudioSource audioSource;
    protected override void Awake() {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOpenRoad() {
        PlayMusic(openRoadClip);
    }

    public void PlayPaperWings() {
        PlayMusic(paperWingsClip);
    }

    private void PlayMusic(AudioClip clip) {
        if (audioSource.clip != clip) {
            audioSource.Stop(); 
            audioSource.clip = clip; 
            audioSource.Play(); 
        }
    }

    public void StopMusic() {
        audioSource.Stop();
    }
}
