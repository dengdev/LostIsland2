using UnityEngine;

public class Ball : MonoBehaviour {
    public BallDetails ballDetails;
    public bool isMatch;

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetupBall(BallDetails ball) {
        ballDetails = ball;

        if (isMatch)
            SetRight();
        else
            SetWrong();
    }

    public void SetRight() {
        MusicManager.Instance.PlayConfirmSound();
        spriteRenderer.sprite = ballDetails.rightSprite;
    }

    public void SetWrong() {
        spriteRenderer.sprite = ballDetails.wrongSprite;
    }
}
