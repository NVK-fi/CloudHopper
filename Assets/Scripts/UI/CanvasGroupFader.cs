using UnityEngine;

namespace UI
{
	using System.Collections;

	/// <summary>
	/// Handles fading of a CanvasGroup component.
	/// </summary>
	[RequireComponent(typeof(CanvasGroup))]
	public class CanvasGroupFader : MonoBehaviour
	{
		private CanvasGroup _canvasGroup;
	
		private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();
		
		/// <summary>
		/// Fades a CanvasGroup component to a target alpha value over a specified duration.
		/// </summary>
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