using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections;
using Board;
using ScriptableVariables;
using Effects;

namespace FruitItem
{
    public class Fruit : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private FloatVariable _fruitMovementTime;
        private FruitID _fruitID;
        private int _column;
        private int _row;
        private Vector3 _initialScale = Vector3.one;
        private Tween tweenTip;

        public int Column { get => _column; private set { } }
        public int Row { get => _row; private set { } }
        public FruitID FruitID { get => _fruitID; private set { } }
        public ItemVanishEvent OnItemVanish = new();
        public ItemMovedEvent OnItemMoved = new();
        public SelectedFruit onSelectedFruit = new();


        public void EndTipRoutine()
        {
            if (tweenTip.IsActive())
            {
                tweenTip.Kill();
                transform.localScale = _initialScale;
            }
        }

        public void SetFruitID(FruitID fruitID)
        {
            this._fruitID = fruitID;
            UpdateVisual();
        }

        private void UpdateVisual() => _spriteRenderer.sprite = _fruitID.FruitSprite;

        public IEnumerator UpdatePosition(int Column, int Row, Vector3 itemPosition)
        {
            _column = Column;
            _row = Row;

            yield return StartCoroutine(Move(itemPosition));
        }

        private IEnumerator Move(Vector3 itemPosition)
        {
            Tween moveTween = transform.DOMove(itemPosition, _fruitMovementTime.Value);
            yield return moveTween.WaitForCompletion();
        }

        public void Vanish()
        {
            FruitEffects.Instance.PlayExplosionParticle(transform.position, FruitID.FruitType);
            EndTipRoutine();
            OnItemVanish?.Invoke(_column, _row);
        }

        public void Tip()
        {
            if (BoardState.Instance.State != State.Common) return;
            tweenTip = transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.7f).SetLoops(-1, LoopType.Yoyo);
        }

    }

    public class ItemVanishEvent : UnityEvent<int, int> { }

    public class ItemMovedEvent : UnityEvent<int, int, Direction> { }

    public class SelectedFruit : UnityEvent<Fruit> { }

}

