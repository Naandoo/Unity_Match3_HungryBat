using UnityEngine;

public class BoardState : MonoBehaviour
{
    public State State { get; set; }

}

public enum State
{
    Moving,
    Sorting,
    Common,
}