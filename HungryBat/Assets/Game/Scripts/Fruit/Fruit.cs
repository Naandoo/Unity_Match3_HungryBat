using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections;

namespace FruitItem
{
    public class Fruit : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private FruitID _fruitID;
        private int _column;
        private int _row;
        private const float _moveDuration = 0.3f;
        private Tween moveTween;

        public int Column { get => _column; private set { } }
        public int Row { get => _row; private set { } }
        public FruitID FruitID { get => _fruitID; private set { } }
        public ItemVanishEvent OnItemVanish = new();
        public ItemMovedEvent OnItemMoved = new();

        public void SetFruitID(FruitID fruitID)
        {
            this._fruitID = fruitID;
            UpdateVisual();
        }

        public IEnumerator UpdatePosition(int Column, int Row, Vector3 itemPosition)
        {
            _column = Column;
            _row = Row;

            yield return StartCoroutine(Move(itemPosition));
        }

        private IEnumerator Move(Vector3 itemPosition)
        {
            moveTween = transform.DOMove(itemPosition, _moveDuration);
            yield return moveTween.WaitForCompletion();
        }

        public void Vanish()
        {
            OnItemVanish?.Invoke(_column, _row);
        }

        private void UpdateVisual()
        {
            _spriteRenderer.sprite = _fruitID.Sprite;
        }
    }

    public class ItemVanishEvent : UnityEvent<int, int>
    {

    }

    public class ItemMovedEvent : UnityEvent<int, int, Direction>
    {

    }
}