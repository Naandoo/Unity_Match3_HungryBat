using UnityEngine;
using UnityEngine.Tilemaps;
using FruitItem;
using ScriptableVariable;

namespace Board
{
    public class BoardGrid : MonoBehaviour
    {
        [SerializeField] private Tilemap _boardTilemap;
        [SerializeField] private BoardFruitPool _boardFruitPool;
        [SerializeField] private BoardListener _boardListener;
        [SerializeField] private Vector3Variable _boardCellSize;
        [SerializeField] private BoardMatcher _boardMatcher;
        private const float _fruitOffset = 0.25f;
        private Fruit[,] _boardFruitArray;

        public int Columns { get => _boardTilemap.size.x; private set { } }
        public int Rows { get => _boardTilemap.size.y; private set { } }
        public Fruit[,] BoardFruitArray { get => _boardFruitArray; private set { } }

        private void Start()
        {
            InitializePool();
            CreateBoard();
        }

        private void InitializePool()
        {
            int poolSize = _boardTilemap.size.x * _boardTilemap.size.y;
            _boardFruitPool.Initialize(poolSize);
        }

        private void CreateBoard()
        {
            _boardFruitArray = new Fruit[Columns, Rows];

            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    if (HasTileAt(j, i))
                    {
                        GenerateBoardFruit(j, i);
                    }
                }
            }

            _boardCellSize.Value = _boardTilemap.cellSize;
            _boardMatcher.TryMatchFruits(matchWithMovement: false);
        }

        public bool HasTileAt(int column, int row)
        {
            Vector3Int placement = GetCellPosition(column, row);
            return _boardTilemap.HasTile(placement);
        }

        private Vector3Int GetCellPosition(int column, int row)
        {
            Vector3Int cellPosition = new(_boardTilemap.cellBounds.x + column, _boardTilemap.cellBounds.y + row, 0);

            return cellPosition;
        }

        public void GenerateBoardFruit(int column, int row)
        {
            Fruit fruit = _boardFruitPool.GetRandomFruit();

            _boardFruitArray[column, row] = fruit;

            float offset = 5;
            Vector3 finalItemPosition = GetFruitPosition(column, row);
            Vector3 initialItemPosition = new(finalItemPosition.x, finalItemPosition.y + offset, finalItemPosition.z);
            fruit.transform.position = initialItemPosition;

            StartCoroutine(fruit.UpdatePosition(column, row, itemPosition: finalItemPosition));
            _boardListener.SubscribeEventsIn(fruit);
        }

        public Vector3 GetFruitPosition(int column, int row)
        {
            Vector3Int cellPosition = GetCellPosition(column, row);
            Vector3 offset = new(0, _fruitOffset, 0);
            Vector3 fruitPosition = _boardTilemap.CellToWorld(cellPosition) + _boardTilemap.tileAnchor + offset;

            return fruitPosition;
        }

        public void ReleaseFruit(int column, int row)
        {
            _boardFruitArray[column, row] = null;
        }
    }
}
