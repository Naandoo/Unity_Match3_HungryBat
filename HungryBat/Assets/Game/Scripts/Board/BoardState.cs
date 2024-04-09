using UnityEngine;
using UnityEngine.Events;
namespace Board
{
    public class BoardState : MonoBehaviour
    {
        private BoardState() { }
        public static BoardState Instance { get; private set; }
        public State State { get; private set; }
        public readonly StateChangedEvent onStateChange = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            DontDestroyOnLoad(gameObject);
        }

        public void SetState(State state)
        {
            State = state;
            onStateChange.Invoke(state);
        }

        private void OnDestroy()
        {
            onStateChange.RemoveAllListeners();
        }
    }

    public enum State
    {
        Moving,
        Sorting,
        Common,
    }

    public class StateChangedEvent : UnityEvent<State> { }
}