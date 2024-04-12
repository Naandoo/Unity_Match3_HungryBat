using System;
using System.Collections.Generic;

namespace Toolbox
{
    public static class ListUtils
    {
        public static void Shuffle<T>(List<T> list)
        {
            Random random = new Random();
            int count = list.Count;

            while (count > 1)
            {
                count--;
                int randomIndex = random.Next(count + 1);
                T value = list[randomIndex];
                list[randomIndex] = list[count];
                list[count] = value;
            }
        }
    }
}