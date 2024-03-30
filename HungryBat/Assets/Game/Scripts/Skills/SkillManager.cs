using System.Collections;
using Board;
using FruitItem;
using UnityEngine;

namespace Skill
{
    public class SkillManager : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardMatcher _boardMatcher;
        [SerializeField] private BoardSorter _boardSorter;
        [SerializeField] private Canvas _skillUI;
        private bool skillState;
        private Skill selectedSkill = null;

        public void TriggerSkillMode(Skill skill)
        {
            BoardState.Instance.SetState(State.WaitingAction);
            skillState = true;
            // _skillUI.enabled = true;
            selectedSkill = skill;
        }

        public IEnumerator CheckSkillState(Fruit fruit)
        {
            if (!skillState) yield break;
            // _skillUI.enabled = false;

            yield return StartCoroutine(selectedSkill.Execute(fruit, _boardGrid.BoardFruitArray));

            skillState = false;
            selectedSkill = null;

            StartCoroutine(_boardSorter.SortBoard());
            _boardMatcher.TryMatchFruits(matchWithMovement: false);
        }

    }
}