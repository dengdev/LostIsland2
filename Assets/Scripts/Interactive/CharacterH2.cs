using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class CharacterH2 : Interactive {
    private DialogueController dialogueController;

    private void Awake() {
        dialogueController = GetComponent<DialogueController>();
    }

    public override void EmptyClicked() {

        if (isDone)
            dialogueController.ShowDialogueFinish();
        else
            dialogueController.ShowDialogueEmpty();
    }

    protected override void OnclickedAction() {
        ShowPopup("�ɹ��ͻش�Ʊ��");
        dialogueController.ShowDialogueFinish();
    }
}
