using System.Collections;
using Board;
using Game.UI;
using LevelData;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoardFruitPool _boardFruitPool;
    [SerializeField] private UiAnimation _uiAnimation;
    [SerializeField] private BoardGrid _boardGrid;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private LevelUIData _levelUIData;

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
        InitiateLevelRoutine();
    }

    private void InitiateLevelRoutine() => StartCoroutine(InitiateLevel());

    public IEnumerator InitiateLevel()
    {
        _levelManager.UpdateLevelScriptable(_levelManager.GetCurrentLevel());
        _levelUIData.UpdateUILevelData();
        yield return StartCoroutine(_uiAnimation.InitializeLevelAnimations());
        _boardGrid.CreateBoard();
    }

}
