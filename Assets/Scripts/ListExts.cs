using System.Collections.Generic;
using UnityEngine;

namespace Questionnaire
{
	public static class ListExts
	{
		public static void Shuffle<T>(this List<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				int randomIndex = Random.Range(i, list.Count);

				(list[i], list[randomIndex]) = (list[randomIndex], list[i]);
			}
		}
	}
}