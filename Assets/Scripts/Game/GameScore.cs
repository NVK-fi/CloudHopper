using UnityEngine;

namespace Game
{
	using System;
	using Platforms;
	using Player;
	using Settings;

	/// <summary>
	/// Manages the game score.
	/// </summary>
	public class GameScore : MonoBehaviour
	{
		/// <summary>
		/// Get the current in-game score.
		/// </summary>
		public int Current { get; private set; }

		/// <summary>
		/// The event triggered when the score is increased.
		/// </summary>
		/// <remarks>
		/// The integer argument represents the amount of score increased.
		/// </remarks>
		public event Action<int> ScoreIncreased;

		private void OnEnable()
		{
			GameManager.Instance.Platforms.PlatformSkipped += OnPlatformSkipped;
			Player.Instance.PlatformTouched += OnPlatformTouched;
			Player.Instance.PlayerDeath += OnPlayerDeath;
		}

		private void OnDisable()
		{
			GameManager.Instance.Platforms.PlatformSkipped -= OnPlatformSkipped;
			Player.Instance.PlatformTouched -= OnPlatformTouched;
			Player.Instance.PlayerDeath -= OnPlayerDeath;
		}

		private void OnPlayerDeath()
		{
			var currentHighScore = PlayerPrefs.GetInt(Constants.HIGH_SCORE_KEY, 0);
			if (Current > currentHighScore) 
				PlayerPrefs.SetInt(Constants.HIGH_SCORE_KEY, Current);
			
			PlayerPrefs.SetInt(Constants.LAST_SCORE_KEY, Current);
			PlayerPrefs.Save();
		}

		// Score 1 point when the player hops.
		private void OnPlatformTouched(Platform _)
		{
			Current++;
			ScoreIncreased?.Invoke(1);
		}

		// Score 10 points (1 point + bonus 9 points) when the player skips a platform.
		private void OnPlatformSkipped()
		{
			Current += 9;
			ScoreIncreased?.Invoke(9);
		}
	}
}
