using UnityEngine;

namespace UI
{
	using System.Collections;
	using Game;
	using Platforms;
	using Player;
	using TMPro;

	[RequireComponent(typeof(TextMeshProUGUI))]
	public class UIGameScoreUpdater : MonoBehaviour
	{

		private TextMeshProUGUI _textContainer;
		private float _defaultFontSize;
		private Coroutine _textPopCoroutine;

		private void Awake()
		{
			_textContainer = GetComponent<TextMeshProUGUI>();
			_defaultFontSize = _textContainer.fontSize;
		}

		private void OnEnable()
		{
			Player.Instance.PlatformTouched += OnPlatformTouched;
			GameManager.Instance.PlatformManager.PlatformSkipped += OnPlatformSkipped;
		}

		private void OnDisable()
		{
			Player.Instance.PlatformTouched -= OnPlatformTouched;
			GameManager.Instance.PlatformManager.PlatformSkipped -= OnPlatformSkipped;
		}

		private void OnPlatformTouched(Platform _)
		{
			_textContainer.text = GameManager.Instance.Score.ToString();
			
			if (_textPopCoroutine != null)
				StopCoroutine(_textPopCoroutine);
			_textPopCoroutine = StartCoroutine(TextPopEffect(1.25f, 2f));
		}

		private void OnPlatformSkipped()
		{
			_textContainer.text = GameManager.Instance.Score.ToString();
			
			if (_textPopCoroutine != null)
				StopCoroutine(_textPopCoroutine);
			_textPopCoroutine = StartCoroutine(TextPopEffect(1.5f, 2f));
		}

		private IEnumerator TextPopEffect(float scale, float duration)
		{
			// Start with the scale and then size it back to normal over the set duration.
			_textContainer.fontSize *= scale;
			var time = 0f;
			while (time < duration)
			{
				_textContainer.fontSize = Mathf.Lerp(_textContainer.fontSize, _defaultFontSize, time / duration);
				yield return null;
				time += Time.deltaTime;
			}
			_textContainer.fontSize = _defaultFontSize;
		}
	}
}
