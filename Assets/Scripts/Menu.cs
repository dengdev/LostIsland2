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

    public void GoBackToMenu() {
        MusicManager.Instance.PlayConfirmSound();
        TransitionManager.Instance.Transition(
            (SceneName)System.Enum.Parse(typeof(SceneName), SceneManager.GetActiveScene().name),
            TransitionManager.Instance.menuName);
        SaveLoadManager.Instance.Save();
    }

    public void StartGameWeek(int gameWeek) {
        MusicManager.Instance.PlayConfirmSound();
        EventHandler.CallStarNewGameEvent(gameWeek);
    }
}
