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
        public void TriggerPause()
        {
            _paused.Value = !_paused.Value;

            Time.timeScale = _paused.Value ? 0 : 1;
            _pauseScreen.enabled = _paused.Value;

        }

        public void TriggerSound() => _soundAvailable.Value = !_soundAvailable.Value;
    }
}
