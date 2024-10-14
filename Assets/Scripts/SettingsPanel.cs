using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SettingsPanel : MonoBehaviour {
    public Slider musicVolumeSlider;
    public Slider soundEffectsVolumeSlider;
    public Button returnMenuButton;
    public Button continueButton;
    public Button exitGameButton;

    private void Start() {
        musicVolumeSlider.value = MusicManager.Instance.GetMusicVolume();
        soundEffectsVolumeSlider.value = MusicManager.Instance.GetSoundEffectsVolume();

        returnMenuButton.onClick.AddListener(OnReturnMenuClicked);
        exitGameButton.onClick.AddListener(OnExitGameClicked);
        continueButton.onClick.AddListener(OnContinueClicked);

        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        soundEffectsVolumeSlider.onValueChanged.AddListener(OnSoundEffectsVolumeChanged);
    }


    private void OnMusicVolumeChanged(float value) {
        MusicManager.Instance.SetMusicVolume(value);
    }

    private void OnSoundEffectsVolumeChanged(float value) {
        MusicManager.Instance.SetSoundEffectsVolume(value);
    }

    private void OnReturnMenuClicked() {
        SaveLoadManager.Instance.Save();
        MusicManager.Instance.PlayConfirmSound();
        this.gameObject.SetActive(false);
        TransitionManager.Instance.Transition(
            (SceneName)System.Enum.Parse(typeof(SceneName), SceneManager.GetActiveScene().name),
            TransitionManager.Instance.menuName);

    }

    private void OnContinueClicked() {
        this.gameObject.SetActive(false);
    }

    private void OnExitGameClicked() {
        Application.Quit();
        this.gameObject.SetActive(false);
    }

}