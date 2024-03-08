using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class BoardItem : MonoBehaviour
{
    private int _row;
    private int _column;
    public int Row { get => _row; set { } }
    public int Column { get => _column; set { } }
    private const float moveDuration = 0.5f;
    public ItemVanishEvent OnItemVanish = new();

    private void OnMouseDown()
    {
        Debug.Log(_row + " " + _column);
        // Vanish();
    }

    public void UpdatePosition(int Row, int Column, Vector3 itemPosition)
    {
        _row = Row;
        _column = Column;

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