using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ScriptableVariables
{
    [Serializable]
    public class ScriptableVariable<T> : ScriptableObject
    {
        [SerializeField] protected T _value;
        [SerializeField] protected T _initialValue;
        private readonly List<Object> _listenersObjects = new List<Object>();
        private Action<T> _onValueChanged;
        public event Action<T> OnValueChanged
        {
            add
            {
                _onValueChanged += value;

                Object listener = value.Target as Object;
                if (listener != null && !_listenersObjects.Contains(listener))
                {
                    _listenersObjects.Add(listener);
                }
            }
            remove
            {
                _onValueChanged -= value;

                Object listener = value.Target as Object;
                if (_listenersObjects.Contains(listener))
                {
                    _listenersObjects.Remove(listener);
                }
            }
        }

        public T PreviousValue { get; private set; }
        public virtual T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value)) return;
                _value = value;
                ValueChanged();
            }
        }

        private void ValueChanged()
        {
            _onValueChanged?.Invoke(_value);
            PreviousValue = _value;
        }

        public void Reset()
        {
            _listenersObjects.Clear();
            ResetToInitialValue();
        }

        public void ResetToInitialValue()
        {
            Value = _initialValue;
            PreviousValue = _initialValue;
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (Equals(_value, PreviousValue)) return;
            ValueChanged();
        }
#endif
    }
}