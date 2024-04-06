using UnityEngine;

namespace UI
{
	using System.Collections;
	using Tools;
	using UnityEngine.UI;

	[RequireComponent(typeof(Image))]
	public class ImageFader : MonoBehaviour
	{
		private Image _image;
	
		private void Awake() => _image = GetComponent<Image>();

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
