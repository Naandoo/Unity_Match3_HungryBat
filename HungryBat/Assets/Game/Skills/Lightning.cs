using System.Collections;
using System.Collections.Generic;
using FruitItem;
using UnityEngine;
using Controllers;
using DG.Tweening;

namespace Skills
{
    [CreateAssetMenu(fileName = "Lightning", menuName = "Skills/Lightning")]
    public class Lightning : Skill
    {
        [SerializeField] private Thunderbolt _thunderboltObject;
        private PoolSystem<Thunderbolt> _thunderboltPool;
        private Animator _lightningAnimator;
        private WaitForSeconds _thunderboltAnimationDuration;

        public void InitializeSkillProperties(int initialSize, Transform parent, Animator lightningAnimator)
        {
            _thunderboltPool = new(_thunderboltObject, initialSize, parent);
            this._lightningAnimator = lightningAnimator;

            _thunderboltAnimationDuration = new(_lightningAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        public override IEnumerator Execute(Fruit selectedFruit, Fruit[,] boardFruit, Vector3 fruitPosition)
        {
            float thunderboltSpeed = 0.5f;

            _lightningAnimator.transform.position = fruitPosition;
            _lightningAnimator.Play("LightningAnimation", 0, 0);

            yield return _thunderboltAnimationDuration;

            List<Fruit> equalFruits = GetAllFruitsOfType(selectedFruit, boardFruit);
            List<Thunderbolt> _invokedThunderbolts = new();
            Sequence thunderboltSequence = DOTween.Sequence();

            foreach (Fruit fruit in equalFruits)
            {
                Thunderbolt thunderbolt = _thunderboltPool.Get();
                _invokedThunderbolts.Add(thunderbolt);
                thunderbolt.transform.position = _lightningAnimator.transform.position;

                thunderbolt.PlayThunderboltAnim();
                thunderboltSequence.Join(thunderbolt.transform.DOMove(fruit.transform.position, thunderboltSpeed).OnComplete(() =>
                {
                    thunderbolt.PlayLightningStrikeAnim();
                }));
            }

            thunderboltSequence.Play();
            thunderboltSequence.OnComplete(() =>
            {
                Strike(equalFruits);

                foreach (Thunderbolt thunderboltObj in _invokedThunderbolts)
                {
                    _thunderboltPool.Return(thunderboltObj);
                }

            });

            yield return thunderboltSequence.WaitForCompletion();
        }

        private List<Fruit> GetAllFruitsOfType(Fruit selectedFruit, Fruit[,] boardFruit)
        {
            List<Fruit> FruitsEqualType = new();
            FruitType fruitType = selectedFruit.FruitID.FruitType;

            foreach (Fruit fruit in boardFruit)
            {
                if (fruit == null) continue;

                if (fruit.FruitID.FruitType == fruitType)
                {
                    FruitsEqualType.Add(fruit);
                }
            }

            return FruitsEqualType;
        }

        private void Strike(List<Fruit> fruits)
        {
            foreach (Fruit fruit in fruits)
            {
                fruit.Vanish();
            }

            GameEvents.Instance.OnFruitsExplodedEvent.Invoke(fruits);
        }
    }
}