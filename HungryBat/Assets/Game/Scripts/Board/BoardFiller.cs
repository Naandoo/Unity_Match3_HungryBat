using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardFiller : MonoBehaviour
{
    [SerializeField] private Tilemap _boardTilemap;
    [SerializeField] private BoardItemPool _boardItemPool;
    [SerializeField] private BoardListener _boardListener;
    private const float _itemOffset = 0.25f;
    private BoardItem[,] _boardItemsArray;
    public int Columns { get => _boardTilemap.size.x; private set { } }
    public int Rows { get => _boardTilemap.size.y; private set { } }
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
        _boardItemsArray = new BoardItem[Columns, Rows];

        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                if (HasTileAt(j, i))
                {
                    GenerateBoardItem(j, i);
                }
            }
        }
    }

    private Vector3Int GetCellPosition(int column, int row)
    {
        Vector3Int cellPosition = new(_boardTilemap.cellBounds.x + column, _boardTilemap.cellBounds.y + row, 0);

        return cellPosition;
    }

    private void GenerateBoardItem(int column, int row)
    {
        BoardItem item = _boardItemPool.PoolSystem.Get();

        _boardItemsArray[column, row] = item;

        float offset = 5;
        Vector3 finalItemPosition = GetItemPosition(column, row);
        Vector3 initialItemPosition = new(finalItemPosition.x, finalItemPosition.y + offset, finalItemPosition.z);
        item.transform.position = initialItemPosition;

        item.UpdatePosition(column, row, itemPosition: finalItemPosition);
        _boardListener.SubscribeEventsIn(item);
    }

    public Vector3 GetItemPosition(int column, int row)
    {
        Vector3Int cellPosition = GetCellPosition(column, row);
        Vector3 offset = new(0, _itemOffset, 0);
        Vector3 itemPosition = _boardTilemap.CellToWorld(cellPosition) + _boardTilemap.tileAnchor + offset;

        return itemPosition;
    }

    public void CheckEmptyStartingAt(int column, int row)
    {
        for (int i = row; i < Rows; i++)
        {
            if (IsEmptyBoardItem(column, row))
            {
                GenerateBoardItem(column, row: i);
            }
        }
    }

    public void ReleaseItem(int column, int row)
    {
        _boardItemsArray[column, row] = null;
    }

    public bool IsEmptyBoardItem(int column, int row)
    {
        bool IsEmpty = _boardItemsArray[column, row] == null;
        return IsEmpty;
    }

    public void UpdateItemPosition(BoardItem boardItem, int newColumn, int newRow, Vector3 itemPosition)
    {
        int oldColumn = boardItem.Column;
        int oldRow = boardItem.Row;

        _boardItemsArray[newColumn, newRow] = boardItem;
        boardItem.UpdatePosition(newColumn, newRow, itemPosition);

        ReleaseItem(oldColumn, oldRow);
    }

    public bool HasTileAt(int column, int row)
    {
        Vector3Int placement = GetCellPosition(column, row);
        return _boardTilemap.HasTile(placement);
    }
}
