using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

using Unity.Mathematics;

namespace JoaoSantos.General
{
    public static class CollectionsExtensions
    {
        public static int randomIndex = 2;
        public static List<T> Shuffle<T>(this List<T> list)
        {
            List<T> copyList = new List<T>(list);

            List<T> newList = new List<T>();

            while (copyList.Count > 0)
            {
                var ran = new Random((uint)randomIndex);
                randomIndex++;

                var index = ran.NextInt(0, copyList.Count);
                var item = copyList[index];

                newList.Add(item);
                copyList.Remove(item);
            }

            return newList;
        }

        public static T[] Shuffle<T>(this T[] list)
        {
            var newList = Shuffle(new List<T>(list));

            return newList.ToArray();
        }

        public static T GetRandomItem<T>(this T[] list)
        {
            var ran = new Random((uint)randomIndex);
            randomIndex++;

            return list[ran.NextInt(0, list.Length)];
        }

        public static T GetRandomItem<T>(this List<T> list)
        {
            var ran = new Random((uint)randomIndex);
            randomIndex++;

            return list[ran.NextInt(0, list.Count)];
        }
    }
}
