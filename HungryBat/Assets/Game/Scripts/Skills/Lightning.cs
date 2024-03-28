using System.Collections;
using System.Collections.Generic;
using FruitItem;
using UnityEngine;

namespace Skill
{
    [CreateAssetMenu(fileName = "Lightning", menuName = "Skills/Lightning")]
    public class Lightning : Skill
    {
        public override IEnumerator Execute(Fruit selectedFruit, Fruit[,] boardFruit)
        {
            yield return new WaitForSeconds(1f); //TODO: substitute this for the needed time to execute animations
            Strike(GetAllFruitsOfType(selectedFruit, boardFruit));
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
        }
    }
}