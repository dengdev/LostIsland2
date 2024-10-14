using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager> {
    public AudioClip openRoadClip;
    public AudioClip paperWingsClip;
    public AudioClip confirmSound;
    public AudioClip cancelSound;

    private float musicVolume = 0.5f;
    private float soundEffectsVolume = 0.8f;

    private AudioSource musicSource;
    private AudioSource soundEffectSource;
    protected override void Awake() {
        base.Awake();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        soundEffectSource = transform.GetChild(1).GetComponent<AudioSource>();

        musicSource.volume = musicVolume;
        soundEffectSource.volume = soundEffectsVolume;
    }

    public void SetMusicVolume(float value) {
        musicVolume = value;
        musicSource.volume = musicVolume;
    }

    public void SetSoundEffectsVolume(float value) {
        soundEffectsVolume = value;
        soundEffectSource.volume = soundEffectsVolume;
    }

    public float GetMusicVolume() {
        return musicVolume;
    }

    public float GetSoundEffectsVolume() {
        return soundEffectsVolume;
    }

    public void PlayOpenRoad() {
        PlayMusic(openRoadClip);
    }

    public void PlayPaperWings() {
        PlayMusic(paperWingsClip);
    }

    public void PlayConfirmSound() {
        soundEffectSource.PlayOneShot(confirmSound);
    }

    public void PlayConcelSound() {
        soundEffectSource.PlayOneShot(cancelSound);
    }

    private void PlayMusic(AudioClip clip) {
        if (musicSource.clip != clip) {
            StopMusic();
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void StopMusic() {
        musicSource.Stop();
    }
}
