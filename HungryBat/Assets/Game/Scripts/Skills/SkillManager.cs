using System.Collections;
using Board;
using FruitItem;
using TMPro;
using UnityEngine;

namespace Skill
{
    public class SkillManager : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardMatcher _boardMatcher;
        [SerializeField] private BoardSorter _boardSorter;
        [SerializeField] private Canvas _skillUI;
        [SerializeField] private TMP_Text _skillNameUI;
        [SerializeField] private TMP_Text _skillDescriptionUI;
        private bool skillState;
        private Skill selectedSkill = null;

        public void TriggerSkillMode(Skill skill)
        {
            BoardState.Instance.SetState(State.WaitingAction);
            skillState = true;
            UpdateSkillUI(selectedSkill: skill);
            selectedSkill = skill;
        }

        public IEnumerator CheckSkillState(Fruit fruit)
        {
            if (!skillState) yield break;
            _skillUI.enabled = false;

            skillState = false;

            yield return StartCoroutine(selectedSkill.Execute(fruit, _boardGrid.BoardFruitArray));

            selectedSkill = null;

            StartCoroutine(_boardSorter.SortBoard());
            _boardMatcher.TryMatchFruits(matchWithMovement: false);
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

    }
}