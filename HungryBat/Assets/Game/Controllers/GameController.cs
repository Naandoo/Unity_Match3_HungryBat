using System.Collections;
using Board;
using Game.UI;
using LevelData;
using ScriptableVariable;
using UnityEngine;

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
        Level currentLevel;

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
            _isLevelFinished.Value = false;
            _boardGrid.ClearBoard();
            currentLevel = _levelManager.GetCurrentLevel();
            _levelManager.UpdateLevelScriptable(currentLevel);
            _gameManager.CopyLevelGoals(currentLevel);
            _levelUIData.UpdateUILevelData();
            yield return StartCoroutine(_uiAnimation.InitializeLevelAnimations());
            _boardGrid.CreateBoard();
        }

        public void InitiateNextLevel()
        {
            PlayerPrefs.SetInt("savedLevel", currentLevel.Number + 1);
            InitiateCurrentLevelRoutine();
        }
    }
}
