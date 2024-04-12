using System.Collections.Generic;
using UnityEngine;
using FruitItem;
using System.Collections;

namespace Skills
{
    [CreateAssetMenu(fileName = "Potion", menuName = "Skills/Potion")]
    public class Potion : Skill
    {
        private const int fruitsAmount = 15;

        public override IEnumerator Execute(Fruit selectedFruit, Fruit[,] boardFruit)
        {
            //TODO: substitute this for the needed time to execute animations when create it
            yield return new WaitForSeconds(1f);
            Clone(selectedFruit: selectedFruit, GetRandomFruits(boardFruit));
        }

        private List<Fruit> GetRandomFruits(Fruit[,] boardFruit)
        {
            List<Fruit> randomFruits = new();
            int remainingFruits = fruitsAmount;

            while (remainingFruits > 0)
            {
                int randomColumn = Random.Range(0, boardFruit.GetLength(0));
                int randomRow = Random.Range(0, boardFruit.GetLength(1));

                Fruit fruit = boardFruit[randomColumn, randomRow];
                if (fruit == null || randomFruits.Contains(fruit)) continue;
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
    }
}