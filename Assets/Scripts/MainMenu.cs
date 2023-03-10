using System.Threading.Tasks;
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
		[SerializeField] Transition transition;

		bool _exiting = false;

		void Start()
		{
			transition.AnimateEnter(.5f);
			
			easyButton.onClick.AddListener(() => StartGame(Difficulty.Easy));
			mediumButton.onClick.AddListener(() => StartGame(Difficulty.Medium));
			hardButton.onClick.AddListener(() => StartGame(Difficulty.Hard));
		}

		async Task StartGame(Difficulty difficulty)
		{
			easyButton.enabled = false;
			mediumButton.enabled = false;
			hardButton.enabled = false;
			
			if (_exiting)
				return;
			
			_exiting = true;

			transition.AnimateExit(.5f);

			await UniTask.Delay(500);
			
			SceneContext.Instance.GameDifficulty = difficulty;
			SceneManager.LoadScene("Questionnaire");
		}
	}
}
