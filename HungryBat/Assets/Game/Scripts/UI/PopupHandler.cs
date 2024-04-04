using UnityEngine;

public class PopupHandler : MonoBehaviour
{
    [SerializeField] private Canvas _winPopup, _losePopup;

    private void Start()
    {
        GameEvents.Instance.OnWinEvent.AddListener(() =>
        {
            ActivatePopup(_winPopup);
        });

        GameEvents.Instance.OnLoseEvent.AddListener(() =>
        {
            ActivatePopup(_losePopup);
        });
    }

    private void OnDestroy()
    {
        GameEvents.Instance.OnWinEvent.RemoveListener(() =>
        {
            ActivatePopup(_winPopup);
        });

        GameEvents.Instance.OnLoseEvent.RemoveListener(() =>
        {
            ActivatePopup(_losePopup);
        });
    }

    private void ActivatePopup(Canvas canvas) => canvas.enabled = true;
}
