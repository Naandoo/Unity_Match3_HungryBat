using System.Collections;
using UnityEngine;
using FruitItem;
using System.Collections.Generic;

namespace Skills
{
    [CreateAssetMenu(fileName = "Bomb", menuName = "Skills/Bomb")]
    public class Bomb : Skill
    {
        private const int squareRange = 3;
        private int halfRange = squareRange / 2;

        public override IEnumerator Execute(Fruit selectedFruit, Fruit[,] boardFruit)
        {
            //TODO: substitute this for the needed time to execute animations when create it
            yield return new WaitForSeconds(1f);
            Explode(GetFruitsInRange(selectedFruit, boardFruit));
        }

        private void Explode(List<Fruit> fruits)
        {
            foreach (Fruit fruit in fruits)
            {
                fruit.Vanish();
            }

            GameEvents.Instance.OnFruitsExplodedEvent.Invoke(fruits);
        }

        private List<Fruit> GetFruitsInRange(Fruit fruit, Fruit[,] boardFruit)
        {
            List<Fruit> fruitsToExplode = new();

            int initialColumn = Mathf.Clamp(fruit.Column - halfRange, 0, boardFruit.GetLength(0));
            int initialRow = Mathf.Clamp(fruit.Row - halfRange, 0, boardFruit.GetLength(1));

            int explosionColumnDistance = Mathf.Clamp(initialColumn + squareRange, 0, boardFruit.GetLength(0));
            int explosionRowDistance = Mathf.Clamp(initialRow + squareRange, 0, boardFruit.GetLength(1));

            for (int i = initialColumn; i < explosionColumnDistance; i++)
            {
                for (int j = initialRow; j < explosionRowDistance; j++)
                {
                    Fruit selectedFruit = boardFruit[i, j];

                    if (selectedFruit == null) continue;
                    fruitsToExplode.Add(selectedFruit);
                }
            }

            return fruitsToExplode;
        }
    }
}