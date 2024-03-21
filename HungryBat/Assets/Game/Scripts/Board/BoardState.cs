using UnityEngine;
namespace Board
{
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
}