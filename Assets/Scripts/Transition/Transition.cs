using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Transition : MonoBehaviour {
    public Text dialogueText;
    public Text continueText;
    public float textDisplaySpeed = 0.1f;
    public float waitTime = 0.5f;

    private Stack<string> dialogueStack;
    private bool canContinue = false;

    private void Start() {
        InitializeDialogueStack();
        continueText.gameObject.SetActive(false);
        StartCoroutine(ShowDialogueRoutine());
    }

    private void InitializeDialogueStack() {
        dialogueStack = new Stack<string>();
        dialogueStack.Push("不远处，一座孤独的岛屿渐渐浮现在视野中....");
        dialogueStack.Push("幸好，身旁漂浮着一块破损的船身，给予你支撑");
        dialogueStack.Push("出海探险却遇上突如其来的风暴，船只已然毁坏");
    }

    private IEnumerator ShowDialogueRoutine() {
        while (dialogueStack.Count > 0) {
            string sentence = dialogueStack.Pop();
                dialogueText.text = "";
                float displayTime = Mathf.Max(textDisplaySpeed * sentence.Length, 1.0f);

                yield return dialogueText.DOText(sentence, displayTime).WaitForCompletion();
            yield return new WaitForSeconds(waitTime);
        }
        ShowContinuePrompt();
    }

    private void ShowContinuePrompt() {
            continueText.gameObject.SetActive(true);
            continueText.color = new Color(0.5f, 0.5f, 0.5f, 0);
            continueText.DOColor(Color.white, 2f).SetLoops(-1, LoopType.Yoyo);
            //continueText.DOFade(1, 2f).SetLoops(-1, LoopType.Yoyo); 
            canContinue = true;
    }

    private void Update() {
        if (canContinue && Input.GetMouseButtonDown(0)) {
            continueText.DOKill();
            continueText.gameObject.SetActive(false);
            TransitionManager.Instance.Transition(SceneName.Transition, SceneName.H1);
        }
    }
}
