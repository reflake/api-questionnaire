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

		public void Hide()
		{
			Destroy(gameObject);
		}
	}
}