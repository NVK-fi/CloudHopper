using UnityEngine;

namespace UI
{
	using System.Collections;
	using Game;
	using TMPro;

	/// <summary>
	/// Updates the in-game score on UI.
	/// </summary>
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class GameScoreVisualizer : MonoBehaviour
	{
		[SerializeField] private float defaultTextPop = 1.3f;
		[SerializeField] private float bigTextPop = 1.6f;
		[SerializeField] private float textPopDuration = 2f;
		
		private TextMeshProUGUI _textContainer;
		private float _defaultFontSize;
		private Coroutine _textPopCoroutine;

		private void Awake()
		{
			_textContainer = GetComponent<TextMeshProUGUI>();
			_defaultFontSize = _textContainer.fontSize;
		}

		private void OnEnable() => Game.Instance.Score.ScoreIncreased += OnScoreIncreased;

		private void OnDisable() => Game.Instance.Score.ScoreIncreased -= OnScoreIncreased;

		private void OnScoreIncreased(int increase)
		{
			if (increase < 1) return;
			
			_textContainer.text = Game.Instance.Score.Current.ToString();
			
			if (_textPopCoroutine != null)
				StopCoroutine(_textPopCoroutine);

			_textPopCoroutine = StartCoroutine(increase > 1 
				? TextPopEffect(bigTextPop, textPopDuration) 
				: TextPopEffect(defaultTextPop, textPopDuration));
		}

		private IEnumerator TextPopEffect(float scale, float duration)
		{
			// Start with the new size and then scale it back to normal over the set duration.
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
