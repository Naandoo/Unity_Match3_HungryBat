using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardFiller : MonoBehaviour
{
    [SerializeField] private Tilemap _boardTilemap;
    [SerializeField] private BoardItemPool _boardItemPool;
    [SerializeField] private BoardListener _boardListener;
    private const float _itemOffset = 0.25f;
    private BoardItem[,] _boardItemsArray;
    public int Rows { get => _boardTilemap.size.x; private set { } }
    public int Columns { get => _boardTilemap.size.y; private set { } }
    public BoardItem[,] BoardItemsArray { get => _boardItemsArray; private set { } }

    private void Start()
    {
        InitializePool();
        CreateBoard();
    }

    private void InitializePool()
    {
        int poolSize = _boardTilemap.size.x * _boardTilemap.size.y;
        _boardItemPool.Initialize(poolSize);
    }

    private void CreateBoard()
    {
        int rows = _boardTilemap.size.x;
        int columns = _boardTilemap.size.y;
        _boardItemsArray = new BoardItem[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3Int cellPosition = GetCellPosition(i, j);
                if (_boardTilemap.HasTile(cellPosition))
                {
                    GenerateBoardItem(i, j);
                }
            }
        }
    }

    private Vector3Int GetCellPosition(int row, int column)
    {
        Vector3Int cellPosition = new(_boardTilemap.cellBounds.x + row, _boardTilemap.cellBounds.y + column, 0);

        return cellPosition;
    }

    private void GenerateBoardItem(int row, int column)
    {
        BoardItem item = _boardItemPool.PoolSystem.Get();

        _boardItemsArray[row, column] = item;

        item.UpdatePosition(row, column, itemPosition: GetItemPosition(row, column));
        _boardListener.SubscribeEventsIn(item);
    }

    public Vector3 GetItemPosition(int row, int column)
    {
        Vector3Int cellPosition = GetCellPosition(row, column);
        Vector3 offset = new(0, _itemOffset, 0);
        Vector3 itemPosition = _boardTilemap.CellToWorld(cellPosition) + _boardTilemap.tileAnchor + offset;

        return itemPosition;
    }

    public void CheckEmptyStartingAt(int row, int column)
    {
        for (int i = row; i < Rows; i++)
        {
            if (IsEmptyBoardItem(i, column))
            {
                GenerateBoardItem(row: i, column);
            }
        }
    }

    public void ReleaseItem(int row, int column)
    {
        _boardListener.UnsubscribeEvents(_boardItemsArray[row, column]);
        _boardItemsArray[row, column] = null;

    }

    public bool IsEmptyBoardItem(int row, int column)
    {
        bool IsEmpty = _boardItemsArray[row, column] == null;
        return IsEmpty;
    }

    public void UpdateItemPosition(BoardItem boardItem, int newRow, int newColumn, Vector3 itemPosition)
    {
        int oldRow = boardItem.Row;
        int oldColumn = boardItem.Column;

        _boardItemsArray[newRow, newColumn] = boardItem;
        boardItem.UpdatePosition(newRow, newColumn, itemPosition);

        ReleaseItem(oldRow, oldColumn);
    }
}
