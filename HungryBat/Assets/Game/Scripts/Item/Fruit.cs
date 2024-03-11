using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace BoardItem
{
    public class Fruit : MonoBehaviour
    {
        private FruitID _fruitID;
        private int _column;
        private int _row;
        private const float _moveDuration = 0.5f;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public int Column { get => _column; private set { } }
        public int Row { get => _row; private set { } }
        public FruitID FruitID { get => _fruitID; private set { } }
        public ItemVanishEvent OnItemVanish = new();

        private void OnMouseDown()
        {
            Vanish();
        }

        public void SetFruitID(FruitID fruitID)
        {
            this._fruitID = fruitID;
            UpdateVisual();
        }

        public void UpdatePosition(int Column, int Row, Vector3 itemPosition)
        {
            _column = Column;
            _row = Row;

            Move(itemPosition);
        }

        private void Move(Vector3 itemPosition)
        {
            transform.DOMove(itemPosition, _moveDuration);
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
}