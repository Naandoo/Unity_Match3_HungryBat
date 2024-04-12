using System.Collections;
using UnityEngine;
using FruitItem;
using System.Collections.Generic;
using Controllers;
using Unity.VisualScripting;

namespace Skills
{
    [CreateAssetMenu(fileName = "Bomb", menuName = "Skills/Bomb")]
    public class Bomb : Skill
    {
        [SerializeField] private GameObject _bombPrefab;
        private const int squareRange = 3;
        private int halfRange = squareRange / 2;
        private WaitForSeconds _secondsToPlayParticle = new(0.2f);
        private PoolSystem<GameObject> _bombPool;

        public void InitializePool(int initialSize, Transform parent)
        {
            _bombPool = new(_bombPrefab, initialSize, parent);
        }

        public void ReturnToPool(GameObject bomb)
        {
            _bombPool.Return(bomb);
        }

        public override IEnumerator Execute(Fruit selectedFruit, Fruit[,] boardFruit)
        {
            GameObject bomb = _bombPool.Get();
            bomb.transform.position = selectedFruit.gameObject.transform.position;

            Animator _bombAnimator = bomb.GetComponent<Animator>();
            while (_bombAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }

            Explode(GetFruitsInRange(selectedFruit, boardFruit));
            yield return _secondsToPlayParticle;
            ReturnToPool(bomb);
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