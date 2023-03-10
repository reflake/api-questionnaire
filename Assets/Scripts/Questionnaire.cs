using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Questionnaire.API;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Questionnaire
{
	public class Questionnaire : MonoBehaviour
	{
		[SerializeField] AnswerButton answerButtonPrefab = default;
		[SerializeField] Transform answersContainer = default;
		[SerializeField] TMP_Text questionNameLabel = default;
		[SerializeField] CanvasGroup canvasGroup = default;
		[SerializeField] LoadingPanel loadingPanel = default;
		[SerializeField] Score _score = default;
		[SerializeField] Button _exitButton = default;
		[SerializeField] Transition _transition = default;

		List<AnswerButton> _instantiatedButtons = new();
		Difficulty _difficulty = Difficulty.Easy;
		bool _exiting = false;
		
		void Start()
		{
			_transition.AnimateEnter(.5f);
			
			_exitButton.onClick.AddListener(() => ExitToMenu());
			
			if (SceneContext.Initialized)
			{
				_difficulty = SceneContext.Instance.GameDifficulty;
			}
			
			StartQuestions();
		}

		async Task StartQuestions()
		{
			loadingPanel.Show();

			var questions = await Query.GetQuestions(_difficulty, this.GetCancellationTokenOnDestroy());

			await loadingPanel.AsyncHide();

			canvasGroup.alpha = 0f;

			_score.Setup(0, questions.Length);

			int questionNumber = 1;

			foreach (var questionData in questions)
			{
				canvasGroup.DOFade(1f, .5f);

				bool answeredCorrectly = await CreateQuestion(questionNumber++, questionData);

				if (answeredCorrectly)
				{
					_score.Add(1);
				}

				DisableAllButtons();

				await UniTask.Delay(2000, cancellationToken: this.GetCancellationTokenOnDestroy());

				canvasGroup.DOFade(0f, .5f);

				await UniTask.Delay(500, cancellationToken: this.GetCancellationTokenOnDestroy());

				DestroyButtons();

				questionNameLabel.text = string.Empty;
			}

			float duration = 1.5f;

			questionNameLabel.text = "Your re<color=#e65a99>s</color>ult";

			_score.Center(duration);

			canvasGroup.DOFade(1f, duration);

			await UniTask.Delay(3000);

			ExitToMenu();
		}

		Func<string, AnswerButton> InstantiateAnswerButton(string correctAnswer)
		{
			return (answer) =>
			{
				AnswerButton instanceOfAnswerButton = Instantiate(answerButtonPrefab, answersContainer);
				instanceOfAnswerButton.Setup(answer, answer == correctAnswer);

				return instanceOfAnswerButton;
			};
		}

		async Task<bool> CreateQuestion(int number, QuestionData questionData)
		{
			// Parse description;
			var description = questionData.Description;

			questionNameLabel.text = $"<size=80><color=#db3030>Q</color>uest<color=#b778eb>i</color>on №{number}</size>\n{description}";

			// Shuffle answers
			List<string> answersShuffled = new(questionData.Answers);
			
			answersShuffled.Shuffle();

			// Instantiate answer buttons
			_instantiatedButtons = answersShuffled
				.Select(InstantiateAnswerButton(questionData.CorrectAnswer))
				.ToList();

			// Await for click
			var getClickedTasks = _instantiatedButtons
				.Select(answerButton => answerButton.AsyncGetClicked())
				.ToArray();

			var answerTask = await Task.WhenAny(getClickedTasks);

			return answerTask.Result;
		}

		void DisableAllButtons()
		{
			foreach (var answerButton in _instantiatedButtons)
			{
				answerButton.SetEnabled(false);
			}
		}

		void DestroyButtons()
		{
			foreach (var button in _instantiatedButtons)
			{
				button.Destroy();
			}

			_instantiatedButtons.Clear();
		}

		async Task ExitToMenu()
		{
			if (_exiting)
				return;
			
			_exiting = true;
			
			_transition.AnimateExit(.5f);

			await UniTask.Delay(500);

			SceneManager.LoadScene("MainMenu");
		}
	}
}
