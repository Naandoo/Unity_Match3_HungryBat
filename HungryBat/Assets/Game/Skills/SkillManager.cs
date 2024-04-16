using System.Collections;
using Board;
using FruitItem;
using ScriptableVariables;
using TMPro;
using UnityEngine;
using Controllers;

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
        private bool skillState;
        private Skill selectedSkill = null;

        private void Start()
        {
            GameEvents.Instance.OnWinWithExtraMovements.AddListener(() =>
            {
                StartCoroutine(ReleaseBombsOnFlawlessWin());
            });
            InitializeSkillProperties();
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnWinWithExtraMovements.RemoveListener(() =>
            {
                StartCoroutine(ReleaseBombsOnFlawlessWin());
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
                skillState = true;
                UpdateSkillUI(selectedSkill: skill);
                selectedSkill = skill;
            }
        }

        public IEnumerator CheckSkillState(Fruit fruit)
        {
            if (!skillState) yield break;
            _skillUI.enabled = false;

            skillState = false;
            selectedSkill.CurrentAmount.Value--;
            BoardState.Instance.SetState(State.WaitingAction);

            yield return StartCoroutine(ExecuteSkill(selectedSkill, fruit));
            StartCoroutine(_boardSorter.SortBoard());
            _boardMatcher.TryMatchFruits(matchWithMovement: false);

            selectedSkill = null;
        }

        private IEnumerator ExecuteSkill(Skill selectedSkill, Fruit selectedFruit)
        {
            yield return StartCoroutine(selectedSkill.Execute(selectedFruit, _boardGrid.BoardFruitArray));
        }

        private void UpdateSkillUI(Skill selectedSkill)
        {
            _skillUI.enabled = true;
            _skillNameUI.text = selectedSkill.Name;
            _skillDescriptionUI.text = selectedSkill.Description;
        }

        public void CancelSkillUse()
        {
            _skillUI.enabled = false;
            skillState = false;
            selectedSkill = null;
        }

        private IEnumerator ReleaseBombsOnFlawlessWin()
        {
            int remainingMoves = _moves.Value;

            for (int i = 0; i < remainingMoves; i++)
            {
                _moves.Value--;
                yield return StartCoroutine(ExecuteSkill(_bomb, GetRandomFruit()));
                StartCoroutine(_boardSorter.SortBoard());
                _boardMatcher.TryMatchFruits(matchWithMovement: false);
            }

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

    }
}