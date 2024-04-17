using UnityEngine;
using Controllers;

namespace Game.UI
{
    public class DefeatPopupFeedbacks : MonoBehaviour
    {
        [SerializeField] private AudioClip _onDefeatSFX;

        private void Start()
        {
            GameEvents.Instance.OnLoseEvent.AddListener(() =>
            {
                TriggerFeedbacks();
            });
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnLoseEvent.RemoveListener(() =>
            {
                TriggerFeedbacks();
            });
        }

        private void TriggerFeedbacks()
        {
            GameSounds.Instance.SwitchBackgroundMusic(audioClip: _onDefeatSFX, loopEnabled: false);
        }
    }
}
