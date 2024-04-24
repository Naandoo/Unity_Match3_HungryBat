using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using ScriptableVariables;

namespace Game.UI
{
    public class UiAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform _moves;
        [SerializeField] private RectTransform _level;
        [SerializeField] private RectTransform _bat;
        [SerializeField] private RectTransform _goal;
        [SerializeField] private RectTransform _skills;
        [SerializeField] private RectTransform _starProgress;
        [SerializeField] private RectTransform _score;
        [SerializeField] private Slider _starSlider;
        [SerializeField] private Image[] _stars;
        [SerializeField] private Animator[] _starsProgressionAnimator;
        [SerializeField] private Transform _soundToggle;
        [SerializeField] private BoolVariable _soundAvailable;
        [SerializeField] private Button _restartButton;

        public IEnumerator InitializeLevelUI()
        {
            ResetStarsTransform();
            ResetSlider();
            _restartButton.interactable = false;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(AnimateMovesAppearing());
            sequence.Join(AnimateLevelAppearing());
            sequence.Append(AnimateStarProgressAppearing());
            sequence.Append(AnimateScoreAppearing());
            sequence.Append(AnimateBatAppearing());
            sequence.Append(AnimateGoalAppearing());
            sequence.Append(AnimateSkillsAppearing());

            sequence.Play();
            yield return sequence.WaitForCompletion();
            _restartButton.interactable = true;
        }

        private void ResetStarsTransform()
        {
            foreach (Image image in _stars)
            {
                image.transform.localScale = Vector3.zero;
            }
        }

        private void ResetSlider() => _starSlider.value = 0f;

        private Tween AnimateMovesAppearing()
        {
            Vector2 finalPosition = _moves.anchoredPosition;

            Vector2 positionToMoveFrom = new(finalPosition.x, Screen.height / 2);
            float duration = 0.5f;

            return MoveToPosition(_moves, positionToMoveFrom, duration);
        }

        private Tween AnimateLevelAppearing()
        {
            Vector2 finalPosition = _level.anchoredPosition;

            Vector2 positionToMoveFrom = new(finalPosition.x, Screen.height / 2);
            float duration = 0.5f;

            return MoveToPosition(_level, positionToMoveFrom, duration);
        }

        private Tween AnimateStarProgressAppearing()
        {
            Vector3 finalPosition = _starProgress.anchoredPosition;

            Vector3 positionToMoveFrom = new(finalPosition.x, Screen.height / 2);
            float duration = 0.5f;


            return MoveToPosition(_starProgress, positionToMoveFrom, duration);
        }

        private Tween AnimateScoreAppearing()
        {
            float duration = 0.25f;
            return PopAnimation(_score, duration);
        }

        private Tween AnimateBatAppearing()
        {
            Vector3 finalPosition = _bat.anchoredPosition;

            Vector3 positionToMoveFrom = new(Screen.width * -0.5f, finalPosition.y);
            float duration = 0.5f;

            return MoveToPosition(_bat, positionToMoveFrom, duration);
        }

        private Tween AnimateGoalAppearing()
        {
            float duration = 0.5f;
            return PopAnimation(_goal, duration);
        }

        private Tween AnimateSkillsAppearing()
        {
            Vector3 finalPosition = _skills.anchoredPosition;

            Vector3 positionToMoveFrom = new(Screen.width / 2, finalPosition.y, finalPosition.z);
            float duration = 0.25f;

            return MoveToPosition(_skills, positionToMoveFrom, duration);
        }

        private Tween PopAnimation(Transform transform, float duration, Ease ease = Ease.Linear)
        {
            Vector3 initialScale = transform.localScale;

            transform.localScale = new Vector3(0, 0, 0);
            return transform.DOScale(initialScale, duration).SetEase(ease);
        }

        private Tween MoveToPosition(RectTransform rectTransform, Vector3 positionToMoveFrom, float duration)
        {
            Vector2 finalPosition = rectTransform.anchoredPosition;

            rectTransform.anchoredPosition = positionToMoveFrom;

            return rectTransform.DOAnchorPos(finalPosition, duration);
        }

        public void AnimateSliderIncreasing(float currentValue)
        {
            float slidingTime = 0.25f;
            _starSlider.DOValue(currentValue, slidingTime);
        }

        public void AnimateStarAppearing(int levelStar)
        {
            int index = levelStar - 1;

            if (_starsProgressionAnimator[index].enabled == false)
            {
                _starsProgressionAnimator[index].enabled = true;
                _starsProgressionAnimator[index].Play("StarProgressionAppear", 0, 0);
            }
            else return;
        }

        public void AnimateSoundToggle()
        {
            if (_soundAvailable.Value)
            {
                _soundToggle.transform.DOLocalMove(new Vector3(-196f, 0f, 0f), 0.25f).SetEase(Ease.OutFlash).SetUpdate(true);
            }
            else
            {
                _soundToggle.transform.DOLocalMove(new Vector3(0, 0f, 0f), 0.25f).SetEase(Ease.OutFlash).SetUpdate(true);
            }
        }
    }
}
