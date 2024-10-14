using UnityEngine;
using DG.Tweening;

public class SpecialEffect : MonoBehaviour {
    private float rotationDuration = 1.5f;
    private float rotationAngle = 360f;
    private float scaleDuration = 1.5f;
    private Vector3 minScale = new Vector3(0.5f, 0.5f, 1f);
    private Vector3 maxScale = new Vector3(1f, 1f, 1f);

    private Tween rotationTween;
    private Tween scaleTween;

    private void Start() {
        StartAutoRotate();
        StartAutoScale();
    }

    private void StartAutoRotate() {
        if (transform != null) {
            rotationTween = transform.DORotate(new Vector3(0, 0, rotationAngle), rotationDuration, RotateMode.LocalAxisAdd)
                     .SetLoops(-1, LoopType.Incremental)
            .OnKill(() => rotationTween = null);
        }
    }

    private void StartAutoScale() {
        if (transform != null) {
            scaleTween = transform.DOScale(maxScale, scaleDuration)
                     .SetEase(Ease.InOutSine)
                     .SetLoops(-1, LoopType.Yoyo)
                     .OnKill(() => scaleTween = null);
        }
    }
    private void OnDestroy() {
        if (rotationTween != null) {
            rotationTween.Kill();
            rotationTween = null;
        }
        if (scaleTween != null) {
            scaleTween.Kill();
            scaleTween = null;
        }
    }
}