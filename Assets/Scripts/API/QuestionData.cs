using System;
using System.Net;

namespace Questionnaire
{
	[Serializable]
	public class QuestionData
	{
		public string question;
		public string correct_answer;
		public string[] incorrect_answers;

		public void Decode()
		{
			question = WebUtility.HtmlDecode(question);
			correct_answer = WebUtility.HtmlDecode(correct_answer);

			for (int i = 0; i < incorrect_answers.Length; i++)
			{
				incorrect_answers[i] = WebUtility.HtmlDecode(incorrect_answers[i]);
			}
		}
	}
}