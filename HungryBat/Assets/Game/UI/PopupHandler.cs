using UnityEngine;
using Controllers;
using DG.Tweening;
using UnityEngine.UI;

public class PopupHandler : MonoBehaviour
{
    [SerializeField] private Canvas _winPopup, _losePopup;
    [SerializeField] private float _openPopupDuration;
    [SerializeField] private float _closePopupDuration;
    [SerializeField] private Image _overlayBackground;
    private void Start()
    {
        GameEvents.Instance.OnWinEvent.AddListener(() =>
        {
            EnablePopup(_winPopup);
        });

        GameEvents.Instance.OnLoseEvent.AddListener(() =>
        {
            EnablePopup(_losePopup);
        });
    }

    private void OnDestroy()
    {
        GameEvents.Instance.OnWinEvent.RemoveListener(() =>
        {
            EnablePopup(_winPopup);
        });

        GameEvents.Instance.OnLoseEvent.RemoveListener(() =>
        {
            EnablePopup(_losePopup);
        });
    }

    public void EnablePopUpAction(Canvas canvas) => EnablePopUpAction(canvas);
    public void DisablePopupAction(Canvas canvas) => DisablePopup(canvas);

    public Tween EnablePopup(Canvas canvas)
    {
        _overlayBackground.enabled = true;

        Tween tween = canvas.transform.DOScale(Vector3.one, _openPopupDuration)
        .SetEase(Ease.OutBack)
        .SetUpdate(isIndependentUpdate: true)
        .OnStart(() =>
        {
            canvas.enabled = true;
        });
        return tween;
    }

    public Tween DisablePopup(Canvas canvas)
    {
        _overlayBackground.enabled = false;

        Tween tween = canvas.transform.DOScale(Vector3.zero, _closePopupDuration)
        .SetEase(Ease.InBack)
        .SetUpdate(isIndependentUpdate: true)
        .OnComplete(() =>
        {
            canvas.enabled = false;
        });
        return tween;
    }
}
