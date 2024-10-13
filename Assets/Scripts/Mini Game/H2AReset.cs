using UnityEngine;
using DG.Tweening;

public class H2AReset : Interactive {
    private Transform gearSprite;

    private void Awake() {
        gearSprite = transform.GetChild(0);
    }

    public override void EmptyClicked() {
        MusicManager.Instance.PlayConcelSound();
        ShowPopup("球体已经重新归位");
        GameController.Instance.Resetgame();
        gearSprite.DOPunchRotation(Vector3.forward * 180, 1, 1, 0);
    }
}
