using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Questionnaire.API;

namespace Questionnaire
{
	public class Questionnaire : MonoBehaviour
	{
		[SerializeField] AnswerButton answerButtonPrefab = default;
		[SerializeField] Transform answersContainer = default;
		[SerializeField] TMP_Text questionNameLabel = default;
		[SerializeField] TMP_Text scoreCounterLabel = default;
		[SerializeField] CanvasGroup canvasGroup = default;
		[SerializeField] LoadingPanel loadingPanel = default;
		[SerializeField] Transform scoreTarget = default;

		List<AnswerButton> _instantiatedButtons = new();
		int _score = 0;
		Difficulty _difficulty = Difficulty.Easy;
		
		async void Start()
		{
			if (SceneContext.Initialized)
			{
				_difficulty = SceneContext.Instance.GameDifficulty;
			}
			
			loadingPanel.Show();
			
			var questions = await Query.GetQuestionsMock(_difficulty, this.GetCancellationTokenOnDestroy());
			
			loadingPanel.Hide();

			canvasGroup.alpha = 0f;
			
			UpdateScoresLabel(questions.Length);

			foreach (var questionData in questions)
			{
				canvasGroup.DOFade(1f, .5f);
				
				bool answeredCorrectly = await CreateQuestion(questionData);

				if (answeredCorrectly)
				{
					_score++;
					
					UpdateScoresLabel(questions.Length);
				}

				await UniTask.Delay(2000);
				await canvasGroup.DOFade(0f, .5f)
									.AsyncWaitForCompletion();
				
				foreach (var button in _instantiatedButtons)
				{
					button.Destroy();
				}

				questionNameLabel.text = string.Empty;
			}

			var endingSequence = DOTween.Sequence();
			
			questionNameLabel.text = "Your result";

			endingSequence
				.Append(canvasGroup.DOFade(1f, 1.5f))
				.Join(scoreCounterLabel.rectTransform.DOMove(scoreTarget.position, 1.5f))
				.Join(scoreCounterLabel.rectTransform.DOScale(1.9f, 1.5f))
				.Append(scoreCounterLabel.rectTransform.DOScale(2.5f, .66f).SetEase(Ease.OutFlash, 2, 0f));
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
				
					_instantiatedButtons.Add(instanceOfAnswerButton);
				}
				
				instanceOfAnswerButton.Setup(answer, answer == correctAnswer);

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
