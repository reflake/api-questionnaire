using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Questionnaire.API
{
	public static class Query
	{
		[Serializable]
		class QuestionDataRaw
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
		
		[Serializable]
		class ResponseData
		{
			public int response_code;
			public QuestionDataRaw[] results;
		}

		public static string GetDifficultySerializedName(Difficulty difficulty)
		{
			var enumMemberInfo = typeof(Difficulty).GetMember(difficulty.ToString())[0];
			var difficultyAttribute = enumMemberInfo.GetCustomAttribute(typeof(DifficultyAttribute), false) as DifficultyAttribute;

			return difficultyAttribute.QueryParameterName;
		}
		
		public static async Task<QuestionData[]> AsyncGetQuestions(Difficulty difficulty, CancellationToken cancellationToken)
		{
			var amount = 20;
			var difficultyName = GetDifficultySerializedName(difficulty);
			var queryUrl = $"https://opentdb.com/api.php?amount={amount}&difficulty={difficultyName}&category=15&type=multiple";
			var client = new HttpClient();

			var httpResponse = await client.GetAsync(queryUrl, cancellationToken);
			var responseDataJson = await httpResponse.Content.ReadAsStringAsync();
			var data = JsonConvert.DeserializeObject<ResponseData>(responseDataJson);

			if (data.response_code != 0)
			{
				throw new Exception("Couldn't quest questions");
			}
			
			foreach (var question in data.results)
			{
				question.Decode();
			}

			return data.results
				.Select(x => new QuestionData(x.question, x.correct_answer, x.incorrect_answers))
				.ToArray();
		}

		public static async Task<QuestionData[]> GetQuestionsMock(Difficulty _, CancellationToken __)
		{
			var mockQuestions = new[]
			{
				new QuestionData("2+2",
					"4", new[] { "1", "10", "5" }),
				new QuestionData("Орём или Орёл",
					"Орёл", new[] { "Орём" }),
				new QuestionData("Где находиться \"клуб любителей кожевного ремесла\"?",
					"Двумя этажами ниже", new[] { "В данджене", "За следующей дверью", "В качалке" }),
			};
			
			return await Task.FromResult(mockQuestions.Shuffle());
		}
	}
}