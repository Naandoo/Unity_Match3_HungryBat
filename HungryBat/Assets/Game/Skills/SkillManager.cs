using System.Collections;
using Board;
using FruitItem;
using ScriptableVariables;
using TMPro;
using UnityEngine;
using Controllers;
using DG.Tweening;
using Game.UI;

namespace Skills
{
    public class SkillManager : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardMatcher _boardMatcher;
        [SerializeField] private BoardSorter _boardSorter;
        [SerializeField] private Canvas _skillUI;
        [SerializeField] private TMP_Text _skillNameUI;
        [SerializeField] private TMP_Text _skillDescriptionUI;
        [SerializeField] private Bomb _bomb;
        [SerializeField] private Lightning _lightning;
        [SerializeField] private Potion _potion;
        [SerializeField] private IntVariable _moves;
        [SerializeField] private BoolVariable _isLevelFinished;
        [SerializeField] private Animator _lightningAnimator;
        [SerializeField] private PotionEffectInstance _potionEffectInstance;
        [SerializeField] private PopupHandler _popupHandler;
        private bool _skillState;
        private Skill _selectedSkill = null;
        private WaitForSeconds _secondsToNextBomb;
        private WaitForSeconds _secondsToWinScreen;
        private void Start()
        {
            GameEvents.Instance.OnWinWithExtraMovements.AddListener(() =>
            {
                StartCoroutine(ReleaseBombsOnFlawlessWin());
                _popupHandler.DisablePopup(_skillUI);
            });

            GameEvents.Instance.OnWinEvent.AddListener(() =>
            {
                _popupHandler.DisablePopup(_skillUI);
            });

            GameEvents.Instance.OnLoseEvent.AddListener(() =>
            {
                _popupHandler.DisablePopup(_skillUI);
            });

            InitializeSkillProperties();

            _secondsToNextBomb = new(0.5f);
            _secondsToWinScreen = new(1f);
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnWinWithExtraMovements.RemoveListener(() =>
            {
                StartCoroutine(ReleaseBombsOnFlawlessWin());
                _popupHandler.DisablePopup(_skillUI);
            });

            GameEvents.Instance.OnWinEvent.RemoveListener(() =>
            {
                _popupHandler.DisablePopup(_skillUI);
            });

            GameEvents.Instance.OnLoseEvent.RemoveListener(() =>
            {
                _popupHandler.DisablePopup(_skillUI);
            });
        }

        private void InitializeSkillProperties()
        {
            _bomb.InitializePool(initialSize: 10, this.transform);
            _lightning.InitializeSkillProperties(initialSize: 10, this.transform, _lightningAnimator);
            _potion.InitializeProperties(_potionEffectInstance);
        }

        public void TriggerSkillMode(Skill skill)
        {
            if (skill.CurrentAmount.Value > 0 && !_isLevelFinished.Value)
            {
                _skillState = true;
                UpdateSkillUI(selectedSkill: skill);
                _selectedSkill = skill;
            }
        }

        public IEnumerator TriggerSkillOn(Fruit fruit)
        {
            if (!_skillState) yield break;
            DisableSkillCanvas();

            _skillState = false;
            _selectedSkill.CurrentAmount.Value--;
            BoardState.Instance.SetState(State.WaitingAction);

            yield return StartCoroutine(ExecuteSkill(_selectedSkill, fruit));
            StartCoroutine(_boardSorter.SortBoard());
            _boardMatcher.TryMatchFruits(matchWithMovement: false);
        }

        private IEnumerator ExecuteSkill(Skill selectedSkill, Fruit selectedFruit)
        {
            Vector3 fruitPosition = _boardGrid.GetFruitPosition(selectedFruit.Column, selectedFruit.Row);
            yield return StartCoroutine(selectedSkill.Execute(selectedFruit, _boardGrid.BoardFruitArray, fruitPosition));
        }

        private void UpdateSkillUI(Skill selectedSkill)
        {
            EnableSkillCanvas();
            _skillNameUI.text = selectedSkill.Name;
            _skillDescriptionUI.text = selectedSkill.Description;
        }

        public void CancelSkillUse()
        {
            DisableSkillCanvas();
            _skillState = false;
            _selectedSkill = null;
        }

        private IEnumerator ReleaseBombsOnFlawlessWin()
        {
            int remainingMoves = _moves.Value;

            for (int i = 0; i < remainingMoves; i++)
            {
                yield return _secondsToNextBomb;
                _moves.Value--;
                StartCoroutine(ExecuteSkill(_bomb, GetRandomFruit()));
                StartCoroutine(_boardSorter.SortBoard());
                _boardMatcher.TryMatchFruits(matchWithMovement: false);
            }

            yield return _secondsToWinScreen;
            StartCoroutine(_boardSorter.SortBoard());
            GameEvents.Instance.OnWinEvent.Invoke();
        }

        private Fruit GetRandomFruit()
        {
            int randomColumn = Random.Range(0, _boardGrid.Columns);
            int randomRow = Random.Range(0, _boardGrid.Rows);
            Fruit randomFruit = _boardGrid.BoardFruitArray[randomColumn, randomRow];

            if (randomFruit == null) return GetRandomFruit();
            else
            {
                return _boardGrid.BoardFruitArray[randomColumn, randomRow];
            }
        }

        private void EnableSkillCanvas()
        {
            Tween tween = _skillUI.transform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .SetUpdate(isIndependentUpdate: true)
            .OnStart(() =>
            {
                _skillUI.enabled = true;
            });
        }

        private void DisableSkillCanvas()
        {
            Tween tween = _skillUI.transform.DOScale(Vector3.zero, 0.25f)
            .SetEase(Ease.InBack)
            .SetUpdate(isIndependentUpdate: true)
            .OnComplete(() =>
            {
                _skillUI.enabled = false;
            });
        }
    }
}