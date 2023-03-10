using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Questionnaire
{
	public class AnswerButton : MonoBehaviour
	{
		[SerializeField] TMP_Text label;
		[SerializeField] Button button;
		[SerializeField] Image background;
		[SerializeField] Color defaultColor;
		[SerializeField] Color correctAnswerColor;
		[SerializeField] Color incorrectAnswerColor;

		bool _isCorrect;

		public void Setup(string answer, bool isCorrect)
		{
			label.text = answer;
			_isCorrect = isCorrect;
			
			gameObject.SetActive(true);
			
			SetColor(defaultColor);
			SetEnabled(true);
		}

		public void SetEnabled(bool value)
		{
			button.enabled = value;
		}

		public async Task<bool> AsyncGetClicked(CancellationToken cancellationToken)
		{
			// Should stop awaiting Click() when button destroyed
			var destroyToken = this.GetCancellationTokenOnDestroy();
			var compositeTokenSource = CancellationTokenSource.CreateLinkedTokenSource(destroyToken, cancellationToken);
			
			await button.OnClickAsync(compositeTokenSource.Token);
			
			SetColor(_isCorrect ? correctAnswerColor : incorrectAnswerColor);

			return _isCorrect;
		}

		public void SetColor(Color color)
		{
			label.color = color.grayscale > .5f ? Color.black : Color.white;
			background.color = color;
		}

		public void Destroy()
		{
			Destroy(gameObject);
		}
	}
}