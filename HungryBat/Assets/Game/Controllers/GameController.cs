using System.Collections;
using Board;
using Game.UI;
using LevelData;
using ScriptableVariables;
using UnityEngine;
using Skills;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private BoardFruitPool _boardFruitPool;
        [SerializeField] private UiAnimation _uiAnimation;
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private LevelUIData _levelUIData;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private BoolVariable _isLevelFinished;
        [SerializeField] private Skill _bomb, _potion, _lightning;
        private Level _currentLevel;
        private bool _ableToRestartLevel = true;

        private void Awake()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            int poolSize = _boardGrid.Columns * _boardGrid.Rows;
            _boardFruitPool.Initialize(poolSize);
        }

        private void Start()
        {
            InitiateCurrentLevelRoutine();
        }

        public void InitiateCurrentLevelRoutine() => StartCoroutine(InitiateLevel());

        public IEnumerator InitiateLevel()
        {
            if (_ableToRestartLevel)
            {
                _ableToRestartLevel = false;
                _isLevelFinished.Value = false;
                _boardGrid.ClearBoard();
                _currentLevel = _levelManager.GetCurrentLevel();
                _levelManager.UpdateLevelScriptable(_currentLevel);
                _gameManager.CopyLevelGoals(_currentLevel);
                _gameManager.SetStarsGoalPercentage();
                _levelUIData.UpdateUILevelData();
                InitializeLevelSkills();
                GameSounds.Instance.PlayDefaultBackgroundMusic();
                yield return StartCoroutine(_uiAnimation.InitializeLevelUI());
                _boardGrid.CreateBoard();
                _ableToRestartLevel = true;
            }
        }

        public void InitiateNextLevel()
        {
            PlayerPrefs.SetInt("savedLevel", _currentLevel.Number + 1);
            InitiateCurrentLevelRoutine();
        }

        private void InitializeLevelSkills()
        {
            _bomb.CurrentAmount.Value = _currentLevel.SkillsAvailability.BombsAmount;
            _potion.CurrentAmount.Value = _currentLevel.SkillsAvailability.PotionAmount;
            _lightning.CurrentAmount.Value = _currentLevel.SkillsAvailability.LightningAmount;
        }
    }
}
