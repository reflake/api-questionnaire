using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Questionnaire
{
	public class LoadingPanel : MonoBehaviour
	{
		[SerializeField] Image background;

		public void Show()
		{
			background.DOFade(0.1f, 3f)
				.SetEase(Ease.InOutFlash, 2f, 0f)
				.SetLoops(-1);
		}

		public async Task AsyncHide()
		{
			background.DOFade(0f, .66f);

			await UniTask.Delay(666, cancellationToken: this.GetCancellationTokenOnDestroy());
			
			Destroy(gameObject);
		}
	}
}