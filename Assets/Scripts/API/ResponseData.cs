using System;

namespace Questionnaire
{
	[Serializable]
	public class ResponseData
	{
		public int response_code;
		public QuestionData[] results;

		public void Decode()
		{
			foreach (var questionData in results)
			{
				questionData.Decode();
			}
		}
	}
}