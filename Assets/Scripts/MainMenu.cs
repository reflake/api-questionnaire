using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Questionnaire
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] Button easyButton;
		[SerializeField] Button mediumButton;
		[SerializeField] Button hardButton;

		void Awake()
		{
			easyButton.onClick.AddListener(() => StartGame(Difficulty.Easy));
			mediumButton.onClick.AddListener(() => StartGame(Difficulty.Medium));
			hardButton.onClick.AddListener(() => StartGame(Difficulty.Hard));
		}

		async void StartGame(Difficulty difficulty)
		{
			SceneContext.Instance.GameDifficulty = difficulty;
			
			await SceneManager.LoadSceneAsync("Questionnaire").ToUniTask();
		}
	}
}
