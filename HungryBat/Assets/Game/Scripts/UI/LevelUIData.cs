using LevelData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIData : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelMoves;
    [SerializeField] private Image _firstGoalFruitIcon, _secondGoalFruitIcon, _thirdGoalFruitIcon;
    [SerializeField] private TMP_Text _firstGoalAmount, _secondGoalAmount, _thirdGoalAmount;
    [SerializeField] private LevelManager _levelManager;

    public void UpdateUILevelData()
    {
        Level currentLevel = _levelManager.GetCurrentLevel();
        UpdateMoves(currentLevel);
        UpdateGoal(currentLevel);
    }

    private void UpdateMoves(Level currentLevel) => _levelMoves.text = currentLevel.Moves.ToString();

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
}
