using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
    public SceneName sourceScene;
    public SceneName targetScene;

    public void TeleportToScene() {
        TransitionManager.Instance.Transition(sourceScene, targetScene);
    }
}
