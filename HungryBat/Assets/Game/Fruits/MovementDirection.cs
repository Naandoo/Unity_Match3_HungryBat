using UnityEngine;

namespace FruitItem
{
    public static class MovementDirection
    {
        public static Vector2Int GetDirectionCoordinates(Direction direction)
        {
            Vector2Int finalDirection = Vector2Int.zero;

            switch (direction)
            {
                case Direction.Up:
                    finalDirection = new(0, 1);
                    break;
                case Direction.Down:
                    finalDirection = new(0, -1);
                    break;
                case Direction.Left:
                    finalDirection = new(-1, 0);
                    break;
                case Direction.Right:
                    finalDirection = new(1, 0);
                    break;
                case Direction.Undefined:
                    finalDirection = Vector2Int.zero;
                    break;
            }

            return finalDirection;
        }
    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Undefined,
    }
}