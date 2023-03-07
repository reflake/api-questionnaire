using System.Collections.Generic;

namespace Questionnaire.API
{
	public class QuestionData
	{
		public readonly string Description;
		public readonly string CorrectAnswer;
		public IReadOnlyCollection<string> IncorrectAnswers;

		public QuestionData(string description, string correctAnswer, string[] incorrectAnswers)
		{
			Description = description;
			CorrectAnswer = correctAnswer;
			IncorrectAnswers = incorrectAnswers;
		}
	}
}