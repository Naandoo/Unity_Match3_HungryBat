using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections;
using Board;

namespace FruitItem
{
    public class Fruit : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private FruitID _fruitID;
        private int _column;
        private int _row;
        private const float _moveDuration = 0.3f;
        private const float _secondsToStartTip = 1f;
        private readonly WaitForSeconds tipSeconds = new(_secondsToStartTip);
        private Vector3 _initialScale;
        private Tween tweenTip;

        public int Column { get => _column; private set { } }
        public int Row { get => _row; private set { } }
        public FruitID FruitID { get => _fruitID; private set { } }
        public ItemVanishEvent OnItemVanish = new();
        public ItemMovedEvent OnItemMoved = new();

        private void OnEnable()
        {
            _initialScale = transform.localScale;
            BoardState.Instance.AddListenerOnStateChange(FinishTipRoutine);
        }

        public void FinishTipRoutine()
        {
            if (BoardState.Instance.State != State.Common)
            {
                KillTipRoutine();
            }
        }

        private void KillTipRoutine()
        {
            StopCoroutine(Tip());
            transform.localScale = _initialScale;
            tweenTip.Kill();
        }

        private void OnDisable()
        {
            KillTipRoutine();
        }

        public void SetFruitID(FruitID fruitID)
        {
            this._fruitID = fruitID;
            UpdateVisual();
        }

        private void UpdateVisual() => _spriteRenderer.sprite = _fruitID.Sprite;

        public IEnumerator UpdatePosition(int Column, int Row, Vector3 itemPosition)
        {
            _column = Column;
            _row = Row;

            yield return StartCoroutine(Move(itemPosition));
        }

        private IEnumerator Move(Vector3 itemPosition)
        {
            Tween moveTween = transform.DOMove(itemPosition, _moveDuration);
            yield return moveTween.WaitForCompletion();
        }

        public void Vanish() => OnItemVanish?.Invoke(_column, _row);

        public IEnumerator Tip()
        {
            yield return tipSeconds;
            tweenTip = transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.7f).SetLoops(-1, LoopType.Yoyo);

        }

    }

    public class ItemVanishEvent : UnityEvent<int, int> { }

    public class ItemMovedEvent : UnityEvent<int, int, Direction> { }
}