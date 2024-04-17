using System;
using System.Collections;
using Controllers;
using ScriptableVariables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class VictoryPopupFeedbacks : MonoBehaviour
    {
        [SerializeField] private AudioClip _onCompleteSFX;
        [SerializeField] private IntVariable _scorePoints;
        [SerializeField] private TMP_Text _textPoints;
        [SerializeField] private float _secondsDelayToIncreaseText;
        [SerializeField] private float _secondsDelayToShowStars;
        [SerializeField] private Animator[] _starsAnimator;
        [SerializeField] private IntVariable _starsEarned;

        private void Start()
        {
            GameEvents.Instance.OnWinEvent.AddListener(() =>
            {
                TriggerFeedbacks();
            });
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnWinEvent.RemoveListener(() =>
            {
                TriggerFeedbacks();
            });
        }

        private void TriggerFeedbacks()
        {
            GameSounds.Instance.SwitchBackgroundMusic(audioClip: _onCompleteSFX, loopEnabled: false);
            StartCoroutine(UpdateScoreText());
            StartCoroutine(TriggerStarsAnimation());
        }

        private IEnumerator UpdateScoreText()
        {
            int currentScore = 0;
            _textPoints.text = currentScore.ToString();

            for (int i = 0; i < _scorePoints.Value; i++)
            {
                currentScore = Math.Min(currentScore + 5, _scorePoints.Value);
                _textPoints.text = currentScore.ToString();
                yield return new WaitForSeconds(_secondsDelayToIncreaseText);
            }
        }

        private IEnumerator TriggerStarsAnimation()
        {
            yield return new WaitForSeconds(_secondsDelayToShowStars);
            for (int i = 0; i < _starsEarned.Value; i++)
            {
                _starsAnimator[i].enabled = true;
                _starsAnimator[i].Play("StarAppearance", 0, 0);
                yield return new WaitForSeconds(_secondsDelayToShowStars);
            }
        }
    }
}
