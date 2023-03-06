using System;

namespace Questionnaire
{
	[Serializable]
	public class QuestionData
	{
		public string question;
		public string correct_answer;
		public string[] incorrect_answers;
	}
}