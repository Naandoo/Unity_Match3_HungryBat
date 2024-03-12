using UnityEngine;
using ScriptableVariable;
using UnityEngine.Events;
using Board;

namespace BoardItem
{
    public class FruitMovement : MonoBehaviour
    {
        [SerializeField] private Fruit _fruit;
        [SerializeField] private Vector3Variable _fruitSize;
        private Vector3 _inputInitialPosition;
        private Vector3 _inputFinalPosition;
        private Vector3 _minDistanceToMove => _fruitSize.Value + (_fruitSize.Value / 2);
        private bool _selected;

        private void OnMouseDown()
        {
            _selected = true;
            _inputInitialPosition = Input.mousePosition;
        }

        private void OnMouseDrag()
        {
            if (_selected)
            {
                _inputFinalPosition = Input.mousePosition;

                if (ReachedMinimumDistance())
                {
                    TryMoveFruit();
                }
            }
        }

        private bool ReachedMinimumDistance()
        {
            float inputDistance = _inputFinalPosition.magnitude - _inputInitialPosition.magnitude;
            if (inputDistance >= _minDistanceToMove.magnitude)
            {
                return true;
            }
            else return false;
        }

        private void TryMoveFruit()
        {
            Direction movementDirection = GetDirection(_inputInitialPosition, _inputFinalPosition);
            if (movementDirection == Direction.Undefined) return;
            else
            {
                int column = _fruit.Column;
                int row = _fruit.Row;

                _fruit.OnItemMoved?.Invoke(column, row, movementDirection);
                _selected = false;
            }
        }


        private void OnMouseUp()
        {
            _selected = false;
        }

        private Direction GetDirection(Vector3 initialInput, Vector3 finalInput)
        {
            float angle = Vector3.Angle(initialInput, finalInput);

            if (angle >= 60 && angle <= 120) return Direction.Up;
            if (angle >= 150 && angle <= 210) return Direction.Left;
            if (angle >= 240 && angle <= 300) return Direction.Down;
            if (angle >= 330 && angle <= 30) return Direction.Right;

            return Direction.Undefined;
        }
    }


}