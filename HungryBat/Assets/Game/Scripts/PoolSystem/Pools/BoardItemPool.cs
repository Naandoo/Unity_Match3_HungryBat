using UnityEngine;

public class BoardItemPool : MonoBehaviour
{
    [SerializeField] private BoardItem _boardItem;
    private PoolSystem<BoardItem> _poolSystem;
    public PoolSystem<BoardItem> PoolSystem { get => _poolSystem; set { } }

    public void Initialize(int poolSize)
    {
        _poolSystem = new PoolSystem<BoardItem>(_boardItem, poolSize, transform);
    }

    public void OnReleasedItem(BoardItem boardItem)
    {
        _poolSystem.Return(boardItem);
    }
}