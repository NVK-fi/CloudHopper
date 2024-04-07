using UnityEngine;

namespace UI
{
	using System.Collections;
	using Tools;
	using UnityEngine.UI;

	/// <summary>
	/// Handles fading of an Image component.
	/// </summary>
	[RequireComponent(typeof(Image))]
	public class ImageFader : MonoBehaviour
	{
		private Image _image;
	
		private void Awake() => _image = GetComponent<Image>();
		
		/// <summary>
		/// Fades an Image component to a target alpha value over a specified duration.
		/// </summary>
		public IEnumerator FadeTo(float targetAlpha, float duration)
		{
			var initialAlpha = _image.color.a;
			var time = 0f;
			
			while (time < duration)
			{
				time += Time.unscaledDeltaTime;
				var alpha = Mathf.Lerp(initialAlpha, targetAlpha, time / duration);
				_image.color = _image.color.With(a: alpha);
				yield return null;
			}
			_image.color = _image.color.With(a: targetAlpha);
		}
	}
}
