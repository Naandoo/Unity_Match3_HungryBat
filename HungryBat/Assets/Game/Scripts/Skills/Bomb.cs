using System.Collections;
using UnityEngine;
using FruitItem;
using System.Collections.Generic;

namespace Skill
{
    [CreateAssetMenu(fileName = "Bomb", menuName = "Skills/Bomb")]
    public class Bomb : Skill
    {
        private const int squareRange = 3;
        private int halfRange = squareRange / 2;

        public override IEnumerator Execute(Fruit selectedFruit, Fruit[,] boardFruit)
        {
            yield return new WaitForSeconds(1f); //TODO: substitute this for the needed time to execute animations

            Explode(GetFruitsInRange(selectedFruit, boardFruit));
        }

        private void Explode(List<Fruit> fruits)
        {
            foreach (Fruit fruit in fruits)
            {
                fruit.Vanish();
            }
        }

        private List<Fruit> GetFruitsInRange(Fruit fruit, Fruit[,] boardFruit)
        {
            List<Fruit> fruitsToExplode = new();
            int initialRow = fruit.Row - halfRange;
            int initialColumn = fruit.Column - halfRange;

            for (int i = initialColumn; i < initialColumn; i++)
            {
                for (int j = initialRow; j < initialRow; j++)
                {
                    Fruit selectedFruit = boardFruit[i, j];

                    if (selectedFruit == null) continue; // or doesn't have tile may need to be included

                    fruitsToExplode.Add(selectedFruit);
                }
            }

            return fruitsToExplode;
        }
    }
}