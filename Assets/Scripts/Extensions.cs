using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
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

        //public static T GetRandomElement<T>(this ICollection<T> collection)
        //{
        //	int index = UnityEngine.Random.Range(0, collection.Count);
        //          IEnumerator<T> enumerator = collection.GetEnumerator();

        //	for(int i = 0; i < index; i++)
        //	{
        //		enumerator.MoveNext();
        //	}

        //	return enumerator.Current;
        //}

        public static T GetRandomElement<T>(this IList<T> list)
        {
            int index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }

        // --- Protected/Private Methods ----------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------------------
    }
}