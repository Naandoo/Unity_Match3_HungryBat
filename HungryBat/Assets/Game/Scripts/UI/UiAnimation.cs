using UnityEngine;
using DG.Tweening;

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

        private void Start()
        {
            InitializeLevelAnimations();
        }

        public void InitializeLevelAnimations()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(AnimateMoves());
            sequence.Append(AnimateStarProgress());
            sequence.Append(AnimateScore());
            sequence.Append(AnimateBat());
            sequence.Append(AnimateGoal());
            sequence.Append(AnimateSkills());

            sequence.Play();
        }

        private Tween AnimateMoves()
        {
            Vector3 finalPosition = _moves.localPosition;
            int initialDistance = 200;

            Vector3 initialPosition = new(finalPosition.x, finalPosition.y + initialDistance, finalPosition.z);
            float duration = 0.5f;


            return MoveToPosition(_moves, initialPosition, finalPosition, duration);
        }

        private Tween AnimateStarProgress()
        {
            Vector3 finalPosition = _starProgress.localPosition;
            int initialDistance = 100;

            Vector3 initialPosition = new(finalPosition.x, finalPosition.y + initialDistance, finalPosition.z);
            float duration = 0.5f;


            return MoveToPosition(_starProgress, initialPosition, finalPosition, duration);
        }

        private Tween AnimateScore()
        {
            float duration = 0.25f;
            return PopAnimation(_score, duration);
        }

        private Tween AnimateBat()
        {
            Vector3 finalPosition = _bat.localPosition;
            int initialDistance = 250;

            Vector3 initialPosition = new(finalPosition.x - initialDistance, finalPosition.y, finalPosition.z);
            float duration = 0.5f;

            return MoveToPosition(_bat, initialPosition, finalPosition, duration);
        }

        private Tween AnimateGoal()
        {
            float duration = 0.5f;
            return PopAnimation(_goal, duration);
        }

        private Tween AnimateSkills()
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
    }
}
