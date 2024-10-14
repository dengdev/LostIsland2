using UnityEngine;

public class MailBox : Interactive {
    public Sprite openSprite;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D coll;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void OnEnable() {
        EventHandler.AfterSceneloadedEvent += OnAfterSceneLoadedEvent;
    }

    public void OnDisable() {
        EventHandler.AfterSceneloadedEvent -= OnAfterSceneLoadedEvent;
    }

    private void OnAfterSceneLoadedEvent() {

        if (!isDone) {
            transform.GetChild(0).gameObject.SetActive(false);
        } else {
            spriteRenderer.sprite = openSprite;
            //coll.enabled = false;
        }
    }

    protected override void OnclickedAction() {
        ShowPopup("成功打开邮箱！");
        spriteRenderer.sprite = openSprite;
        transform.GetChild(0).gameObject.SetActive(true);
        ObjectManager.Instance.UpdateInteractiveState(this.name, true);
    }

    public override void EmptyClicked() {
        if (!isDone) {
            ShowPopup("打开邮箱需要一把钥匙");

        } else {
            ShowPopup("现在它只是一个空邮箱");
        }
    }
}
