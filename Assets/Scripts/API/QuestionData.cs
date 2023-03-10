using System.Collections.Generic;

namespace Questionnaire.API
{
	public class QuestionData
	{
		public readonly string Description;
		public readonly string CorrectAnswer;
		public IReadOnlyCollection<string> Answers;

		public QuestionData(string description, string correctAnswer, string[] incorrectAnswers)
		{
			Description = description;
			CorrectAnswer = correctAnswer;

			var answers = new List<string>();

			answers.Add(correctAnswer);
			answers.AddRange(incorrectAnswers);

			Answers = answers;
		}
	}
}