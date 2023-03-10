using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Questionnaire
{
	public class Transition : MonoBehaviour
	{
		[SerializeField] Image _cutoutImage;
		[SerializeField] Sprite _enterSprite;
		[SerializeField] Sprite _exitSprite;

		void Awake()
		{
			_cutoutImage.material = new Material(_cutoutImage.material);
		}

		public void AnimateExit(float duration)
		{
			Animate(1f, 0f, duration);
			
			_cutoutImage.sprite = _exitSprite;
		}
		
		public void AnimateEnter(float duration)
		{
			Animate(0f, 1f, duration);
			
			_cutoutImage.sprite = _enterSprite;
		}

		void Animate(float initialValue, float targetValue, float duration)
		{
			var animatedMaterial = _cutoutImage.material;
			var propId = Shader.PropertyToID("_Cutoff");
			var value = initialValue;

			DOTween.To(() => value, (x) =>
			{
				animatedMaterial.SetFloat(propId, x);
				value = x;
			}, targetValue, duration);
		}
	}
}