using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour {

    public void QuitGame() {
        Application.Quit();
    }

    public void ContinueGame() {
        SaveLoadManager.Instance.Load();
    }

    public void GoBackToMenu() {
        TransitionManager.Instance.Transition(
            (SceneName)System.Enum.Parse(typeof(SceneName), SceneManager.GetActiveScene().name),
            TransitionManager.Instance.menuName);
        SaveLoadManager.Instance.Save();
    }

    public void StartGameWeek(int gameWeek) {
        EventHandler.CallStarNewGameEvent(gameWeek);
    }
}
