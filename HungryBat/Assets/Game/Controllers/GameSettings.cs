using DG.Tweening;
using Game.UI;
using ScriptableVariables;
using UnityEngine;

namespace Controllers
{
    public class GameSettings : MonoBehaviour
    {
        [SerializeField] private BoolVariable _soundAvailable;
        [SerializeField] private UiAnimation _uiAnimation;
        [SerializeField] private BoolVariable _paused;
        [SerializeField] private Canvas _pauseScreen;
        [SerializeField] PopupHandler _popupHandler;

        private void Start()
        {
            _paused.Value = false;
        }

        public void TriggerPause()
        {
            _paused.Value = !_paused.Value;

            if (_paused.Value)
            {
                _popupHandler.EnablePopup(_pauseScreen).OnComplete(() =>
                {
                    TriggerTimeScale();
                });


            }
            else
            {
                _popupHandler.DisablePopup(_pauseScreen).OnComplete(() =>
                {
                    TriggerTimeScale();
                });
            }
        }

        private void TriggerTimeScale() => Time.timeScale = _paused.Value ? 0 : 1;
        public void TriggerSound() => _soundAvailable.Value = !_soundAvailable.Value;
    }
}
