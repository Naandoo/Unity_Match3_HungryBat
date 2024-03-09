using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class BoardItem : MonoBehaviour
{
    private int _column;
    private int _row;
    public int Column { get => _column; set { } }
    public int Row { get => _row; set { } }
    private const float moveDuration = 0.5f;
    public ItemVanishEvent OnItemVanish = new();

    private void OnMouseDown()
    {
        Vanish();
    }

    public void UpdatePosition(int Column, int Row, Vector3 itemPosition)
    {
        _column = Column;
        _row = Row;

        Move(itemPosition);
    }

    private void Move(Vector3 itemPosition)
    {
        transform.DOMove(itemPosition, moveDuration);
    }


    public void Vanish()
    {
        OnItemVanish?.Invoke(_column, _row);
    }
}

public class ItemVanishEvent : UnityEvent<int, int>
{

}