using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using Questionnaire.API;

namespace Questionnaire
{
	public class Test : MonoBehaviour
	{
		public TMP_Text questionNameLabel = default;

		async void Start()
		{
			var questions = await new Query().GetQuestions();
			
			questionNameLabel.text = questions.First().Description;
		}
	}
}
