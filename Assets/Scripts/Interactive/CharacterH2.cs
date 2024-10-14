using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class CharacterH2 : Interactive {
    private DialogueController dialogueController;

    private float clickCooldown = 0.8f;
    private float lastClickTime;

    private void Awake() {
        dialogueController = GetComponent<DialogueController>();
    }

    public override void EmptyClicked() {
        if (Time.time >= lastClickTime + clickCooldown) {
            lastClickTime = Time.time; // 更新最后点击时间

            if (isDone) {
                dialogueController.ShowDialogueFinish();
            } else {
                dialogueController.ShowDialogueEmpty();
            }
        } 
    }

    protected override void OnclickedAction() {
        ShowPopup("成功送回船票！");
        dialogueController.ShowDialogueFinish();
        ObjectManager.Instance.UpdateInteractiveState(this.name, true);
    }
}
