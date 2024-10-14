using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Interactive {

    protected override void OnclickedAction() {
        ShowPopup("恭喜你！通关成功！");
        Invoke("ReturnToMainMenu", popupDuration + 0.5f);
        ObjectManager.Instance.UpdateInteractiveState(this.name, true);
    }

    public override void EmptyClicked() {
        int randomTip = Random.Range(0, 2);

        if (randomTip == 0) {
            ShowPopup("不知通往何处的传送门");
        } else {
            ShowPopup("可能需要某种物品来唤醒它");
        }
    }

    private void ReturnToMainMenu() {
        TransitionManager.Instance.Transition(SceneName.H5, SceneName.Menu);
        SaveLoadManager.Instance.Save();
    }
}
