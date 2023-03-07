using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace Questionnaire
{
	public class Test : MonoBehaviour
	{
		public TMP_Text questionNameLabel = default;

		async void Start()
		{
			string queryUrl = "https://opentdb.com/api.php?amount=10&category=15&difficulty=easy&type=multiple";
			HttpClient client = new HttpClient();

			string responseData = await client.GetStringAsync(queryUrl);
			var data = JsonConvert.DeserializeObject<ResponseData>(responseData);

			data.Decode();
			
			questionNameLabel.text = data.results.First().question;
		}
	}
}
