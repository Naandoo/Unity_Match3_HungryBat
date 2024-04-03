using Controllers;
using Game.UI;
using LevelData;
using ScriptableVariable;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class LevelUIData : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelMoves;
    [SerializeField] private Image _firstGoalFruitIcon, _secondGoalFruitIcon, _thirdGoalFruitIcon;
    [SerializeField] private TMP_Text _firstGoalAmount, _secondGoalAmount, _thirdGoalAmount;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Slider _starSlider;
    [SerializeField] private UiAnimation _UIAnimation;
    [SerializeField] private IntVariable _score;
    [SerializeField] private IntVariable _levelStars;
    [SerializeField] private IntVariable _moves;

    public void UpdateUILevelData()
    {
        Level currentLevel = _levelManager.GetCurrentLevel();
        UpdateMoves(currentLevel);
        UpdateGoal(currentLevel);
        SetSliderMaxValue();
        ResetLevelStars();
        ResetScore();
    }

    private void UpdateMoves(Level currentLevel) => _moves.Value = currentLevel.Moves;
    private void UpdateGoal(Level currentLevel)
    {
        Goal firstGoalID = currentLevel.FirstGoal;
        _firstGoalFruitIcon.sprite = firstGoalID.FruitID.FruitSprite;
        _firstGoalAmount.text = firstGoalID.Amount.ToString();

        Goal secondGoalID = currentLevel.SecondGoal;
        _secondGoalFruitIcon.sprite = secondGoalID.FruitID.FruitSprite;
        _secondGoalAmount.text = secondGoalID.Amount.ToString();

        Goal thirdGoalID = currentLevel.ThirdGoal;
        _thirdGoalFruitIcon.sprite = thirdGoalID.FruitID.FruitSprite;
        _thirdGoalAmount.text = thirdGoalID.Amount.ToString();
    }

    private void SetSliderMaxValue() => _starSlider.maxValue = _gameManager.GetHighestScore();
    private void ResetLevelStars() => _levelStars.Value = 0;
    private void ResetScore() => _score.Value = 0;
    public void CalculateScore(bool isGoalFruit)
    {
        if (isGoalFruit) _score.Value += GameManager.GoalFruitPoints;
        else _score.Value += GameManager.CommonFruitPoints;

        UpdateStarPercentage();
    }

    public void UpdateStarPercentage()
    {
        float starPercentage = _score.Value / 10f;
        _UIAnimation.AnimateSliderIncreasing(starPercentage);

        if (_starSlider.value >= _gameManager.FirstStarPercentage) _levelStars.Value = 1;

        if (_starSlider.value >= _gameManager.SecondStarPercentage) _levelStars.Value = 2;

        if (_starSlider.value >= _gameManager.ThirdStarPercentage) _levelStars.Value = 3;

        if (_levelStars.Value >= 1)
        {
            _UIAnimation.AnimateStarAppearing(levelStar: _levelStars.Value);
        }
    }
}
