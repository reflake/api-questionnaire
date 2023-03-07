using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Questionnaire.API;

namespace Questionnaire
{
	public class Test : MonoBehaviour
	{
		[SerializeField] AnswerButton answerButtonPrefab = default;
		[SerializeField] Transform answersContainer = default;
		[SerializeField] TMP_Text questionNameLabel = default;
		[SerializeField] TMP_Text scoreCounterLabel = default;

		List<AnswerButton> _instantiatedButtons = new();
		int _score = 0;
		
		async void Start()
		{
			var questions = await new Query().GetQuestions(this.GetCancellationTokenOnDestroy());
			
			UpdateScoresLabel(questions.Length);

			foreach (var questionData in questions)
			{
				bool answeredCorrectly = await CreateQuestion(questionData);

				if (answeredCorrectly)
				{
					_score++;
					
					UpdateScoresLabel(questions.Length);
				}

				await UniTask.Delay(2000);
				
				foreach (var button in _instantiatedButtons)
				{
					button.Destroy();
				}

				questionNameLabel.text = string.Empty;

				await UniTask.Delay(500);
			}
		}

		void UpdateScoresLabel(int questionsAmount)
		{
			scoreCounterLabel.text = $"{_score}/{questionsAmount}";
		}

		Func<string, AnswerButton> InstantiateAnswerButton(string correctAnswer)
		{
			return (answer) =>
			{
				AnswerButton instanceOfAnswerButton = null;

				if (_instantiatedButtons.Any(button => button.Cached))
				{
					instanceOfAnswerButton = _instantiatedButtons.First(button => button.Cached);
				}
				else
				{
					instanceOfAnswerButton = Instantiate(answerButtonPrefab, answersContainer);
				}
				
				instanceOfAnswerButton.Setup(answer, answer == correctAnswer);
				
				_instantiatedButtons.Add(instanceOfAnswerButton);

				return instanceOfAnswerButton;
			};
		}

		async Task<bool> CreateQuestion(QuestionData questionData)
		{
			questionNameLabel.text = questionData.Description;

			List<string> answersShuffled = new();
			answersShuffled.Add(questionData.CorrectAnswer);
			answersShuffled.AddRange(questionData.IncorrectAnswers);
			
			answersShuffled.Shuffle();

			var answerInstances = answersShuffled
				.Select(InstantiateAnswerButton(questionData.CorrectAnswer))
				.ToArray();

			var cts = new CancellationTokenSource();
			var getClickedTasks = answerInstances
				.Select(answerButton => answerButton.AsyncGetClicked(cts.Token))
				.ToArray();

			var answer = await Task.WhenAny(getClickedTasks);

			cts.Cancel();

			foreach (var answerButton in answerInstances)
			{
				answerButton.SetEnabled(false);
			}

			return answer.Result;
		}
	}
}
