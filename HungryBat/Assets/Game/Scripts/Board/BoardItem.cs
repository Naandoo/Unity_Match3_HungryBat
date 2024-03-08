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
        Debug.Log(_row + " " + _column);
        Vanish();
    }

    public void UpdatePosition(int Column, int Row, Vector3 itemPosition)
    {
        _column = Column;
        _row = Row;

        Move(itemPosition);
        // Debug.Log(_row + " " + _column);
    }

    private void Move(Vector3 itemPosition)
    {
        transform.DOMove(itemPosition, moveDuration);
    }


    public void Vanish()
    {
        OnItemVanish?.Invoke(_row, _column);
    }
}

public class ItemVanishEvent : UnityEvent<int, int>
{

}