using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LJ
{
    public static class Extensions
    {
        // --- Enums ------------------------------------------------------------------------------------------------------		

        // --- Fields -----------------------------------------------------------------------------------------------------

        // --- Properties -------------------------------------------------------------------------------------------------

        // --- Constructors -----------------------------------------------------------------------------------------------

        // --- Event callbacks --------------------------------------------------------------------------------------------

        // --- Public/Internal Methods ------------------------------------------------------------------------------------
        public static void SetAlpha(this Image img, float a)
        {
            Color color = img.color;
            color.a = a;
            img.color = color;
        }

        public static T GetRandomElement<T>(this IList<T> list)
        {
            int index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }

        public static int IndexOf<T>(this T[] array, T element)
        {
            if(element == null || element.Equals(null))
                return -1;

            for(int i = 0; i < array.Length; i++)
            {
                if(element.Equals(array[i]))
                    return i;
            }

            return -1;
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}