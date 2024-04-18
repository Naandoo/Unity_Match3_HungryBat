using Controllers;
using Game.UI;
using LevelData;
using ScriptableVariables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIData : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelMoves;
    [SerializeField] private Image _firstGoalFruitIcon, _secondGoalFruitIcon, _thirdGoalFruitIcon;
    [SerializeField] private TMP_Text _firstGoalAmountText, _secondGoalAmountText, _thirdGoalAmountText;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Slider _starSlider;
    [SerializeField] private UiAnimation _UIAnimation;
    [SerializeField] private IntVariable _score;
    [SerializeField] private IntVariable _levelStars;
    [SerializeField] private IntVariable _moves;
    private Level currentLevel;
    public void UpdateUILevelData()
    {
        currentLevel = _levelManager.GetCurrentLevel();
        UpdateMoves(currentLevel);
        SetGoal(currentLevel);
        SetSliderMaxValue();
        ResetLevelStars();
        ResetScore();
    }

    private void UpdateMoves(Level currentLevel) => _moves.Value = currentLevel.Moves;

    private void SetGoal(Level currentLevel)
    {
        Goal firstGoalID = currentLevel.FirstGoal;
        UpdateGoalSprite(firstGoalID, _firstGoalFruitIcon);
        UpdateGoalText(firstGoalID);

        Goal secondGoalID = currentLevel.SecondGoal;
        UpdateGoalSprite(secondGoalID, _secondGoalFruitIcon);
        UpdateGoalText(secondGoalID);

        Goal thirdGoalID = currentLevel.ThirdGoal;
        UpdateGoalSprite(thirdGoalID, _thirdGoalFruitIcon);
        UpdateGoalText(thirdGoalID);
    }

    public void UpdateGoalSprite(Goal goal, Image goalIcon) => goalIcon.sprite = goal.FruitID.FruitSprite;

    public void UpdateGoalText(Goal goal)
    {
        switch (goal.ID)
        {
            case 1:
                _firstGoalAmountText.text = goal.Amount.ToString();
                break;
            case 2:
                _secondGoalAmountText.text = goal.Amount.ToString();
                break;
            case 3:
                _thirdGoalAmountText.text = goal.Amount.ToString();
                break;
            default:
                return;
        }
    }

    private void SetSliderMaxValue() => _starSlider.maxValue = _gameManager.GetHighestScore();
    private void ResetLevelStars() => _levelStars.Value = 0;
    private void ResetScore() => _score.Value = 0;

    public void UpdateScore(bool isGoalFruit)
    {
        if (isGoalFruit) _score.Value += GameManager.GoalFruitPoints;
        else _score.Value += GameManager.CommonFruitPoints;

        UpdateStarPercentage();
    }

    public void UpdateStarPercentage()
    {
        float starPercentage = _score.Value;
        _UIAnimation.AnimateSliderIncreasing(starPercentage);

        if (_starSlider.value >= _gameManager.FirstStarPercentage)
        {
            _levelStars.Value = 1;
            _UIAnimation.AnimateStarAppearing(levelStar: _levelStars.Value);
        }

        if (_starSlider.value >= _gameManager.SecondStarPercentage)
        {
            _levelStars.Value = 2;
            _UIAnimation.AnimateStarAppearing(levelStar: _levelStars.Value);
        }

        if (_starSlider.value >= _gameManager.ThirdStarPercentage)
        {
            _levelStars.Value = 3;
            _UIAnimation.AnimateStarAppearing(levelStar: _levelStars.Value);
        }
    }
}
