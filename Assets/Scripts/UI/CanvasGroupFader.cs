using UnityEngine;

namespace UI
{
	using System.Collections;

	[RequireComponent(typeof(CanvasGroup))]
	public class CanvasGroupFader : MonoBehaviour
	{
		private CanvasGroup _canvasGroup;
	
		private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

		public IEnumerator FadeTo(float targetAlpha, float duration)
		{
			var initialAlpha = _canvasGroup.alpha;
			var time = 0f;
			
			while (time < duration)
			{
				time += Time.unscaledDeltaTime;
				var alpha = Mathf.Lerp(initialAlpha, targetAlpha, time / duration);
				_canvasGroup.alpha = alpha;
				yield return null;
			}

			_canvasGroup.alpha = targetAlpha;
		}
	}
}