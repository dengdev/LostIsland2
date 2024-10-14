using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour {

    public void QuitGame() {
        MusicManager.Instance.PlayConfirmSound();
        Application.Quit();
    }

    public void ContinueGame() {
        MusicManager.Instance.PlayConfirmSound();
        SaveLoadManager.Instance.Load();
    }

    public void StartGameWeek(int gameWeek) {
        GameManager.Instance.mainCanvas.gameObject.SetActive(false);
        MusicManager.Instance.PlayConfirmSound();
        EventHandler.CallStarNewGameEvent(gameWeek);
    }

    public void ToggleSettingPanel() {
        Transform panelTransform = this.transform.Find("SettingsPanel");

        if (panelTransform != null) {
            bool isActive = panelTransform.gameObject.activeSelf;
            panelTransform.gameObject.SetActive(!isActive);
        }
    }
}
