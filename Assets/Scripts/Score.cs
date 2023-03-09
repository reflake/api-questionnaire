using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Questionnaire
{
	public class Score : MonoBehaviour
	{
		[SerializeField] TMP_Text scoreCounterLabel = default;
		[SerializeField] Transform scoreTarget = default;
		
		int _score = 0;
		int _maximalScore;

		public void Setup(int initialScore, int maximalScore)
		{
			_maximalScore = maximalScore;
			
			UpdateScoresLabel();
		}
		
		public void Add(int amount)
		{
			_score += amount;
			
			UpdateScoresLabel();
		}

		void UpdateScoresLabel()
		{
			scoreCounterLabel.text = $"{_score}/{_maximalScore}";
		}

		public void Center(float duration)
		{
			var endingSequence = DOTween.Sequence();
			
			endingSequence
				.Append(scoreCounterLabel.rectTransform.DOMove(scoreTarget.position, duration))
				.Join(scoreCounterLabel.rectTransform.DOScale(1.9f, duration))
				.Append(scoreCounterLabel.rectTransform.DOScale(2.5f, .66f).SetEase(Ease.OutFlash, 2, 0f));
		}
	}
}