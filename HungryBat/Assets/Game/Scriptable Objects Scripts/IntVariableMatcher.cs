using UnityEngine;
using UnityEngine.Events;

namespace ScriptableVariables
{
    public class IntVariableMatcher : MonoBehaviour
    {
        [SerializeField] private IntVariable _valueToCompare;
        [SerializeField] private int _referenceAmount;
        [SerializeField] private MatchRule comparisonRule;
        [SerializeField] private UnityEvent _onMatch, _onMismatch;

        public void OnEnable()
        {
            _valueToCompare.OnValueChanged += CompareValue;
            CompareValue(_valueToCompare.Value);
        }
        private void OnDisable()
        {
            _valueToCompare.OnValueChanged -= CompareValue;
        }

        public void CompareValue(int value)
        {
            bool isMatch = false;
            switch (comparisonRule)
            {
                case MatchRule.Smaller:
                    isMatch = value < _referenceAmount;
                    break;
                case MatchRule.SmallerOrEqual:
                    isMatch = value <= _referenceAmount;
                    break;
                case MatchRule.Equal:
                    isMatch = value == _referenceAmount;
                    break;
                case MatchRule.EqualOrBigger:
                    isMatch = value >= _referenceAmount;
                    break;
                case MatchRule.Bigger:
                    isMatch = value > _referenceAmount;
                    break;
            }

            if (isMatch) _onMatch.Invoke();
            else _onMismatch.Invoke();
        }


    }

    public enum MatchRule
    {
        Smaller,
        SmallerOrEqual,
        Equal,
        EqualOrBigger,
        Bigger,
    }
}