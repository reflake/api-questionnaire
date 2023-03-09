using System;

namespace Questionnaire
{
	[AttributeUsage(AttributeTargets.Field)]
	public class DifficultyAttribute : Attribute
	{
		public readonly string QueryParameterName;

		public DifficultyAttribute(string queryParameterName)
		{
			QueryParameterName = queryParameterName;
		}
	}
}