using System.Collections.Generic;
using UnityEngine;
using FruitItem;
using System.Collections;
using DG.Tweening;

namespace Skills
{
    [CreateAssetMenu(fileName = "Potion", menuName = "Skills/Potion")]
    public class Potion : Skill
    {
        private const int fruitsAmount = 15;
        private PotionEffectInstance _potionEffectInstance;
        [SerializeField] private FruitIdDictionary _fruitIdDictionary;
        private float _effectDuration;
        private WaitForSeconds _stateTransitionDuration;

        public void InitializeProperties(PotionEffectInstance potionEffectInstance)
        {
            this._potionEffectInstance = potionEffectInstance;
            _fruitIdDictionary.InitializeDictionary();

            _effectDuration = _potionEffectInstance.GetEffectDuration();
            _stateTransitionDuration = new(0.1f);
        }

        public override IEnumerator Execute(Fruit selectedFruit, Fruit[,] boardFruit, Vector3 fruitPosition)
        {
            _potionEffectInstance.transform.position = fruitPosition;
            Texture2D fruitTexture = _fruitIdDictionary.dictionary[selectedFruit.FruitID.FruitType].FruitSprite.texture;
            _potionEffectInstance.PlayEffect(fruitTexture);
            List<Fruit> randomFruits = GetRandomFruits(boardFruit, selectedFruit.FruitID);

            yield return _stateTransitionDuration; // This line is responsible for waiting to catch the duration of the correctly state after transition.
            _effectDuration = _potionEffectInstance.GetEffectDuration();

            yield return new WaitForSeconds(_effectDuration / 2);
            ShakeFruitsScale(randomFruits);

            yield return new WaitForSeconds(_effectDuration / 2);
            Clone(selectedFruit: selectedFruit, randomFruits);
        }

        private List<Fruit> GetRandomFruits(Fruit[,] boardFruit, FruitID selectedID)
        {
            List<Fruit> randomFruits = new();
            int remainingFruits = fruitsAmount;

            while (remainingFruits > 0)
            {
                int randomColumn = Random.Range(0, boardFruit.GetLength(0));
                int randomRow = Random.Range(0, boardFruit.GetLength(1));

                Fruit fruit = boardFruit[randomColumn, randomRow];

                if (fruit == null || randomFruits.Contains(fruit)) continue;
                if (fruit.FruitID == selectedID) continue;

                else
                {
                    randomFruits.Add(fruit);
                    remainingFruits--;
                }
            }

            return randomFruits;
        }

        private void Clone(Fruit selectedFruit, List<Fruit> fruits)
        {
            FruitID fruitID = selectedFruit.FruitID;

            foreach (Fruit fruit in fruits)
            {
                fruit.SetFruitID(fruitID);
            }
        }

        private void ShakeFruitsScale(List<Fruit> fruits)
        {
            foreach (Fruit fruit in fruits)
            {
                ChangeFruitTween(fruit);
            }
        }
        private Tween ChangeFruitTween(Fruit fruit)
        {
            Tween tween = fruit.transform.DOShakeScale(_effectDuration, 0.5f, 3).SetEase(Ease.InBounce);
            return tween;
        }
    }
}