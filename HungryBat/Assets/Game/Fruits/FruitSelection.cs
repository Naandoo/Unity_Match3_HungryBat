using UnityEngine;
using ScriptableVariables;
using Board;

namespace FruitItem
{
    public class FruitSelection : MonoBehaviour
    {
        [SerializeField] private Fruit _fruit;
        [SerializeField] private Vector3Variable _cellBoardSize;
        [SerializeField] private IntVariable _movesAmount;
        [SerializeField] private BoolVariable _levelFinished;
        [SerializeField] private BoolVariable _gamePaused;
        private Vector3 _inputInitialPosition;
        private Vector3 _inputFinalPosition;
        private bool _selected;

        private void OnMouseDown()
        {
            if (!AbleToInteract()) return;

            _selected = true;
            _fruit.onSelectedFruit?.Invoke(_fruit);
            _inputInitialPosition = GetWorldMousePosition();
        }

        private void OnMouseDrag()
        {
            if (!AbleToInteract()) return;

            if (_selected)
            {
                _inputFinalPosition = GetWorldMousePosition();

                if (ReachedMinimumDistance())
                {
                    TryMoveFruit();
                }
            }
        }

        private bool AbleToInteract()
        {
            bool ableToInteract = BoardState.Instance.State == State.Common
            && _movesAmount.Value > 0 && !_levelFinished.Value && !_gamePaused.Value;

            return ableToInteract;
        }
        private Vector3 GetWorldMousePosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.transform.position.z;

            return Camera.main.ScreenToWorldPoint(mousePosition);
        }

        private bool ReachedMinimumDistance()
        {
            float inputDistance = Vector3.Distance(_inputInitialPosition, _inputFinalPosition);

            float squareWidth = _cellBoardSize.Value.x * _cellBoardSize.Value.y;
            float minDistanceToMove = squareWidth / 2;

            if (inputDistance >= minDistanceToMove) return true;
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
            Vector3 direction = finalInput - initialInput;
            float angleRad = Mathf.Atan2(direction.y, direction.x);
            float angleDeg = angleRad * Mathf.Rad2Deg;

            if (angleDeg < 0) angleDeg += 360;

            if (angleDeg >= 60 && angleDeg <= 120)
            {
                return Direction.Up;
            }
            if (angleDeg >= 150 && angleDeg <= 210)
            {
                return Direction.Left;
            }
            if (angleDeg >= 240 && angleDeg <= 300)
            {
                return Direction.Down;
            }
            if (angleDeg >= 330 || angleDeg <= 30)
            {
                return Direction.Right;
            }

            return Direction.Undefined;
        }
    }
}