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

		void StartGame(Difficulty difficulty)
		{
			SceneContext.Instance.GameDifficulty = difficulty;
			SceneManager.LoadScene("Questionnaire");
		}
	}
}
