using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;


namespace JoaoSantos.General
{
    public static class CollectionsGenerator
    {
        public static List<int> MakeSequence(int start, int end)
        {
            IEnumerable<int> sequence = null;

            if (start > end)
            {
                sequence = Enumerable.Range(end, start - end + 1);

                sequence = sequence.Reverse();
            }
            else
            {
                sequence = Enumerable.Range(start, end - start + 1);

            }

            return sequence.ToList();
        }
    }
}
