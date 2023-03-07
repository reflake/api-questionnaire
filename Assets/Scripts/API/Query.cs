using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Questionnaire.API
{
	public class Query
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
		
		public async Task<QuestionData[]> GetQuestions()
		{
			string queryUrl = "https://opentdb.com/api.php?amount=10&category=15&difficulty=easy&type=multiple";
			HttpClient client = new HttpClient();

			string responseDataJson = await client.GetStringAsync(queryUrl);
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
	}
}