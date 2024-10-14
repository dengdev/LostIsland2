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
            lastClickTime = Time.time; // ���������ʱ��

            if (isDone) {
                dialogueController.ShowDialogueFinish();
            } else {
                dialogueController.ShowDialogueEmpty();
            }
        } 
    }

    protected override void OnclickedAction() {
        ShowPopup("�ɹ��ͻش�Ʊ��");
        dialogueController.ShowDialogueFinish();
        ObjectManager.Instance.UpdateInteractiveState(this.name, true);
    }
}
