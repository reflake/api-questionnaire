using System.Collections;
using UnityEngine;

namespace Questionnaire
{
	public static class ListExts
	{
		public static TList Shuffle<TList>(this TList list) where TList : IList
		{
			for (int i = 0; i < list.Count; i++)
			{
				int randomIndex = Random.Range(i, list.Count);

				(list[i], list[randomIndex]) = (list[randomIndex], list[i]);
			}
			
			return list;
		}
	}
}