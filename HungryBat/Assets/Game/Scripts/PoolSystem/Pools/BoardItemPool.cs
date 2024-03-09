using UnityEngine;

public class BoardItemPool : MonoBehaviour
{
    [SerializeField] private BoardItem _boardItem;
    [SerializeField] private BoardListener _boardListener;
    private PoolSystem<BoardItem> _poolSystem;
    public PoolSystem<BoardItem> PoolSystem { get => _poolSystem; set { } }

    public void Initialize(int poolSize)
    {
        _poolSystem = new PoolSystem<BoardItem>(_boardItem, poolSize, transform);
    }

    public void OnReleasedItem(BoardItem boardItem)
    {
        _boardListener.UnsubscribeEventsIn(boardItem);
        _poolSystem.Return(boardItem);
    }
}