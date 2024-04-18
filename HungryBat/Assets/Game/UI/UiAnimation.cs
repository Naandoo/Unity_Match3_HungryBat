using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using ScriptableVariables;

namespace Game.UI
{
    public class UiAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _moves;
        [SerializeField] private Transform _bat;
        [SerializeField] private Transform _goal;
        [SerializeField] private Transform _skills;
        [SerializeField] private Transform _starProgress;
        [SerializeField] private Transform _score;
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
            Vector3 finalPosition = _moves.localPosition;
            int initialDistance = 200;

            Vector3 initialPosition = new(finalPosition.x, finalPosition.y + initialDistance, finalPosition.z);
            float duration = 0.5f;


            return MoveToPosition(_moves, initialPosition, finalPosition, duration);
        }

        private Tween AnimateStarProgressAppearing()
        {
            Vector3 finalPosition = _starProgress.localPosition;
            int initialDistance = 100;

            Vector3 initialPosition = new(finalPosition.x, finalPosition.y + initialDistance, finalPosition.z);
            float duration = 0.5f;


            return MoveToPosition(_starProgress, initialPosition, finalPosition, duration);
        }

        private Tween AnimateScoreAppearing()
        {
            float duration = 0.25f;
            return PopAnimation(_score, duration);
        }

        private Tween AnimateBatAppearing()
        {
            Vector3 finalPosition = _bat.localPosition;
            int initialDistance = 250;

            Vector3 initialPosition = new(finalPosition.x - initialDistance, finalPosition.y, finalPosition.z);
            float duration = 0.5f;

            return MoveToPosition(_bat, initialPosition, finalPosition, duration);
        }

        private Tween AnimateGoalAppearing()
        {
            float duration = 0.5f;
            return PopAnimation(_goal, duration);
        }

        private Tween AnimateSkillsAppearing()
        {
            Vector3 finalPosition = _skills.localPosition;
            int initialDistance = 600;

            Vector3 initialPosition = new(finalPosition.x + initialDistance, finalPosition.y, finalPosition.z);
            float duration = 0.25f;

            return MoveToPosition(_skills, initialPosition, finalPosition, duration);
        }

        private Tween PopAnimation(Transform transform, float duration, Ease ease = Ease.Linear)
        {
            Vector3 initialScale = transform.localScale;

            transform.localScale = new Vector3(0, 0, 0);
            return transform.DOScale(initialScale, duration).SetEase(ease);
        }

        private Tween MoveToPosition(Transform transform, Vector3 initialPosition, Vector3 finalPosition, float duration)
        {
            transform.localPosition = initialPosition;
            return transform.DOLocalMove(finalPosition, duration);
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
